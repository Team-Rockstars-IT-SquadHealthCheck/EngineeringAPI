namespace EngineeringAPI.Models;

public class Question
{
    public int Id { get; set; }
    public int SurveyId { get; set; }
    public string? Title { get; set; } = "";
    public string? Description { get; set; } = "";
}