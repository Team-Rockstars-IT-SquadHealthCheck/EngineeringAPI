using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Policy;
using EngineeringAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace EngineeringAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SurveyController
{
    private readonly IConfiguration _configuration;

    public SurveyController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    [Route("/survey/{url}")]
    public Survey GetSurveyByUrl(string url)
    {
        byte[] bytes = Convert.FromBase64String(url);
        url = System.Text.Encoding.UTF8.GetString(bytes);
        char[] delimiters = { '&' };
        string[] substrings = url.Split(delimiters);
        UrlLink urlLink = new UrlLink();
        urlLink.SurveyNumber = Int32.Parse(substrings[0]);
        urlLink.UUID = substrings[1];
        urlLink.SquadID = Int32.Parse(substrings[2]);
        urlLink.UserID = Int32.Parse(substrings[3]);

        List<Question> questions = new();
        Survey survey = new();
        var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        conn.Open();

        var command = new SqlCommand("SELECT * FROM survey WHERE id = @id", conn);
        command.Parameters.AddWithValue("@id", urlLink.SurveyNumber);

        SqlDataReader read = command.ExecuteReader();
        if (read.Read())
        {
            survey.Id = (int)read["id"];
            survey.Name = (string)read["name"];
            survey.Description = (string)read["description"];
        }
        read.Close();
        var cmd = new SqlCommand("SELECT * FROM question WHERE surveyid = @surveyid", conn);
        cmd.Parameters.AddWithValue("@surveyid", urlLink.SurveyNumber);

        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var question = new Question
            {
                Id = (int)reader["id"],
                SurveyId = (int)reader["surveyid"],
                Description = (string)reader["description"],
                Title = (string)reader["question"],
                DescGood = (string)reader["Desc_good"],
                DescAvg = (string)reader["Desc_avg"],
                DescBad = (string)reader["Desc_bad"]
            };

            questions.Add(question);
        }
        reader.Close();
        survey.Questions = questions;

        var result = cmd.ExecuteScalar();
        if (result == null)
        {
            return survey;
        }
        return survey;
    }

    [HttpGet]
    [Route("/surveys")]
    public List<Survey> GetSurveys()
    {
        var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        conn.Open();
        List<Survey> surveys = new();
        var surveyDataAdapter = new SqlDataAdapter("SELECT * FROM survey", conn);
        var surveyDataTable = new DataTable();
        var questionController = new QuestionController(_configuration);

        surveyDataAdapter.Fill(surveyDataTable);
        if (surveyDataTable.Rows.Count <= 0) return surveys;
        for (var i = 0; i < surveyDataTable.Rows.Count; i++)
        {
            List<Question> questions = new();
            var surveyId = Convert.ToInt32(surveyDataTable.Rows[i]["id"]);
            var questionDataAdapter = new SqlDataAdapter("SELECT * FROM question WHERE surveyid = " + surveyId, conn);
            var questionDataTable = new DataTable();

            questionDataAdapter.Fill(questionDataTable);
            if (questionDataTable.Rows.Count > 0)
            {
                for (var y = 0; y < questionDataTable.Rows.Count; y++)
                {
                    var question = new Question
                    {
                        Id = Convert.ToInt32(questionDataTable.Rows[y]["id"]),
                        SurveyId = Convert.ToInt32(questionDataTable.Rows[y]["surveyid"]),
                        Description = Convert.ToString(questionDataTable.Rows[y]["description"]),
                        Title = Convert.ToString(questionDataTable.Rows[y]["question"]),
                        DescGood = Convert.ToString(questionDataTable.Rows[y]["desc_good"]),
                        DescAvg = Convert.ToString(questionDataTable.Rows[y]["desc_avg"]),
                        DescBad = Convert.ToString(questionDataTable.Rows[y]["desc_bad"]),
                    };
                    questions.Add(question);
                }
            }
            
            var survey = new Survey
            {
                Id = Convert.ToInt32(surveyDataTable.Rows[i]["id"]),
                Name = Convert.ToString(surveyDataTable.Rows[i]["name"]),
                Description = Convert.ToString(surveyDataTable.Rows[i]["Description"]),
                Questions = questions
            };
            surveys.Add(survey);
        }
        

        return surveys;
    }

}