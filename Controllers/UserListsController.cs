using System.Data;
using JWTProjectManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectManagement.Controllers
{
    public class UserListResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public List<dynamic> Data { set; get; }
    }

    public class UserListStatusResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }

    }

    public class ProfileData
    {
        public int Id { set; get; }
        public string Phonenumber { set; get; }
        public string Address { set; get; }
    }

    public class PasswordData
    {
        public int Id { set; get; }
        public string CurrentPassword { set; get; }
        public string NewPassword { set; get; }
    }

    public class RoleData
    {
        public int Id { set; get; }
        public int UserRolesId { set; get; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UserListsController : Controller
    {
        private IConfiguration _configuration;

        public UserListsController(IConfiguration config)
        {
            _configuration = config;
        }

        // GET: api/values
        [HttpGet]
        public UserListResponseModel Get()
        {
            UserListResponseModel _objResponseModel = new UserListResponseModel();

            string query = @"
                            select * from
                            user_list
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

            List<dynamic> userList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                UserList users = new UserList();
                users.Id = Convert.ToInt32(table.Rows[i]["id"]);
                users.Username = table.Rows[i]["username"].ToString();
                users.Password = table.Rows[i]["password"].ToString();
                users.UserRolesId = Convert.ToInt32(table.Rows[i]["user_roles_id"]);
                users.Email = table.Rows[i]["email"].ToString();
                users.Phonenumber = table.Rows[i]["phonenumber"].ToString();
                users.Address = table.Rows[i]["address"].ToString();
                users.FirstName = table.Rows[i]["first_name"].ToString();
                users.LastName = table.Rows[i]["last_name"].ToString();
                userList.Add(users);
            }


            _objResponseModel.Data = userList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "User Data Received successfully";
            return _objResponseModel;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public UserListResponseModel Get(int id)
        {
            UserListResponseModel _objResponseModel = new UserListResponseModel();

            string query = @"
                            select * from
                            user_list where id=@id
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

            List<dynamic> userList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                UserList users = new UserList();
                users.Id = Convert.ToInt32(table.Rows[i]["id"]);
                users.Username = table.Rows[i]["username"].ToString();
                users.Password = table.Rows[i]["password"].ToString();
                users.UserRolesId = Convert.ToInt32(table.Rows[i]["user_roles_id"]);
                users.Email = table.Rows[i]["email"].ToString();
                users.Phonenumber = table.Rows[i]["phonenumber"].ToString();
                users.Address = table.Rows[i]["address"].ToString();
                users.FirstName = table.Rows[i]["first_name"].ToString();
                users.LastName = table.Rows[i]["last_name"].ToString();
                userList.Add(users);
            }


            _objResponseModel.Data = userList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "User Data Received successfully";
            return _objResponseModel;

        }


        // POST api/values
        [HttpPost]
        public ActionResult<string> Post(UserList userdata)
        {
            UserListResponseModel _objResponseModel = new UserListResponseModel();

            string query = @"
                            insert into user_list
                            (username, password, user_roles_id, first_name, last_name, email, phonenumber, address, team_id) values (@username, @password, @user_roles_id, @first_name, @last_name, @email, @phonenumber, @address, @team_id)
                            ";

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
                    checkUser.Parameters.AddWithValue("@username", userdata.Username);

                    myReader = checkUser.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }

                if (table.Rows.Count > 0)
                {
                    myCon.Close();
                    return BadRequest("User already exists.");
                }
                else
                {
                    using (SqlCommand createNewUser = new SqlCommand(query, myCon))
                    {
                        createNewUser.Parameters.AddWithValue("@username", userdata.Username);
                        createNewUser.Parameters.AddWithValue("@password", userdata.Password);
                        createNewUser.Parameters.AddWithValue("@user_roles_id", userdata.UserRolesId);
                        createNewUser.Parameters.AddWithValue("@first_name", userdata.FirstName);
                        createNewUser.Parameters.AddWithValue("@last_name", userdata.LastName);
                        createNewUser.Parameters.AddWithValue("@email", userdata.Email);
                        createNewUser.Parameters.AddWithValue("@phonenumber", userdata.Phonenumber);
                        createNewUser.Parameters.AddWithValue("@address", userdata.Address);

                        myReader = createNewUser.ExecuteReader();
                        myReader.Close();
                    }
                    using (SqlCommand retrieveNewUser = new SqlCommand(queryUser, myCon))
                    {
                        retrieveNewUser.Parameters.AddWithValue("@username", userdata.Username);

                        myReader = retrieveNewUser.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }

                    List<dynamic> userList = new List<dynamic>();
                    for (int i = 0; i < table.Rows.Count; i++)
                    {

                        UserList users = new UserList();
                        users.Id = Convert.ToInt32(table.Rows[i]["id"]);
                        users.Username = table.Rows[i]["username"].ToString();
                        users.Password = table.Rows[i]["password"].ToString();
                        users.UserRolesId = Convert.ToInt32(table.Rows[i]["user_roles_id"]);
                        users.Email = table.Rows[i]["email"].ToString();
                        users.Phonenumber = table.Rows[i]["phonenumber"].ToString();
                        users.Address = table.Rows[i]["address"].ToString();
                        users.FirstName = table.Rows[i]["first_name"].ToString();
                        users.LastName = table.Rows[i]["last_name"].ToString();
                        userList.Add(users);
                    }

                    _objResponseModel.Data = userList;
                    _objResponseModel.Status = true;
                    _objResponseModel.Message = "New user created successfully.";
                    return Ok(_objResponseModel);

                }

            }

            //List<dynamic> userList = new List<dynamic>();
            //for (int i = 0; i < table.Rows.Count; i++)
            //{

            //    UserList users = new UserList();
            //    users.Id = Convert.ToInt32(table.Rows[i]["id"]);
            //    users.Username = table.Rows[i]["username"].ToString();
            //    users.Password = table.Rows[i]["password"].ToString();
            //    users.UserRolesId = Convert.ToInt32(table.Rows[i]["user_roles_id"]);
            //    userList.Add(users);
            //}

            //_objResponseModel.Data = userList;
            //_objResponseModel.Status = true;
            //_objResponseModel.Message = "New user created successfully.";
            //return _objResponseModel;
        }


        // PUT api/values/5
        [HttpPut("EditDetails")]
        public UserListStatusResponseModel Put(ProfileData profileData)
        {
            UserListStatusResponseModel _objResponseModel = new UserListStatusResponseModel();

            string query = @"
                           update user_list set
                           phonenumber = @phonenumber,
                           address = @address
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
                    myCommand.Parameters.AddWithValue("@id", profileData.Id);
                    myCommand.Parameters.AddWithValue("@phonenumber", profileData.Phonenumber);
                    myCommand.Parameters.AddWithValue("@address", profileData.Address);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            _objResponseModel.Status = true;
            _objResponseModel.Message = "User details updated successfully";
            return _objResponseModel;
        }

        [HttpPut("EditPassword")]
        public ActionResult<string> Put(PasswordData passwordData)
        {
            UserListStatusResponseModel _objResponseModel = new UserListStatusResponseModel();

            string checkpw = @"
                           select * from user_list
                           where id=@id
                           ";

            string query = @"
                           update user_list set
                           password = @password
                           where id=@id
                           ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(checkpw, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", passwordData.Id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            if (table.Rows[0]["password"].ToString() != passwordData.CurrentPassword)
            {
                myCon.Close();
                return BadRequest("Current password is incorrect.");
            }
            else
            {
                using (SqlCommand setNewPassword = new SqlCommand(query, myCon))
                {
                    setNewPassword.Parameters.AddWithValue("@id", passwordData.Id);
                    setNewPassword.Parameters.AddWithValue("@password", passwordData.NewPassword);

                    myReader = setNewPassword.ExecuteReader();
                    myReader.Close();
                    myCon.Close();
                    }

                _objResponseModel.Status = true;
                _objResponseModel.Message = "Password updated successfully.";
                return Ok(_objResponseModel);
            }

        }
    }


        [HttpPut("EditRole")]
        public UserListStatusResponseModel Put(RoleData roleData)
        {
            UserListStatusResponseModel _objResponseModel = new UserListStatusResponseModel();

            string query = @"
                           update user_list set
                           user_roles_id = @user_roles_id
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
                    myCommand.Parameters.AddWithValue("@id", roleData.Id);
                    myCommand.Parameters.AddWithValue("@user_roles_id", roleData.UserRolesId);

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
        public UserListStatusResponseModel Delete(int id)
        {
            UserListStatusResponseModel _objResponseModel = new UserListStatusResponseModel();

            string query = @"
                           delete from user_list where id=@id
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
            _objResponseModel.Message = "User deleted successfully";
            return _objResponseModel;

        }
    }
}

