using System.Collections;
using MongoDB.Driver.Core.Configuration;
using Npgsql;
using System.Data;
using System.Data.Common;
using Dapper;
using HrmsBe.Helper;

namespace HrmsBe.StoredProcedure
{
    public static class PostgresFunctionCalls
    {
        public static readonly string HousePagination = "fn_get_houses_pagination";
        public static readonly string RoomCategoriesPagination = "fn_get_room_categories_pagination";
        public static readonly string RenterTypesPagination = "fn_get_renter_types_pagination";
        public static readonly string RoomPagination = "fn_get_room_pagination";

        public static (DataTable dataTable, int totalCount) GetPaginatedData(DbConnection dbConnection, string functionName, Dictionary<string, object> parameters)
        {
            try
            {
                var query = $"SELECT * FROM public.{functionName}(";
                query += string.Join(", ", parameters.Keys.Select(k => $"@{k}"));
                query += ");";

                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }

                using var transaction = dbConnection.BeginTransaction();

                // Execute the function and get data
                dbConnection.Query(query, parameters, transaction: transaction);
                var data = dbConnection.Query("FETCH ALL IN \"ref1\";", transaction: transaction).ToList();
                var totalCount = dbConnection.QuerySingle<int>("FETCH ALL IN \"total_count\";", transaction: transaction);

                transaction.Commit();

                var dataTable = CommonHelper.ConvertDynamicListToDataTable(data);

                return (dataTable, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static (DataTable dataTable, int TotalCount) GetHousesPagination(DbConnection dbConnection, string search, string userId, int pageNumber, int pageSize)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Search", search },
                { "UserId", userId },
                { "PageNo", pageNumber },
                { "PageSize", pageSize }
            };

            return GetPaginatedData(dbConnection, $"{HousePagination}", parameters);
        }
        public static (DataTable dataTable, int TotalCount) GetRoomCategoriesPagination(DbConnection dbConnection, string search, long houseId, int pageNumber, int pageSize)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Search", search },
                { "HouseId", houseId },
                { "PageNo", pageNumber },
                { "PageSize", pageSize }
            };

            return GetPaginatedData(dbConnection, $"{RoomCategoriesPagination}", parameters);
        }
        public static (DataTable dataTable, int TotalCount) GetRenterTypesPagination(DbConnection dbConnection, string search, string userId, int pageNumber, int pageSize)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Search", search },
                { "UserId", userId },
                { "PageNo", pageNumber },
                { "PageSize", pageSize }
            };

            return GetPaginatedData(dbConnection, $"{RenterTypesPagination}", parameters);
        }
        public static (DataTable dataTable, int TotalCount) GetRoomPagination(DbConnection dbConnection, string? search,long houseId,long roomCategoryId, string userId, int pageNumber, int pageSize)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Search", search ?? string.Empty },
                { "HouseId", houseId },
                { "RoomCategoryId", roomCategoryId },
                { "UserId", userId },
                { "PageNo", pageNumber },
                { "PageSize", pageSize }
            };

            return GetPaginatedData(dbConnection, $"{RoomPagination}", parameters);
        }
    }
}
