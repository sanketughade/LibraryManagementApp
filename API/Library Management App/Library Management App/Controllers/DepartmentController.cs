using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace Library_Management_App.Controllers
{

    public class DepartmentController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                SELECT departmentid, departmentname FROM department
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            NpgsqlDataReader npgsqlDataReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    npgsqlDataReader = myCommand.ExecuteReader();
                    table.Load(npgsqlDataReader);

                    npgsqlDataReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
