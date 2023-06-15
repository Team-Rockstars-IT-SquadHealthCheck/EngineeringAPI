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

    [HttpGet]
    [Route("/Answer/{url}")]
    public List<Answer> GetAnswers(string url)
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

        List<Answer> answers = new List<Answer>();
        var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        conn.Open();
        var cmd = new SqlCommand("Select answer.questionid, userid, answer, comment From answer inner join question on answer.questionid = question.id where question.surveyid = @surveyid and userid = @userid", conn);
        cmd.Parameters.AddWithValue("@surveyid", urlLink.SurveyNumber);
        cmd.Parameters.AddWithValue("@userid", urlLink.UserID);

        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Answer answer = new Answer();
            answer.QuestionId = (int)reader["questionid"];
            answer.UserId = (int)reader["userid"];
            answer.AnswerInt = (int)reader["answer"];
            answer.Comment = (string)reader["comment"];
            answers.Add(answer);
        }
        reader.Close();
        var result = cmd.ExecuteScalar();
        if (result == null)
        {
            return answers;
        }
        return answers;
    }

    [HttpPost]
    [Route("/Answer")]
    public async void PostSurvey([FromBody] List<Answer> answers)
    {
        await using var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        await conn.OpenAsync();
        foreach (var answer in answers)
        {
            var cmd = new SqlCommand("INSERT INTO answer " +
                                     "(answer, comment, userid, questionid) VALUES " +
                                     "(@answer, @comment, @userid, @questionid);", conn);
            
            cmd.Parameters.AddWithValue("@answer", answer.AnswerInt);
            cmd.Parameters.AddWithValue("@comment", answer.Comment);
            cmd.Parameters.AddWithValue("@userid", answer.UserId);
            cmd.Parameters.AddWithValue("@questionid", answer.QuestionId);
            try
            {
                var result = await cmd.ExecuteNonQueryAsync();
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    [HttpGet]
    [Route("/StatusFilled/{url}")]
    public string PostStatus(string url)
    {
        byte[] bytes = Convert.FromBase64String(url);
        url = System.Text.Encoding.UTF8.GetString(bytes);
        var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        conn.Open();
        var cmd = new SqlCommand("Update Url Set status = 1 where url = @url", conn);
        cmd.Parameters.AddWithValue("@url", url);

        try
        {
            var result = cmd.ExecuteNonQuery();
            return "1";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "0";
        }
    }
}