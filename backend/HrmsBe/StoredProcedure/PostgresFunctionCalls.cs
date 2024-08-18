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

        //public static (DataTable dataTable, int TotalCount) GetHousesPagination(DbConnection dbConnection,string search, string userId,
        //    int pageNumber, int pageSize)
        //{
        //    try
        //    {
        //        var query = $"SELECT * FROM public.{HousePagination}(@Search, @UserId, @PageNo, @PageSize);";

        //        using
        //            var connection = new NpgsqlConnection(DbConnection.ConnectionString);

        //        connection.Open();

        //        using
        //            var transaction = connection.BeginTransaction();

        //        var functionResult = connection.Query(query, new
        //        {
        //            UserId = userId,
        //            pageNumber = pageNumber,
        //            PageSize = pageSize
        //        }, transaction: transaction);

        //        var data = connection.Query(sql: "fetch_all_in \"ref1\";").ToList();

        //        var totalCount = connection.QuerySingle<int>(sql: "fetch all in \"total_count\";");

        //        transaction.Commit();

        //        var dataTable = DataTableToModelConversion.ConvertToDataTable(data);

        //        return (dataTable,
        //            totalCount);

        //    }
        //    catch (
        //        Exception e)
        //    {
        //        throw new Exception(e.Message);

        //    }
        //}
        public static (DataTable dataTable, int TotalCount) GetHousesPagination(DbConnection dbConnection, string search, string userId, int pageNumber, int pageSize)
        {
            try
            {
                // Create the query for the function
                var query = $"SELECT * FROM public.{HousePagination}(@Search, @UserId, @PageNo, @PageSize);";
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }

                using var transaction = dbConnection.BeginTransaction();

                var functionResult = dbConnection.Query(query, new
                {
                    Search = search,
                    UserId = userId,
                    PageNo = pageNumber,
                    PageSize = pageSize
                }, transaction: transaction);
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

    }
}
