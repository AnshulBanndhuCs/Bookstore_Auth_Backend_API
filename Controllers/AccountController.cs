using BookStore_Auth_Backend_API_DataAccess.ServiceContract;
using BookStore_Auth_Backend_API_Models.ViewModels;
using BookStore_Auth_Backend_API_Utility.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore_Auth_Backend_API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]LoginVM loginVM)
        {
            var user = await _userService.Authenticate(loginVM);
            if (user == null) 
                return BadRequest(new { message = "Wrong UserName / Password !!!" });
            return Ok(user);
        }
    }
}
