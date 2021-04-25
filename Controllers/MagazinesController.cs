using Assessment.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MagazinesController : Controller
    {
        private ITokenService _tokenService;

        public MagazinesController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        //[Authorize]
        [HttpGet]
        [Route("{token}/{category}")]
        public ActionResult GetCategories(string token, string category)
        {
            var response = _tokenService.GetMagazineService(token, category);

            if (response == null)
                return BadRequest(new { message = "Error" });

            return Ok(response);
        }
    }
}
