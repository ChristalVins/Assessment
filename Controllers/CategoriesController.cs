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
    public class CategoriesController : Controller
    {
        private ITokenService _tokenService;

        public CategoriesController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        //[Authorize]
        [HttpGet]
        [Route("{token}")]
        public ActionResult GetCategories(string token)
        {
            var response = _tokenService.CategoriesService(token);

            if (response == null)
                return BadRequest(new { message = "Error" });

            return Ok(response);
        }
    }
}
