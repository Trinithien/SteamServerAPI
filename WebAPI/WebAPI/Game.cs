namespace WebAPI;

public class Game
{
    public enum ServerCommand
    {
        Start,
        Stop,
        Restart,
        Create,
        Delete

    }
    public enum ServerStatus
    {
        Stopped,
        Starting,
        Running,
        Stopping,
        Crashed
    }
    public ServerCommand Command { get; set; }
    public ServerStatus Status { get; set; } = ServerStatus.Stopped;
    public int? SteamID { get; set; }
    public int? ServerID { get; set; }

}
