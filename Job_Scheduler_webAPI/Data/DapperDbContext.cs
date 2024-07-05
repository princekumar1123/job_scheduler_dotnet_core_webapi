using Microsoft.Data.SqlClient;
using System.Data;

namespace Job_Scheduler_webAPI.Data
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string ConnectionStrings;

        public DapperDbContext(IConfiguration configuration, string connectionStrings)
        {
            this._configuration = configuration;
            this.ConnectionStrings = this._configuration.GetConnectionString("Job_Scheduler_webAPIContext");
        }

        public IDbConnection CreateConnection() => new SqlConnection(this.ConnectionStrings);
    }
}
