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
    public class ClientResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public List<dynamic> Data { set; get; }
    }

    public class ClientStatusResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }

    }

    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private IConfiguration _configuration;

        public ClientController(IConfiguration config)
        {
            _configuration = config;
        }

        // GET: api/values
        [HttpGet]
        public ClientResponseModel Get()
        {
            ClientResponseModel _objResponseModel = new ClientResponseModel();

            string query = @"
                            select * from
                            clients
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

            List<dynamic> clientList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                Client client = new Client();
                client.Id = Convert.ToInt32(table.Rows[i]["id"]);
                client.ClientName = table.Rows[i]["client_name"].ToString();
                client.Address = table.Rows[i]["address"].ToString();
                client.Phonenumber = table.Rows[i]["phonenumber"].ToString();
                clientList.Add(client);
            }


            _objResponseModel.Data = clientList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Clients received successfully";
            return _objResponseModel;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ClientResponseModel Get(int id)
        {
            ClientResponseModel _objResponseModel = new ClientResponseModel();

            string query = @"
                            select * from
                            clients where id=@id
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

            List<dynamic> clientList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                Client client = new Client();
                client.Id = Convert.ToInt32(table.Rows[i]["id"]);
                client.ClientName = table.Rows[i]["client_name"].ToString();
                client.Address = table.Rows[i]["address"].ToString();
                client.Phonenumber = table.Rows[i]["phonenumber"].ToString();
                clientList.Add(client);
            }


            _objResponseModel.Data = clientList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Client received successfully";
            return _objResponseModel;

        }

        // POST api/values
        [HttpPost]
        public ClientStatusResponseModel Post(Client clientdata)
        {
            ClientStatusResponseModel _objResponseModel = new ClientStatusResponseModel();

            string query = @"
                            insert into clients
                            (client_name, address, phonenumber) values (@client_name, @address, @phonenumber)
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("PMDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand addClient = new SqlCommand(query, myCon))
                {
                    addClient.Parameters.AddWithValue("@client_name", clientdata.ClientName);
                    addClient.Parameters.AddWithValue("@address", clientdata.Address);
                    addClient.Parameters.AddWithValue("@phonenumber", clientdata.Phonenumber);

                    myReader = addClient.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            _objResponseModel.Status = true;
            _objResponseModel.Message = "New client created successfully.";
            return _objResponseModel;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ClientStatusResponseModel Put(Client clientdata)
        {
            ClientStatusResponseModel _objResponseModel = new ClientStatusResponseModel();

            string query = @"
                           update clients set
                           client_name = @client_name,
                           address = @address,
                           phonenumber = @phonenumber
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
                    myCommand.Parameters.AddWithValue("@id", clientdata.Id);
                    myCommand.Parameters.AddWithValue("@client_name", clientdata.ClientName);
                    myCommand.Parameters.AddWithValue("@address", clientdata.Address);
                    myCommand.Parameters.AddWithValue("@phonenumber", clientdata.Phonenumber);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            _objResponseModel.Status = true;
            _objResponseModel.Message = "Client updated successfully";
            return _objResponseModel;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ClientStatusResponseModel Delete(int id)
        {
            ClientStatusResponseModel _objResponseModel = new ClientStatusResponseModel();

            string query = @"
                           delete from clients where id=@id
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
            _objResponseModel.Message = "Client deleted successfully";
            return _objResponseModel;

        }
    }
}