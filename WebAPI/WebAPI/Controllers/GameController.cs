using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
   private readonly ILogger<GameController> _logger;

    public GamesKeeper Hagrid { get; }

    public GameController(ILogger<GameController> logger, GamesKeeper hagrid)
    {
        _logger = logger;
        Hagrid = hagrid;
    }

    [HttpGet(Name = "Games")]
    public IEnumerable<Game> Get()
    {
        return Hagrid.Games;
    }

    


    // POST: GameController/Create
    [HttpPost]
    public IEnumerable<Game> Post([FromBody]Game Game)
    {
        try
        {
            switch (Game.Command)
            {
                case Game.ServerCommand.Start:
                    StartServer(Game);
                    break;

                case Game.ServerCommand.Stop:
                    StopServer(Game);
                    break;

                case Game.ServerCommand.Restart:
                    RestartServer(Game);
                    break;

                case Game.ServerCommand.Create:
                    Hagrid.Games.Add(new Game { SteamID = Game.SteamID, ServerID = Hagrid.Games.Select(game => game.ServerID).Max() + 1 });
                    break;

                case Game.ServerCommand.Delete:
                    
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

        }
        return Hagrid.Games;
    }

    private void RestartServer(Game Game)
    {
        var game = Hagrid.Games.Where(game => game.SteamID == Game.SteamID && game.ServerID == Game.ServerID).SingleOrDefault();

        if(game.Status == Game.ServerStatus.Stopped)
        {
            StartServer(Game);
            return;
        }


        game.Status = Game.ServerStatus.Stopping;
        Task.Delay(1000)
            .ContinueWith(g => game.Status = Game.ServerStatus.Stopped)
            .ContinueWith(async g => await Task.Delay(1000))
            .ContinueWith(g => game.Status = Game.ServerStatus.Starting)
            .ContinueWith(async g => await Task.Delay(1000))
            .ContinueWith(g => game.Status = Game.ServerStatus.Running);
    }

    private void StopServer(Game Game)
    {
        var game = Hagrid.Games.Where(game => game.SteamID == Game.SteamID && game.ServerID == Game.ServerID).SingleOrDefault();
        game.Status = Game.ServerStatus.Stopping;
        Task.Delay(1000).ContinueWith(g => game.Status = Game.ServerStatus.Stopped);
    }

    private void StartServer(Game Game)
    {
        var game = Hagrid.Games.Where(game => game.SteamID == Game.SteamID && game.ServerID == Game.ServerID).SingleOrDefault();
        game.Status = Game.ServerStatus.Starting;
        Task.Delay(1000).ContinueWith(g => game.Status = Game.ServerStatus.Running);
        
    }

}
