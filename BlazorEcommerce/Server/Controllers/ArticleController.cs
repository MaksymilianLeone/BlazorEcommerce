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

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Article>>>> GetAdminArticles()
        {
            var result = await _articleService.GetAdminArticles();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Article>>> CreateArticle(Article article)
        {
            var result = await _articleService.CreateArticle(article);
            if (result.Success)
            {
                return Ok(result);

            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<Article>>> UpdateArticle(Article article)
        {
            var result = await _articleService.UpdateArticle(article);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteArticle(int id)
        {
            var result = await _articleService.DeleteArticle(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Article>>>> GetArticles()
        {
            var result = await _articleService.GetArticles();
            return Ok(result);
        }

        [HttpGet("{articleId}")]
        public async Task<ActionResult<ServiceResponse<Article>>> GetArticle(int articleId)
        {
            var result = await _articleService.GetArticleAsync(articleId);
            return Ok(result);
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Article>>>> GetArticlesByCategory(string categoryUrl)
        {
            var result = await _articleService.GetArticlesByCategory(categoryUrl);
            return Ok(result);
        }


        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Article>>>> GetArticleSearchSuggestions(string searchText)
        {
            var result = await _articleService.GetArticleSearchSuggestions(searchText);
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Article>>>> GetFeaturedArticles()
        {
            var result = await _articleService.GetFeaturedArticles();
            return Ok(result);
        }
    }
}
