namespace EngineeringAPI.Models;

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; } = "";
    public string? Password { get; set; } = "";
    public string? Email { get; set; } = "";
    public int RoleId { get; set; }
    public int? SquadId { get; set; }
}