using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Blog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comments.Include(c => c.Author).Include(c => c.Post);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            if (comment.AuthorId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized();
            }

            return RedirectToAction("Details", "Posts", new { id = comment.PostId });
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Body,UpdatedAt,PostId")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            // Retrieve the original comment from the database
            var originalComment = await _context.Comments.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure that the PostId is preserved from the original comment
                    comment.PostId = originalComment.PostId;
                    comment.UpdatedAt = DateTime.Now;

                    // Update the comment
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
            }

            return RedirectToAction("Details", "Posts", new { id = comment.PostId });
        }


        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Console.WriteLine("id:" + id);
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            if (comment.AuthorId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized();
            }

            return RedirectToAction("Details", "Posts", new { id = comment.PostId });
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine("sec: " + id);
            if (CommentExists(id))
            {
                var comment = await _context.Comments.FindAsync(id);
                Console.WriteLine("comment: " + comment);
                if (comment.Author == null || comment.Post == null)
                {
                    return NotFound();
                }
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
            }
            return NotFound("Comment not found");
            
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
