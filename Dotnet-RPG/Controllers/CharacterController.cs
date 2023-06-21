using Microsoft.AspNetCore.Mvc;

namespace Dotnet_RPG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private static Character knight = new Character();

        [HttpGet]
        public ActionResult<Character> Get()
        {
            if (knight == null)
            {
                return NotFound();
            }
            return Ok(knight);
        }
    }
}
