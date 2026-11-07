namespace WebApiExampleP42.Models;

public class Message
{
    public int Id { get; set; }
    public string To { get; set; }
    public string From { get; set; }
    public string Text { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
}
