namespace EngineeringAPI.Models;

public class AnswerModel
{
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public int Answer { get; set; }
    public string Comment { get; set; } = "";
}