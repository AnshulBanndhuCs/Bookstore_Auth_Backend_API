using BookStore_Auth_Backend_API_DataAccess.Identity;
using BookStore_Auth_Backend_API_DataAccess.ServiceContract;
using BookStore_Auth_Backend_API_Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Auth_Backend_API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUser _userRep;
        //public static IWebHostEnvironment _webHostEnvironment;
        public UserController(ApplicationDbContext context, IUser userRep)
        {
            _context = context;
            _userRep = userRep;
            //_webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _context.users.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]User user)
        {
            if (user!=null)
            {
                var newUser = await _userRep.AddUserAsync(user);
                if(newUser!=null) return Ok(newUser);
            }
            return BadRequest(new { message = "User is Not valid !!" });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if(await _userRep.UserExistsAsync(id))
            {
                var userInDb = await _userRep.UpdateUserAsync(id, user);
                if (userInDb != null) return Ok(userInDb);
            }           
            return NotFound();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (await _userRep.UserExistsAsync(id))
            {
                var deleteUser =await _userRep.DeleteUserAsync(id);
                if (deleteUser != false)
                    return Ok();
            }
            return NotFound();
        }
    }
}
