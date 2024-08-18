using HrmsBe.Dto.V1.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data;
using System.Net;

namespace HrmsBe.Helper
{
    public static class CommonHelper
    {
        public static DateTime CurrentDateTime()
        {
            return DateTime.UtcNow.AddHours(6);
        }

        public static void ApplyCommonConfigurations(ModelBuilder modelBuilder)
        {
            var ulidConverter = new UlidToStringConverter();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Retrieve the properties of the current entity type
                var properties = entityType.ClrType.GetProperties();

                // Apply default value configuration for CreatedAt
                if (properties.Any(p => p.Name == "CreatedAt"))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("CreatedAt")
                        .HasDefaultValueSql("timezone('UTC', now() + interval '6 hours')");
                }

                // Apply default value configuration for ServerActionDateTime
                if (properties.Any(p => p.Name == "ServerActionDateTime"))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("ServerActionDateTime")
                        .HasDefaultValueSql("timezone('UTC', now() + interval '6 hours')");
                }

                // Apply Ulid to string conversion for properties of type Ulid or Ulid?
                foreach (var property in properties.Where(p =>
                             p.PropertyType == typeof(Ulid) || p.PropertyType == typeof(Ulid?)))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(property.Name)
                        .HasConversion(ulidConverter);
                }
            }
        }

        public static Ulid StringToUlidConverter(string userId)
        {
            try
            {
                if (Ulid.TryParse(userId, out Ulid user))
                {
                    return user;
                }
                else
                {
                    throw new ArgumentException("Invalid User ID format.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        public class UlidToStringConverter : ValueConverter<Ulid, string>
        {
            public UlidToStringConverter() : base(
                ulid => ulid.ToString(), // Convert Ulid to string
                value => Ulid.Parse(value) // Convert string to Ulid
            )
            {
            }
        }

        public static DataTable ConvertDynamicListToDataTable(IEnumerable<dynamic> items)
        {
            var dataTable = new DataTable();

            if (items == null || !items.Any())
            {
                return dataTable;
            }

            // Extract the properties from the first item to create the columns
            var firstItem = items.First();
            var properties = ((IDictionary<string, object>)firstItem).Keys;

            // Create the columns in the DataTable
            foreach (var prop in properties)
            {
                dataTable.Columns.Add(prop);
            }

            // Populate the DataTable with the items
            foreach (var item in items)
            {
                var row = dataTable.NewRow();
                var values = ((IDictionary<string, object>)item).Values;

                int i = 0;
                foreach (var value in values)
                {
                    row[i] = value ?? DBNull.Value;
                    i++;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public static List<T> ConvertDataTableToList<T>(DataTable dataTable) where T : new()
        {
            var modelList = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                var model = new T();
                foreach (DataColumn column in dataTable.Columns)
                {
                    // Get the property with the same name as the column name
                    var property = typeof(T).GetProperty(column.ColumnName);
                    if (property != null && row[column] != DBNull.Value)
                    {
                        // Set the value of the property from the DataRow
                        property.SetValue(model, Convert.ChangeType(row[column], property.PropertyType));
                    }
                }
                modelList.Add(model);
            }

            return modelList;
        }
        public static T ConvertDataTableToSingleModel<T>(DataTable dataTable) where T : new()
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return default;
            }

            var model = new T();
            DataRow row = dataTable.Rows[0];

            foreach (DataColumn column in dataTable.Columns)
            {
                var property = typeof(T).GetProperty(column.ColumnName);
                if (property != null && row[column] != DBNull.Value)
                {
                    property.SetValue(model, Convert.ChangeType(row[column], property.PropertyType));
                }
            }

            return model;
        }


    }
}
