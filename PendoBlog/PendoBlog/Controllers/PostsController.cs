using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PendoBlog.Models;

namespace PendoBlog.Controllers {
    using System;

    [Produces("application/json")]
    [Route("api/Posts")]
    public class PostsController : Controller {
        private readonly MicroBlogPendoContext _context;

        public PostsController(MicroBlogPendoContext context) {
            _context = context;
        }

        // GET: api/Posts
        /// <summary>
        /// Get all posts
        /// </summary>
        /// <returns>Posts list</returns>
        [HttpGet("posts")]
        public IEnumerable<Post> GetPost() {
            return _context.Post;
        }

        // GET: api/Posts/5
        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="id">Post details</param>
        /// <returns></returns>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetPost([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var post = await _context.Post.SingleOrDefaultAsync(m => m.PostId == id);

            if (post == null) {
                return NotFound();
            }

            return Ok(post);
        }

        /// <summary>
        /// Create new post
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("create/{userId}")]
        public async Task<IActionResult> PostPost([FromRoute] int userId, [FromBody] PostModel content) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (_context.Post.Any(e => e.Content == content.Text)) {
                ModelState.AddModelError("Post with same title and content already exists", "Please edit the post or create new post");
                return BadRequest(ModelState);
            }
           
            Post post = new Post(){
                Content = content.Text,
                UserId = userId,
                Title = content.Text.Substring(0, 5),
                CreationDate = DateTime.UtcNow
            };

            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.PostId }, post);
        }

        /// <summary>
        /// Edit post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        // PUT: api/Posts/5 update
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> PutPost([FromRoute] int id, [FromBody] PostModel content) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var post = await _context.Post.SingleOrDefaultAsync(m => m.PostId == id);

            if (post == null) {
                return NotFound();
            }

            post.Content = content.Text;

            _context.Entry(post).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!PostExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        private bool PostExists(int id) {
            return _context.Post.Any(e => e.PostId == id);
        }

        //// POST: api/Posts insert 
        ////{
        ////    "userId": 2,
        ////    "content": "Get the latest BBC World News: international news, features and analysis from Africa, the Asia-Pacific, Europe, Latin America, the Middle East, South Asia, and the United States and Canada.",
        ////    "title": "BBC World News",
        ////    "creationDate": "2018-04-28T20:59:59.430Z" 
        ////}
        //[HttpPost("create")]
        //public async Task<IActionResult> PostPost_([FromBody] Post post) {
        //    //     post.User = null;

        //    if (!ModelState.IsValid) {
        //        return BadRequest(ModelState);
        //    }

        //    if (post.PostId > 0) {
        //        ModelState.AddModelError("PostId is DB increment value", "Dont send PostId in json");
        //        return BadRequest(ModelState);
        //    }

        //    if (post.UserId == 0) {
        //        ModelState.AddModelError("UserId is required", "Provide UserId in json");
        //        return BadRequest(ModelState);
        //    }

        //    if (!_context.User.Any(e => e.UserId == post.UserId)) {
        //        ModelState.AddModelError("UserId not found", "Provide valid UserId in json");
        //        return BadRequest(ModelState);
        //    }

        //    if (_context.Post.Any(e => e.Title == post.Title && e.Content == post.Content)) {
        //        ModelState.AddModelError("Post with same title and content already exists", "Please edit the post or create new post");
        //        return BadRequest(ModelState);
        //    }

        //    _context.Post.Add(post);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPost", new { id = post.PostId }, post);
        //}



    }
}