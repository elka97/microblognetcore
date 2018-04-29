using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PendoBlog.Models;

namespace PendoBlog.Controllers {
    [Produces("application/json")]
    [Route("api/PostVote")]
    public class PostVoteController : Controller {
        private readonly MicroBlogPendoContext _context;

        public PostVoteController(MicroBlogPendoContext context) {
            _context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Top posts by post creation date and upvotes number</returns>
        [HttpGet("TopVotes")]
        public IEnumerable<TopPost> TopVotes() {
            var items = this._context.TopPost.FromSql("SELECT PostId, Content, CreationDate, VoteUpNumber from viewTopPosts")
                .OrderByDescending(x => x.CreationDate)
                .ThenByDescending(x => x.VoteUpNumber)
                .AsNoTracking();

            return items;
        }

        /// <summary>
        /// Vote for post: up (1) or down (2)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        // GET: api/PostVote/Vote/5/3/[1|0]
        [HttpGet("{id}/{userId}/{vote}")]
        public async Task<IActionResult> Vote(int id, int userId, int vote) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var user = await _context.User.SingleOrDefaultAsync(m => m.UserId == userId);
            if (user == null) {
                ModelState.AddModelError("userId", "not found");
                return BadRequest(ModelState);
            }

            var pp = await _context.Post.SingleOrDefaultAsync(m => m.PostId == id);
            if (pp == null) {
                ModelState.AddModelError("userId", "not found");
                return BadRequest(ModelState);
            }

            if (vote != 0 && vote != 1) {
                ModelState.AddModelError("vote", "can be 0 or 1");
                return BadRequest(ModelState);
            }

            var postvote = await this._context.PostVote.SingleOrDefaultAsync(m => m.PostId == id && m.UserId == userId);

            if (postvote != null) {

                if (postvote.VoteDown && postvote.VoteUp)
                    return Json(new { errMsg = "you already voted up and down for this post" });

                if (vote == 1 && postvote.VoteUp)
                    return Json(new { errMsg = "you already voted up for this post" });

                if (vote == 0 && postvote.VoteDown)
                    return Json(new { errMsg = "you already voted down for this post" });
            }

            // update
            if (postvote != null) {
                if (vote == 0)
                    postvote.VoteDown = true;
                else if (vote == 1)
                    postvote.VoteUp = true;


                if (await TryUpdateModelAsync<PostVote>(postvote)) {
                    try {
                        await _context.SaveChangesAsync();
                        // return RedirectToAction(nameof(Index));
                    } catch (DbUpdateException ex) {
                        //Log the error (uncomment ex variable name and write a log.)
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "contact your system administrator." + ex);
                    }
                }

                return NoContent();
            }

            // insert
            var newVote = new PostVote() {
                PostId = id,
                UserId = userId
            };

            if (vote == 0)
                newVote.VoteDown = true;
            else if (vote == 1)
                newVote.VoteUp = true;

            try {
                _context.PostVote.Add(newVote);
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                throw;
            }

            return Ok();
        }


    }
}
