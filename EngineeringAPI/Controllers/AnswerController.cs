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
}