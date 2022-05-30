using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using JWTProjectManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Task = JWTProjectManagement.Models.Task;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectManagement.Controllers
{
    public class TaskResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public List<dynamic> Data { set; get; }
    }

    public class TaskStatusResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }

    }

    public class AddTaskResponseModel
    {
        public string Message { set; get; }
        public int Id { get; set; }
        public bool Status { set; get; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        private IConfiguration _configuration;

        public TaskController(IConfiguration config)
        {
            _configuration = config;
        }

        // GET: api/values
        [HttpGet(Name = "GetTasks")]
        public TaskResponseModel Get()
        {
            TaskResponseModel _objResponseModel = new TaskResponseModel();

            string query = @"
                            select * from
                            task
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand getTasks = new SqlCommand(query, myCon))
                {
                    myReader = getTasks.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            List<dynamic> taskList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                Task tasks = new Task();
                tasks.Id = Convert.ToInt32(table.Rows[i]["id"]);
                tasks.ProjectId = Convert.ToInt32(table.Rows[i]["project_id"]);
                tasks.TaskTitle = table.Rows[i]["task_title"].ToString();
                tasks.TaskDescription = table.Rows[i]["task_description"].ToString();
                tasks.TaskStatus = table.Rows[i]["task_status"].ToString();
                tasks.AssignedToId = Convert.ToInt32(table.Rows[i]["assigned_to_id"]);
                tasks.AssignedById = Convert.ToInt32(table.Rows[i]["assigned_by_id"]);
                tasks.CreatedDate = table.Rows[i]["created_date"].ToString();
                tasks.Deadline = table.Rows[i]["deadline"].ToString();
                taskList.Add(tasks);
            }


            _objResponseModel.Data = taskList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Task Data Received successfully";
            return _objResponseModel;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public AddTaskResponseModel Post(Task taskdata)
        {

            AddTaskResponseModel _objResponseModel = new AddTaskResponseModel();

            string query = @"
                           insert into task
                           (project_id, task_title, task_description, task_status, assigned_to_id, assigned_by_id, deadline)
                    values (@project_id, @task_title, @task_description, @task_status, @assigned_to_id, @assigned_by_id, @deadline);
                            ";

            string queryId = @"SELECT MAX(ID) AS LastID FROM task";

            DataTable table = new DataTable();
            DataTable idTable = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;
            int taskId;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand addTask = new SqlCommand(query, myCon))
                {
                    addTask.Parameters.AddWithValue("@project_id", taskdata.ProjectId);
                    addTask.Parameters.AddWithValue("@task_title", taskdata.TaskTitle);
                    addTask.Parameters.AddWithValue("@task_description", taskdata.TaskDescription);
                    addTask.Parameters.AddWithValue("@task_status", taskdata.TaskStatus);
                    addTask.Parameters.AddWithValue("@assigned_to_id", taskdata.AssignedToId);
                    addTask.Parameters.AddWithValue("@assigned_by_id", taskdata.AssignedById);
                    addTask.Parameters.AddWithValue("@deadline", taskdata.Deadline);

                    myReader = addTask.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }

                using (SqlCommand findId = new SqlCommand(queryId, myCon))
                {
                    myReader = findId.ExecuteReader();
                    idTable.Load(myReader);
                    taskId = Convert.ToInt32(idTable.Rows[0]["LastID"]);

                    myReader.Close();
                    myCon.Close();
                }
            }

            _objResponseModel.Id = taskId;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Task Data Inserted successfully";
            return _objResponseModel;

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public TaskStatusResponseModel Put(Task tasksdata)
        {
            TaskStatusResponseModel _objResponseModel = new TaskStatusResponseModel();

            string query = @"
                           update task set
                           project_id = @project_id,
                           task_title = @task_title,
                           task_description = @task_description,
                           task_status = @task_status,
                           assigned_to_id = @assigned_to_id,
                           assigned_by_id = @assigned_by_id,
                           deadline = @deadline
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
                    myCommand.Parameters.AddWithValue("@id", tasksdata.Id);
                    myCommand.Parameters.AddWithValue("@project_id", tasksdata.ProjectId);
                    myCommand.Parameters.AddWithValue("@task_title", tasksdata.TaskTitle);
                    myCommand.Parameters.AddWithValue("@task_description", tasksdata.TaskDescription);
                    myCommand.Parameters.AddWithValue("@task_status", tasksdata.TaskStatus);
                    myCommand.Parameters.AddWithValue("@assigned_to_id", tasksdata.AssignedToId);
                    myCommand.Parameters.AddWithValue("@assigned_by_id", tasksdata.AssignedById);
                    myCommand.Parameters.AddWithValue("@deadline", tasksdata.Deadline);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            _objResponseModel.Status = true;
            _objResponseModel.Message = "Task updated successfully";
            return _objResponseModel;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public TaskStatusResponseModel Delete(int id)
        {
            TaskStatusResponseModel _objResponseModel = new TaskStatusResponseModel();

            string query = @"
                           delete from task where id=@id
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
            _objResponseModel.Message = "Task deleted successfully";
            return _objResponseModel;

        }
    }
}

