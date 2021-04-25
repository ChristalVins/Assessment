using Assessment.Interface;
using Microsoft.AspNetCore.Authorization;
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
    public class SubscribersController : Controller
    {
        private ITokenService _tokenService;

        public SubscribersController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        //[Authorize]
        [HttpGet]
        [Route("{token}")]
        public ActionResult AuthenticateDetails(string token)
        {
            var response = _tokenService.SubscriberService(token);

            if (response == null)
                return BadRequest(new { message = "Error" });

            return Ok(response);
        }
    }
}
