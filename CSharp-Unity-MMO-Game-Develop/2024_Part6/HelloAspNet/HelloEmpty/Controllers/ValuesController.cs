using HelloEmpty.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelloEmpty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public List<HelloMessage> Get()
        {
            List<HelloMessage> message = new List<HelloMessage>();
            message.Add(new HelloMessage { Message = "Hello Web API 1" });
            message.Add(new HelloMessage { Message = "Hello Web API 2" });
            message.Add(new HelloMessage { Message = "Hello Web API 3" });
            return message;
        }
    }
}
