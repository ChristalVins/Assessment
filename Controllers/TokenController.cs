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
    public class TokenController : Controller
    {
        private ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        public ActionResult AuthenticateDetails()
        {
            var response = _tokenService.Authenticate();

            if (response == null)
                return BadRequest(new { message = "Error" });

            return Ok(response);
        }
    }
}
