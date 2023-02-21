namespace PR.CommandCenter.Modules.Events.Application;

public class Event
{
  public Guid Id { get; set; }
  public Guid BotId { get; set; }
  public string Type { get; set; } = "";
  public string Content { get; set; } = "";
  public DateTime DateCreated { get; set; }
}