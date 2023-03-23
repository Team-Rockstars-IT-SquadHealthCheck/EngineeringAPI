using System.Data;
using System.Data.SqlClient;
using System.Net;
using EngineeringAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController
{
    private readonly IConfiguration _configuration;

    public UserController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    [Route("/users/{id:int}")]
    public User? User(int? id)
    {
        if (id == null || id <= 0) { return null; }
        
        var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        var user = new User();
        var dataAdapter = new SqlDataAdapter("SELECT * FROM \"user\" WHERE id = " + id, conn);
        var dataTable = new DataTable();

        dataAdapter.Fill(dataTable);
        if (dataTable.Rows.Count <= 0) return user;
        user.Id = Convert.ToInt32(dataTable.Rows[0]["id"]);
        user.Username = Convert.ToString(dataTable.Rows[0]["username"]);
        user.Password = Convert.ToString(dataTable.Rows[0]["password"]);
        user.Email = Convert.ToString(dataTable.Rows[0]["email"]);
        user.RoleId = Convert.ToInt32(dataTable.Rows[0]["id"]);
        try
        {
            user.SquadId = Convert.ToInt32(dataTable.Rows[0]["id"]);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            user.SquadId = null;
        }

        return user;
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
}