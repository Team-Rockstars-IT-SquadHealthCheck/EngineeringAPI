using System.Data;
using System.Data.SqlClient;
using EngineeringAPI.Models;

namespace EngineeringAPI.Controllers;

public class QuestionController
{
    private readonly IConfiguration _configuration;

    public QuestionController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}