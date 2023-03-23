using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using EngineeringAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public List<Question> GetQuestionsBySurvey(Int32 surveyId)
        {
            var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
            List<Question> questions = new();
            var dataAdapter = new SqlDataAdapter("SELECT * FROM question WHERE surveryid = " + surveyId, conn);
            var dataTable = new DataTable();

            dataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count <= 0) return questions;
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                var question = new Question
                {
                    Id = Convert.ToInt32(dataTable.Rows[i]["id"]),
                    SurveyId = Convert.ToInt32(dataTable.Rows[i]["surveryid"]),
                    Description = Convert.ToString(dataTable.Rows[i]["Description"]),
                    Title = Convert.ToString(dataTable.Rows[i]["question"])
                };
                questions.Add(question);
            }

            return questions;
        }

        [HttpGet]
        [Route("/surveys")]
        public List<Survey> Surveys()
        {
            var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
            List<Survey> surveys = new();
            var dataAdapter = new SqlDataAdapter("SELECT * FROM survey", conn);
            var dataTable = new DataTable();
            
            dataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count <= 0) return surveys;
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                var survey = new Survey
                {
                    Id = Convert.ToInt32(dataTable.Rows[i]["id"]),
                    Name = Convert.ToString(dataTable.Rows[i]["name"]),
                    Description = Convert.ToString(dataTable.Rows[i]["Description"]),
                    Questions = GetQuestionsBySurvey(Convert.ToInt32(dataTable.Rows[i]["id"]))
                };
                surveys.Add(survey);
            }

            return surveys;
        }
        
        [HttpGet] 
        [Route("/HelloWorld")] 
        public string HelloWorld()
        {
            return "Hello World!";
        }
        
        [HttpGet] 
        [Route("/users")] 
        public List<User> Users()
        {
            var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
            var users = new List<User>();
            var dataAdapter = new SqlDataAdapter("SELECT * FROM \"user\"", conn);
            var dataTable = new DataTable();
            
            dataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count <= 0) return users;
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                var user = new User
                {
                    Id = Convert.ToInt32(dataTable.Rows[i]["id"]),
                    Username = Convert.ToString(dataTable.Rows[i]["username"]),
                    Password = Convert.ToString(dataTable.Rows[i]["password"]),
                    Email = Convert.ToString(dataTable.Rows[i]["email"]),
                    RoleId = Convert.ToInt32(dataTable.Rows[i]["roleid"])
                };
                try
                {
                    user.SquadId = Convert.ToInt32(dataTable.Rows[i]["squadid"]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    int? x = null;
                    user.SquadId = x;
                }
                users.Add(user);
            }

            return users;
        }

        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
