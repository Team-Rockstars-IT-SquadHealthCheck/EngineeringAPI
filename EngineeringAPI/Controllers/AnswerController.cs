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
    public async void PostSurvey([FromBody] List<AnswerModel> answers)
    {
        foreach (var answer in answers)
        {
            await using var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
            await using var cmd = new SqlCommand("INSERT INTO answer " +
                                                 "(answer, comment, userid, questionid) VALUES " +
                                                 "($1, $2, $3, $4);", conn)
            {
                Parameters =
                {
                    new SqlParameter { Value = answer.Answer },
                    new SqlParameter { Value = answer.Comment },
                    new SqlParameter { Value = answer.UserId },
                    new SqlParameter { Value = answer.QuestionId }
                }
            };
            var result = await cmd.ExecuteNonQueryAsync();
            Console.WriteLine(result);
        };
        //TODO var result is not within using() brackets of cmd
    }
}