using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace App.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        static List<Article> _articles = new List<Article>();


        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Article>))]
        public IActionResult List()
        {
            if (_articles.Count == 0)
            {
                _articles.Add(new Article { Id = 1, Title = "Birinci", Content = "Birinci Makale" });
                _articles.Add(new Article { Id = 2, Title = "İkinci", Content = "İkinci Makale" });
                _articles.Add(new Article { Id = 3, Title = "Üçüncü", Content = "Üçücü Makale" });
            }
            return Ok(_articles);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Article>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult Get(int id)
        {
            var article = _articles.FirstOrDefault(a => a.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(List<Article>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult Post([FromBody] Article article)
        {
            if (string.IsNullOrWhiteSpace(article.Title))
            {
                return BadRequest("Title null veya boş olamaz.");
            }

            if (_articles.Any(x => x.Id == article.Id))
            {
                return BadRequest("Bu Id ile zaten bir makale mevcut");
            }
            _articles.Add(article);

            return CreatedAtAction(nameof(Get), new { id = article.Id }, article);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Article>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult Put(int id, [FromBody] Article article)
        {
            if (string.IsNullOrWhiteSpace(article.Title))
            {
                return BadRequest("Title null veya boş olamaz.");
            }
            var aArticle = _articles.FirstOrDefault(x => x.Id == id);

            if (aArticle == null)
            {
                return NotFound("Makale bulunamadı.");
            }
            aArticle.Title = article.Title;
            aArticle.Content = article.Content;
            return Ok(aArticle);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Article>))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(string))]
        public IActionResult Delete(int id)
        {
            var article = _articles.FirstOrDefault(x => x.Id == id);

            if (article == null)
            {
                return NoContent();
            }
            _articles.Remove(article);

            return Ok("Makale başarıyla silindi.");
        }
    }
}
