using H5Security.Models;
using H5Security.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace H5Security.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private IEncryption _encryption;
        public MessageController(IEncryption encryption)
        {
            _encryption = encryption;
        }

        [HttpPost("Encrypt")]
        public ActionResult<string> Encrypt([FromBody] MessageDTO message)
        {
            return _encryption.Encrypt(message.Text);
        }

        [HttpPost("Decrypt")]
        public ActionResult<string> Decrypt([FromBody] MessageDTO message)
        {
            return _encryption.Decrypt(message.Text);
        }
    }
}
