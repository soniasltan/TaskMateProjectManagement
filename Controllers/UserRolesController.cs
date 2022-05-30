using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using JWTProjectManagement.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectManagement.Controllers
{
    public class ResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public List<dynamic> Data { set; get; }
    }

    public class StatusResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : Controller
    {
        private IConfiguration _configuration;

        public UserRolesController(IConfiguration config)
        {
            _configuration = config;
        }

        // GET: api/values
        [HttpGet]
        public ResponseModel Get()
        {
            ResponseModel _objResponseModel = new ResponseModel();

            string query = @"
                            select * from
                            user_roles
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            List<dynamic> userRolesList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                UserRole roles = new UserRole();
                roles.Id = Convert.ToInt32(table.Rows[i]["id"]);
                roles.Role = table.Rows[i]["role"].ToString();
                userRolesList.Add(roles);
            }


            _objResponseModel.Data = userRolesList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "User Roles Data Received successfully";
            return _objResponseModel;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ResponseModel Get(int id)
        {
            ResponseModel _objResponseModel = new ResponseModel();

            string query = @"
                            select * from
                            user_roles where id=@id
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            List<dynamic> userRolesList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                UserRole roles = new UserRole();
                roles.Id = Convert.ToInt32(table.Rows[i]["id"]);
                roles.Role = table.Rows[i]["role"].ToString();
                userRolesList.Add(roles);
            }


            _objResponseModel.Data = userRolesList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "User Roles Data Received successfully";
            return _objResponseModel;

        }

        // POST api/values
        [HttpPost]
        public StatusResponseModel Post(UserRole rolesdata)
        {
            StatusResponseModel _objResponseModel = new StatusResponseModel();

            string query = @"
                            insert into user_roles
                            (role) values (@role)
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand addRole = new SqlCommand(query, myCon))
                {
                    addRole.Parameters.AddWithValue("@role", rolesdata.Role);

                    myReader = addRole.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            _objResponseModel.Status = true;
            _objResponseModel.Message = "New role created successfully.";
            return _objResponseModel;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public StatusResponseModel Put(UserRole rolesdata)
        {
            StatusResponseModel _objResponseModel = new StatusResponseModel();

            string query = @"
                           update user_roles set
                           role = @role
                           where id=@id
                           ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", rolesdata.Id);
                    myCommand.Parameters.AddWithValue("@role", rolesdata.Role);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            _objResponseModel.Status = true;
            _objResponseModel.Message = "User role updated successfully";
            return _objResponseModel;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public StatusResponseModel Delete(int id)
        {
            StatusResponseModel _objResponseModel = new StatusResponseModel();

            string query = @"
                           delete from user_roles where id=@id
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            _objResponseModel.Status = true;
            _objResponseModel.Message = "User role deleted successfully";
            return _objResponseModel;

        }
    }
}

