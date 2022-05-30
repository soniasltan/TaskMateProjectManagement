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
    public class ProjectMembersResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public List<dynamic> Data { set; get; }
    }

    public class ProjectMembersStatusResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class ProjectMembersController : Controller
    {
        private IConfiguration _configuration;

        public ProjectMembersController(IConfiguration config)
        {
            _configuration = config;
        }

        // GET: api/values
        [HttpGet]
        public ProjectMembersResponseModel Get()
        {
            ProjectMembersResponseModel _objResponseModel = new ProjectMembersResponseModel();

            string query = @"
                            select * from
                            project_members
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

            List<dynamic> membersList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                ProjectMember member = new ProjectMember();
                member.Id = Convert.ToInt32(table.Rows[i]["id"]);
                member.ProjectId = Convert.ToInt32(table.Rows[i]["project_id"]);
                member.UserListId = Convert.ToInt32(table.Rows[i]["user_list_id"]);
                membersList.Add(member);
            }


            _objResponseModel.Data = membersList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Project members received successfully";
            return _objResponseModel;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ProjectMembersResponseModel Get(int id)
        {
            ProjectMembersResponseModel _objResponseModel = new ProjectMembersResponseModel();

            string query = @"
                            select * from
                            project_members where id=@id
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

            List<dynamic> membersList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                ProjectMember member = new ProjectMember();
                member.Id = Convert.ToInt32(table.Rows[i]["id"]);
                member.ProjectId = Convert.ToInt32(table.Rows[i]["project_id"]);
                member.UserListId = Convert.ToInt32(table.Rows[i]["user_list_id"]);
                membersList.Add(member);
            }


            _objResponseModel.Data = membersList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Project members received successfully";
            return _objResponseModel;

        }

        // POST api/values
        [HttpPost]
        public ProjectMembersStatusResponseModel Post(ProjectMember memberdata)
        {
            ProjectMembersStatusResponseModel _objResponseModel = new ProjectMembersStatusResponseModel();

            string query = @"
                            insert into project_members
                            (project_id, user_list_id) values (@project_id, @user_list_id)
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand addTeam = new SqlCommand(query, myCon))
                {
                    addTeam.Parameters.AddWithValue("@project_id", memberdata.ProjectId);
                    addTeam.Parameters.AddWithValue("@user_list_id", memberdata.UserListId);

                    myReader = addTeam.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            _objResponseModel.Status = true;
            _objResponseModel.Message = "New project member created successfully.";
            return _objResponseModel;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ProjectMembersStatusResponseModel Put(ProjectMember memberdata)
        {
            ProjectMembersStatusResponseModel _objResponseModel = new ProjectMembersStatusResponseModel();

            string query = @"
                           update project_members set
                           project_id = @project_id,
                           user_list_id = @user_list_id
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
                    myCommand.Parameters.AddWithValue("@id", memberdata.Id);
                    myCommand.Parameters.AddWithValue("@project_id", memberdata.ProjectId);
                    myCommand.Parameters.AddWithValue("@user_list_id", memberdata.UserListId);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            _objResponseModel.Status = true;
            _objResponseModel.Message = "Project member updated successfully";
            return _objResponseModel;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ProjectMembersStatusResponseModel Delete(int id)
        {
            ProjectMembersStatusResponseModel _objResponseModel = new ProjectMembersStatusResponseModel();

            string query = @"
                           delete from project_members where id=@id
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
            _objResponseModel.Message = "Project member deleted successfully";
            return _objResponseModel;

        }
    }
}

