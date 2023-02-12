using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    [HttpGet(Name = "Statuses")]
    public string GetStatuses()
    {
        var statuses = "[\r\n  {";
        int i = 0;
        foreach (var status in typeof(Game.ServerStatus).GetEnumNames())
        {
            statuses += $"\r\n    \"{status}\": {i},";
            i++;
        }

        return statuses.TrimEnd(',') + "\r\n  }\r\n]";
    }
}
