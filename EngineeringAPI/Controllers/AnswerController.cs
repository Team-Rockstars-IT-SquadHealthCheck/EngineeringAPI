using System.Data;
using System.Data.SqlClient;
using EngineeringAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnswerController
{
    private readonly IConfiguration _configuration;

    public AnswerController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpPost]
    [Route("/Answer")]
    public void PostSurvey([FromBody] List<Answer> answers)
    {
        using var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        conn.Open();
        foreach (var answer in answers)
        {
            var cmd = new SqlCommand("INSERT INTO \"answer\" " +
                                     "(\"answer\", comment, userid, questionid) VALUES " +
                                     "(@answer, @comment, @userid, @questionid);", conn);
            cmd.Parameters.AddWithValue("@answer", answer.AnswerInt);
            cmd.Parameters.AddWithValue("@comment", answer.Comment);
            cmd.Parameters.AddWithValue("@userid", answer.UserId);
            cmd.Parameters.AddWithValue("@questionid", answer.QuestionId);
            var result = cmd.ExecuteNonQuery();
            Console.WriteLine(result);
        }
    }

    [HttpGet]
    [Route("/Answers")]
    public List<Answer> GetAnswers()
    {
        using var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        List<Answer> answers = new();
        var answerDataAdapter = new SqlDataAdapter("SELECT * FROM \"answer\"", conn);
        var answerDataTable = new DataTable();

        answerDataAdapter.Fill(answerDataTable);
        if (answerDataTable.Rows.Count <= 0) return answers;
        for (var i = 0; i < answerDataTable.Rows.Count; i++)
        {
            var answer = new Answer
            {
                AnswerInt = Convert.ToInt32(answerDataTable.Rows[i]["answer"]),
                Comment = Convert.ToString(answerDataTable.Rows[i]["comment"]) ?? string.Empty,
                UserId = Convert.ToInt32(answerDataTable.Rows[i]["userid"]),
                QuestionId = Convert.ToInt32(answerDataTable.Rows[i]["questionid"])
            };
            answers.Add(answer);
        }

        return answers;
    }
}