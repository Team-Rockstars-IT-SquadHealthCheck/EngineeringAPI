namespace EngineeringAPI.Models;

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; } = "";
    public string? Password { get; set; } = "";
    public string? Email { get; set; } = "";
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? RoleDescription { get; set; }
    public int? SquadId { get; set; }
    public int SquadSurveyId { get; set; }
    public int? CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public string? CompanyPhone { get; set; }
}