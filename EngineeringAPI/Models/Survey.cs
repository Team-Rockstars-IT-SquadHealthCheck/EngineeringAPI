namespace EngineeringAPI.Models;

public class Survey
{
    public int Id { get; set; }
    public string? Name { get; set; } = "";
    public string? Description { get; set; } = "";
    public List<Question> Questions { get; set; } = new List<Question>();
}