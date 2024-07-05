using Job_Scheduler_webAPI.Controllers;
using Job_Scheduler_webAPI.Data;
using Job_Scheduler_webAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using OfficeOpenXml;
using Quartz;
using System.Configuration;
using System.Diagnostics.Metrics;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Xml.Linq;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Job_Scheduler_webAPI.Jobs
{

    public class ImportData
    {

        private readonly IConfiguration _configuration;

        public ImportData(IConfiguration config)
        {
            _configuration = config;
        }

        public ImportData()
        {

        }
        public  List<Countries> Import()
        {

            try
            {
                var pathfile = "D:\\PrinceRecent\\internal_Excel\\countryCode.xlsx";
                var List = new List<Countries>();
           
                using (var package = new ExcelPackage(pathfile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var RowCount = worksheet.Dimension.Rows;

                    if (RowCount > 0)
                    {

                        string connectionstring = _configuration.GetConnectionString("Job_Scheduler_webAPIContext");
                        SqlConnection connection = new SqlConnection(connectionstring);
                        connection.Open();
                        SqlCommand truncateCmd = new SqlCommand("TRUNCATE TABLE Countries", connection);
                        truncateCmd.ExecuteNonQuery();
                        connection.Close();

                        for (int row = 2; row <= RowCount; row++)
                        {
                            List.Add(new Countries
                            {
                                CountryID = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                CountryName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                TwoCharCountryCode = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                ThreeCharCountryCode = worksheet.Cells[row, 4].Value.ToString().Trim(),
                            });

                            connection.Open();
                            SqlCommand cmd = new SqlCommand("INSERT INTO Countries (CountryID,CountryName,TwoCharCountryCode,ThreeCharCountryCode) VALUES (@id,@name,@twoCode,@threeCode)", connection);
                            cmd.Parameters.AddWithValue("@id", List.Last().CountryID);
                            cmd.Parameters.AddWithValue("@name", List.Last().CountryName);
                            cmd.Parameters.AddWithValue("@twoCode", List.Last().TwoCharCountryCode);
                            cmd.Parameters.AddWithValue("@threeCode", List.Last().ThreeCharCountryCode);
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }

                    }

                    
                }
               
                return List;
            }
            catch (Exception ex)
            {

               throw new Exception(ex.Message);
            }
        


        }



        public void setData(Countries data)
        {
            //if (name == null) throw new ArgumentNullException("name");
            //else if (desc == null) throw new ArgumentNullException("desc");
            //else if (email == null) throw new ArgumentNullException("email");
            //else
            try
            {
                //var constr = System.Configuration.ConfigurationManager.ConnectionStrings["Job_Scheduler_webAPIContext"].ConnectionString;
                //string connectionstring = _configuration.GetConnectionString("Job_Scheduler_webAPIContext");
                string connectionstring = _configuration.GetConnectionString("Job_Scheduler_webAPIContext");
                SqlConnection connection = new SqlConnection(connectionstring);
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Countries (CountryID,CountryName,TwoCharCountryCode,ThreeCharCountryCode) VALUES (@id,@name,@twoCode,@threeCode)", connection);
                cmd.Parameters.AddWithValue("@id", data.CountryID);
                cmd.Parameters.AddWithValue("@name", data.CountryName);
                cmd.Parameters.AddWithValue("@twoCode", data.TwoCharCountryCode);
                cmd.Parameters.AddWithValue("@threeCode", data.ThreeCharCountryCode);

                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }


           
            
        }


    }
    public class NotificationJob : Controller, IJob
    {

        private readonly IConfiguration con;
        public NotificationJob(IConfiguration configuration)
        {
            con = configuration;
        }


        public Task Execute(IJobExecutionContext context)
        {
            ImportData d = new ImportData(con);
            var data= d.Import();


            
            //d.setData(data[0]);

            //CountriesController l = new();
            //var ListData=l.Import();

            //CountriesCrud c = new ();
            //var a = c.Create(ListData[0]);

            //Console.WriteLine("aaa",a);

            //Console.WriteLine("ListData" + ListData[0].CountryName);

            Console.WriteLine($"Notify user at {DateTime.Now} and Job Type {context.JobDetail.JobType}");

            return Task.CompletedTask;
        }
    }
}