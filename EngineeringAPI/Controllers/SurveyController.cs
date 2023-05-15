using System.Data;
using System.Data.SqlClient;
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
    [Route("/surveys")]
    public List<Survey> GetSurveys()
    {
        var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
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
                        DescGood = Convert.ToString(questionDataTable.Rows[y]["Desc_good"]),
                        DescAvg = Convert.ToString(questionDataTable.Rows[y]["Desc_avg"]),
                        DescBad = Convert.ToString(questionDataTable.Rows[y]["Desc_bad"]),
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