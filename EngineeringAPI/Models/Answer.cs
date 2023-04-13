namespace EngineeringAPI.Models;

public class Answer
{
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public int AnswerInt { get; set; }
    public string Comment { get; set; } = "";
}