using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using JWTProjectManagement.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTProjectManagement.Controllers
{
    [Route("api/[controller]")]
    public class LoginResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public UserList Data { set; get; }
    }

    public class PMResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public List<dynamic> Data { set; get; }
    }

    public class CheckUserResponseModel
    {
        public string Message { set; get; }
        public string Status { set; get; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IConfiguration _configuration;

        public UserController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost("login")]
        public ActionResult<string> Post(UserDto request)
        {
            LoginResponseModel _objResponseModel = new LoginResponseModel();

            string query = @"
                            select * from
                            user_list where username=@username
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@username", request.Username);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            int recordcount = table.Rows.Count;

            if (recordcount > 0)
            {
                if (table.Rows[0]["password"].ToString() == request.Password)
                {
                        UserList user = new UserList();
                        user.Id = Convert.ToInt32(table.Rows[0]["id"]);
                        user.Username = table.Rows[0]["username"].ToString();
                        user.Password = table.Rows[0]["password"].ToString();
                        user.UserRolesId = Convert.ToInt32(table.Rows[0]["user_roles_id"]);
                        user.Email = table.Rows[0]["email"].ToString();
                        user.Phonenumber = table.Rows[0]["phonenumber"].ToString();
                        user.Address = table.Rows[0]["address"].ToString();
                        user.FirstName = table.Rows[0]["first_name"].ToString();
                        user.LastName = table.Rows[0]["last_name"].ToString();


                    _objResponseModel.Data = user;
                    _objResponseModel.Status = true;
                    _objResponseModel.Message = "User logged in successfully";
                    return Ok(_objResponseModel);
                }
                else
                {
                    return BadRequest("Password does not match.");
                }
            }
            else
            {
                return BadRequest("User not found.");
            }

        }

        // GET api/values/5
        [HttpGet("ProjectTeam")]
        public PMResponseModel Get(int UserId)
        {
            PMResponseModel _objResponseModel = new PMResponseModel();

            string query = @"
                            select * from
                            project_members where user_list_id=@user_list_id
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@user_list_id", UserId);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            List<dynamic> projectTeamList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                ProjectMember projectTeam = new ProjectMember();
                projectTeam.Id = Convert.ToInt32(table.Rows[i]["id"]);
                projectTeam.UserListId = Convert.ToInt32(table.Rows[i]["user_list_id"]);
                projectTeam.ProjectId = Convert.ToInt32(table.Rows[i]["project_id"]);
                projectTeamList.Add(projectTeam);
            }


            _objResponseModel.Data = projectTeamList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "User project team data received successfully";
            return _objResponseModel;

        }

        [HttpGet("CheckUser")]
        public CheckUserResponseModel Get(string username)
        {
            CheckUserResponseModel _objResponseModel = new CheckUserResponseModel();

            string queryUser = @"
                            select * from
                            user_list where username = @username
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand checkUser = new SqlCommand(queryUser, myCon))
                {
                    checkUser.Parameters.AddWithValue("@username", username);

                    myReader = checkUser.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }

                if (table.Rows.Count > 0)
                {
                    _objResponseModel.Status = "unavailable";
                    _objResponseModel.Message = "Username '"+username+ "' already exists. Please try again.";
                    return _objResponseModel;
                }
                else
                {
                    _objResponseModel.Status = "available";
                    _objResponseModel.Message = "Username is available.";
                    return _objResponseModel;

                }

            }
        }

            //// POST api/values
            //[HttpPost]
            //public void Post([FromBody]string value)
            //{
            //}

            //// PUT api/values/5
            //[HttpPut("{id}")]
            //public void Put(int id, [FromBody]string value)
            //{
            //}

            //// DELETE api/values/5
            //[HttpDelete("{id}")]
            //public void Delete(int id)
            //{
            //}
        }
}

