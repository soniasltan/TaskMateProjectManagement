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
    public class TeamResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public List<dynamic> Data { set; get; }
    }

    public class TeamStatusResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }

    }

    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private IConfiguration _configuration;

        public TeamController(IConfiguration config)
        {
            _configuration = config;
        }

        // GET: api/values
        [HttpGet]
        public TeamResponseModel Get()
        {
            TeamResponseModel _objResponseModel = new TeamResponseModel();

            string query = @"
                            select * from
                            team
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

            List<dynamic> teamsList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                Team teams = new Team();
                teams.Id = Convert.ToInt32(table.Rows[i]["id"]);
                teams.TeamName = table.Rows[i]["team_name"].ToString();
                teams.ProjectManagerId = Convert.ToInt32(table.Rows[i]["project_manager_id"]);
                teamsList.Add(teams);
            }


            _objResponseModel.Data = teamsList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Teams received successfully";
            return _objResponseModel;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public TeamResponseModel Get(int id)
        {
            TeamResponseModel _objResponseModel = new TeamResponseModel();

            string query = @"
                            select * from
                            team where id=@id
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

            List<dynamic> teamsList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                Team teams = new Team();
                teams.Id = Convert.ToInt32(table.Rows[i]["id"]);
                teams.TeamName = table.Rows[i]["team_name"].ToString();
                teams.ProjectManagerId = Convert.ToInt32(table.Rows[i]["project_manager_id"]);
                teamsList.Add(teams);
            }


            _objResponseModel.Data = teamsList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Team received successfully";
            return _objResponseModel;

        }

        // POST api/values
        [HttpPost]
        public TeamStatusResponseModel Post(Team teamdata)
        {
            TeamStatusResponseModel _objResponseModel = new TeamStatusResponseModel();

            string query = @"
                            insert into team
                            (team_name, project_manager_id) values (@team_name, @project_manager_id)
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand addTeam = new SqlCommand(query, myCon))
                {
                    addTeam.Parameters.AddWithValue("@team_name", teamdata.TeamName);
                    addTeam.Parameters.AddWithValue("@project_manager_id", teamdata.ProjectManagerId);

                    myReader = addTeam.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            _objResponseModel.Status = true;
            _objResponseModel.Message = "New team created successfully.";
            return _objResponseModel;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public TeamStatusResponseModel Put(Team teamdata)
        {
            TeamStatusResponseModel _objResponseModel = new TeamStatusResponseModel();

            string query = @"
                           update team set
                           team_name = @team_name,
                           project_manager_id = @project_manager_id
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
                    myCommand.Parameters.AddWithValue("@id", teamdata.Id);
                    myCommand.Parameters.AddWithValue("@team_name", teamdata.TeamName);
                    myCommand.Parameters.AddWithValue("@project_manager_id", teamdata.ProjectManagerId);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            _objResponseModel.Status = true;
            _objResponseModel.Message = "Team updated successfully";
            return _objResponseModel;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public TeamStatusResponseModel Delete(int id)
        {
            TeamStatusResponseModel _objResponseModel = new TeamStatusResponseModel();

            string query = @"
                           delete from team where id=@id
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
            _objResponseModel.Message = "Team deleted successfully";
            return _objResponseModel;

        }
    }
}