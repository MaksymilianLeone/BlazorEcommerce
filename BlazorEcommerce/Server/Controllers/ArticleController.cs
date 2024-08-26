using BlazorEcommerce.Server.Services.ArticleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Article>>>> GetArticles()
        {
            var result = await _articleService.GetArticles();
            return Ok(result);
        }

        [HttpGet("admin")/*, Authorize(Roles = "Admin")*/]
        public async Task<ActionResult<ServiceResponse<List<Article>>>> GetAdminArticles()
        {
            var result = await _articleService.GetAdminArticles();
            return Ok(result);
        }

        [HttpDelete("admin/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Article>>>> DeleteArticle(int id)
        {
            var result = await _articleService.DeleteArticle(id);
            return Ok(result);
        }

        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Article>>>> AddCategory(Article article)
        {
            var result = await _articleService.AddArticle(article);
            return Ok(result);
        }

        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Article>>>> UpdateArticle(Article article)
        {
            var result = await _articleService.UpdateArticle(article);
            return Ok(result);
        }
    }
}
