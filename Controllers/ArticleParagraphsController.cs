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
    public class ArticleParagraphsController : ControllerBase
    {
        private readonly KnowledgeDbContext _context;

        public ArticleParagraphsController(KnowledgeDbContext context)
        {
            _context = context;
        }

        // GET: api/ArticleParagraphs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleParagraph>>> GetArticleParagraphs()
        {
            return await _context.ArticleParagraphs.Include(a => a.Article).ThenInclude(c => c.Category).ToListAsync();
        }

        // GET: api/ArticleParagraphs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleParagraph>> GetArticleParagraph(int id)
        {
            var articleParagraph = await _context.ArticleParagraphs.FindAsync(id);

            if (articleParagraph == null)
            {
                return NotFound();
            }

            return articleParagraph;
        }

        // PUT: api/ArticleParagraphs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticleParagraph(int id, [FromForm] ArticleParagraph articleParagraph)
        {
            if (id != articleParagraph.ArticleParagraphId)
            {
                return BadRequest();
            }

            _context.Entry(articleParagraph).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleParagraphExists(id))
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

        // POST: api/ArticleParagraphs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArticleParagraph>> PostArticleParagraph([FromForm] ArticleParagraph articleParagraph)
        {
            _context.ArticleParagraphs.Add(articleParagraph);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticleParagraph", new { id = articleParagraph.ArticleParagraphId }, articleParagraph);
        }

        // DELETE: api/ArticleParagraphs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticleParagraph(int id)
        {
            var articleParagraph = await _context.ArticleParagraphs.FindAsync(id);
            if (articleParagraph == null)
            {
                return NotFound();
            }

            _context.ArticleParagraphs.Remove(articleParagraph);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleParagraphExists(int id)
        {
            return _context.ArticleParagraphs.Any(e => e.ArticleParagraphId == id);
        }
    }
}
