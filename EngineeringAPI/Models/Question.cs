namespace EngineeringAPI.Models;

public class Question
{
    public int Id { get; set; }
    public int SurveyId { get; set; }
    public string? Title { get; set; } = "";
    public string? Description { get; set; } = "";
    public string? DescGood { get; set; } = "";
    public string? DescAvg { get; set; } = "";
    public string? DescBad { get; set; } = "";
}