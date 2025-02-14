using System.Data;
using DxReportingWeb.Models;
using Microsoft.Data.SqlClient;
namespace DxReportingWeb.Services
{
    public class MyDataService
    {
        private readonly IConfiguration _configuration;

        public MyDataService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<MyData[]> GetEmployeesAsync(string startDate, string endDate, int someInt, string someString)
        {
            var connectionString = "Server=DESKTOP-V4L8T0J;Database=SSS;Trusted_Connection=True;TrustServerCertificate=True;";
            var MyDatas = new List<MyData>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("SP_STOCK_REPORT_DC_ONLINE", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar) { Value = startDate });
                command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.Date) { Value = endDate });
                command.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.Int) { Value = someInt });
                command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = someString });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        MyDatas.Add(new MyData
                        {
                            Price = reader.GetInt32(0),
                            Article = reader.GetString(1),
                            Code = reader.GetString(2)
                        });
                    }
                }
            }

            return MyDatas.ToArray();
        }

    }
}
