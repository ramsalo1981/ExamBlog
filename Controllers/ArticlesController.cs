using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KnowledgeAppApi.Models;

namespace KnowledgeAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly KnowledgeDbContext _context;

        public ArticlesController(KnowledgeDbContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            return await _context.Articles.Include(c => c.Category).Include(p => p.ArticleParagraphs).ToListAsync();
        }
        /// <summary>
        /// get last 3 Articles from database include articles
        /// </summary>
        /// <returns>list of 3 last Articles</returns>

        [HttpGet("GetLastArticles")]
        public async Task<ActionResult<IEnumerable<Article>>> GetLastArticles()
        {
            return await _context.Articles.OrderByDescending(a => a.ArticleId).Take(3).Include(c => c.Category).Include(p => p.ArticleParagraphs).ToListAsync();
           
        }

            // GET: api/Articles/5
            [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.Articles.Include(c => c.Category).Where(a => a.ArticleId == id).Include(p=> p.ArticleParagraphs).FirstOrDefaultAsync();

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        [Route("SearchCategoryArticle/{search}")]
        [HttpGet]
        public async Task<IEnumerable<Article>> SearchCategoryArticle(string search)
        {
            return await _context.Articles.OrderByDescending(x => x.ArticleId)
                .Include(x => x.Category)
                
                .Where(x => x.ArticleTitle.ToLower().Contains(search.ToLower())
                || x.Category.CategoryTitle.ToLower().Contains(search.ToLower())).Include(p => p.ArticleParagraphs)
                .ToListAsync();
        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, [FromForm] Article article)
        {
            if (id != article.ArticleId)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle([FromForm] Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.ArticleId }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}
