using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HrmsBe.Helper
{
    public static class CommonHelper
    {
        //public void CreateUser(string password)
        //{
        //    string hashedPassword = BCrypt.HashPassword(password);
        //    // Save hashedPassword to your database
        //}

        //public bool ValidateUser(string enteredPassword, string storedHashedPassword)
        //{
        //    return BCrypt.Verify(enteredPassword, storedHashedPassword);
        //}
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
                foreach (var property in properties.Where(p => p.PropertyType == typeof(Ulid) || p.PropertyType == typeof(Ulid?)))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(property.Name)
                        .HasConversion(ulidConverter);
                }
            }
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
    
}
