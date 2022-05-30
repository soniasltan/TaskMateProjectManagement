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
    public class FollowUpsResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public List<dynamic> Data { set; get; }
    }

    public class FollowUpsStatusResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class FollowUpsController : Controller
    {
        private IConfiguration _configuration;

        public FollowUpsController(IConfiguration config)
        {
            _configuration = config;
        }

        // GET: api/values
        [HttpGet]
        public FollowUpsResponseModel Get()
        {
            FollowUpsResponseModel _objResponseModel = new FollowUpsResponseModel();

            string query = @"
                            select * from
                            follow_ups
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

            List<dynamic> followUpsList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                FollowUp followUps = new FollowUp();
                followUps.Id = Convert.ToInt32(table.Rows[i]["id"]);
                followUps.TaskId = Convert.ToInt32(table.Rows[i]["task_id"]);
                followUps.Notes = table.Rows[i]["notes"].ToString();
                followUps.CreatedDate = table.Rows[i]["created_date"].ToString();
                followUps.UpdatedById = Convert.ToInt32(table.Rows[i]["updated_by_id"]);
                followUpsList.Add(followUps);
            }


            _objResponseModel.Data = followUpsList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Follow ups received successfully";
            return _objResponseModel;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public FollowUpsResponseModel Get(int id)
        {
            FollowUpsResponseModel _objResponseModel = new FollowUpsResponseModel();

            string query = @"
                            select * from
                            follow_ups where id=@id
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

            List<dynamic> followUpsList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                FollowUp followUps = new FollowUp();
                followUps.Id = Convert.ToInt32(table.Rows[i]["id"]);
                followUps.TaskId = Convert.ToInt32(table.Rows[i]["task_id"]);
                followUps.Notes = table.Rows[i]["notes"].ToString();
                followUps.CreatedDate = table.Rows[i]["created_date"].ToString();
                followUps.UpdatedById = Convert.ToInt32(table.Rows[i]["updated_by_id"]);
                followUpsList.Add(followUps);
            }


            _objResponseModel.Data = followUpsList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Follow ups received successfully";
            return _objResponseModel;

        }

        // POST api/values
        [HttpPost]
        public FollowUpsStatusResponseModel Post(FollowUp followupsdata)
        {
            FollowUpsStatusResponseModel _objResponseModel = new FollowUpsStatusResponseModel();

            string query = @"
                            insert into follow_ups
                            (task_id, notes, created_date, updated_by_id) values (@task_id, @notes, @created_date, @updated_by_id)
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand addFollowUp = new SqlCommand(query, myCon))
                {
                    addFollowUp.Parameters.AddWithValue("@task_id", followupsdata.TaskId);
                    addFollowUp.Parameters.AddWithValue("@notes", followupsdata.Notes);
                    addFollowUp.Parameters.AddWithValue("@created_date", followupsdata.CreatedDate);
                    addFollowUp.Parameters.AddWithValue("@updated_by_id", followupsdata.UpdatedById);

                    myReader = addFollowUp.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            _objResponseModel.Status = true;
            _objResponseModel.Message = "New follow up created successfully.";
            return _objResponseModel;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public FollowUpsStatusResponseModel Put(FollowUp followupdata)
        {
            FollowUpsStatusResponseModel _objResponseModel = new FollowUpsStatusResponseModel();

            string query = @"
                           update follow_ups set
                           task_id = @task_id,
                           notes = @notes,
                           created_date = @created_date,
                           updated_by_id = @updated_by_id
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
                    myCommand.Parameters.AddWithValue("@id", followupdata.Id);
                    myCommand.Parameters.AddWithValue("@task_id", followupdata.TaskId);
                    myCommand.Parameters.AddWithValue("@notes", followupdata.Notes);
                    myCommand.Parameters.AddWithValue("@created_date", followupdata.CreatedDate);
                    myCommand.Parameters.AddWithValue("@updated_by_id", followupdata.UpdatedById);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            _objResponseModel.Status = true;
            _objResponseModel.Message = "Follow up updated successfully";
            return _objResponseModel;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public FollowUpsStatusResponseModel Delete(int id)
        {
            FollowUpsStatusResponseModel _objResponseModel = new FollowUpsStatusResponseModel();

            string query = @"
                           delete from follow_ups where id=@id
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
            _objResponseModel.Message = "Follow up deleted successfully";
            return _objResponseModel;

        }
    }
}

