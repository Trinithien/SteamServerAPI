using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommandsController : ControllerBase
{
    [HttpGet(Name = "Commands")]
    public object GetCommands()
    {
        var commands = "[\r\n  {";
        int i = 0;
        foreach (var command in typeof(Game.ServerCommand).GetEnumNames())
        {
            commands += $"\r\n    \"{command}\": {i},";
            i++;
        }
        
        return commands.TrimEnd(',') + "\r\n  }\r\n]";
    }
}
