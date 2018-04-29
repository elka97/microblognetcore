using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PendoBlog.Models;

namespace PendoBlog.Controllers {

    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller {
        private readonly MicroBlogPendoContext _context;

        public UsersController(MicroBlogPendoContext context) {
            _context = context;
        }

        //http://localhost:3402/api/users/3 
        //{
        //    "userId": 1,
        //    "name": "elka",
        //    "email": "elka97@hotmail.com",
        //    "passsword": "123456",
        //    "post": []
        //}
        // GET: api/Users
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Users list</returns>
        [HttpGet("users")]
        public IEnumerable<User> GetUser() {
            return _context.User;
        }

         
        /// <summary>
        /// Find user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User data</returns>
        [HttpGet("details/{id}")]  // GET: api/UsersApi/5
        public async Task<IActionResult> GetUser([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.UserId == id);

            if (user == null) {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Create new blog user 
        /// {
        ///    "Name":		'john',
        ///    "Email":	    'doe@gmail.com',
        ///    "Passsword": '123456'
        /// }
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        // POST: api/users insert
        [HttpPost("create")]
        public async Task<IActionResult> PostUser([FromBody] UseModel user) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            User newUser = new User(){
                Name = user.Name,
                Passsword = user.Passsword,
                Email = user.Email
            };

            var existsUser = await _context.User.SingleOrDefaultAsync(m => m.Email == user.Email);

            if (existsUser != null) {
                ModelState.AddModelError("user exists", "user alrady registered in the system");
                return BadRequest(ModelState);
            }

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = newUser.UserId }, user);
        }

        //private bool UserExists(int id) {
        //    return _context.User.Any(e => e.UserId == id);
        //}

    }
}