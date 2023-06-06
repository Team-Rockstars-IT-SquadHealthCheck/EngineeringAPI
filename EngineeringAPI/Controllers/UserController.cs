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
    [Route("/Validate/{url}")]
    public Url? Validation(string url)
    { 
        Url urlModel = new Url();
        var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        conn.Open();
        var cmd = new SqlCommand("SELECT * FROM url WHERE url = @url", conn);
        cmd.Parameters.AddWithValue("@url", url);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            urlModel.Id = (int)reader["id"];
            urlModel.url = (string)reader["url"];
            urlModel.userid = (int)reader["userid"];
            urlModel.status = (int)reader["status"];
        }
        reader.Close();

        var result = cmd.ExecuteScalar();
        if (result == null)
        {
            return urlModel;
        }
        return urlModel;
    }

    [HttpGet]
    [Route("/users/{id:int}")]
    public User? User(int? id)
    {
        if (id == null || id <= 0) { return null; }
        
        var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        conn.Open();
        var user = new User();
        var dataAdapter = new SqlDataAdapter("SELECT * FROM \"user\" WHERE id = " + id, conn);
        var dataTable = new DataTable();

        dataAdapter.Fill(dataTable);
        if (dataTable.Rows.Count <= 0) return user;
        user.Id = Convert.ToInt32(dataTable.Rows[0]["id"]);
        user.Username = Convert.ToString(dataTable.Rows[0]["username"]);
        user.Password = Convert.ToString(dataTable.Rows[0]["password"]);
        user.Email = Convert.ToString(dataTable.Rows[0]["email"]);
        user.RoleId = Convert.ToInt32(dataTable.Rows[0]["roleid"]);
        try
        {
            user.SquadId = Convert.ToInt32(dataTable.Rows[0]["squadid"]);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            user.SquadId = null;
        }

        var roleData = GetRoleData(user.RoleId);
        user.RoleName = roleData.Name;
        user.RoleDescription = roleData.Description;

        var squadData = GetSquadData(user.SquadId);
        user.CompanyId = squadData.CompanyId;
        user.SquadSurveyId = squadData.SurveyId;

        var companyData = GetCompanyData(user.CompanyId);
        user.CompanyName = companyData.Name;
        user.CompanyAddress = companyData.Address;
        user.CompanyPhone = companyData.PhoneNumber;
        
        return user;
    }

    [HttpGet]
    private Role GetRoleData(int roleId)
    {
        var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        conn.Open();
        var role = new Role();
        var dataAdapter = new SqlDataAdapter("SELECT * FROM role WHERE id = " + roleId, conn);
        var dataTable = new DataTable();
        
        dataAdapter.Fill(dataTable);
        if (dataTable.Rows.Count <= 0) return role;
        role.Id = Convert.ToInt32(dataTable.Rows[0]["id"]);
        role.Name = Convert.ToString(dataTable.Rows[0]["name"]);
        role.Description = Convert.ToString(dataTable.Rows[0]["description"]);

        return role;
    }
    
    [HttpGet]
    private Squad GetSquadData(int? squadId)
    {
        var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        conn.Open();
        var squad = new Squad();
        var dataAdapter = new SqlDataAdapter("SELECT * FROM squad WHERE id = " + squadId, conn);
        var dataTable = new DataTable();
        
        dataAdapter.Fill(dataTable);
        if (dataTable.Rows.Count <= 0) return squad;
        squad.Id = Convert.ToInt32(dataTable.Rows[0]["id"]);
        squad.SurveyId = Convert.ToInt32(dataTable.Rows[0]["surveyid"]);
        squad.CompanyId = Convert.ToInt32(dataTable.Rows[0]["companyid"]);

        return squad;
    }
    
    [HttpGet]
    private Company GetCompanyData(int? companyId)
    {
        var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        conn.Open();        
        var company = new Company();
        var dataAdapter = new SqlDataAdapter("SELECT * FROM company WHERE id = " + companyId, conn);
        var dataTable = new DataTable();
        
        dataAdapter.Fill(dataTable);
        if (dataTable.Rows.Count <= 0) return company;
        company.Id = Convert.ToInt32(dataTable.Rows[0]["id"]);
        company.Name = Convert.ToString(dataTable.Rows[0]["name"]);
        company.Address = Convert.ToString(dataTable.Rows[0]["address"]);
        company.PhoneNumber = Convert.ToString(dataTable.Rows[0]["telephonenr"]);

        return company;
    }
    
    [HttpGet] 
    [Route("/users")] 
    public List<User> Users()
    {
        var conn = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        conn.Open();
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