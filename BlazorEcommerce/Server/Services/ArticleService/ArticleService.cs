


using BlazorEcommerce.Client.Pages.Admin;
using BlazorEcommerce.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.ArticleService
{
    public class ArticleService : IArticleService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArticleService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<List<Article>>> AddArticle(Article article)
        {
            article.Editing = article.IsNew = false;
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return await GetAdminArticles();
        }

        public async Task<ServiceResponse<List<Article>>> DeleteArticle(int id)
        {
            Article article = await GetArticleById(id);
            if (article == null)
            {
                return new ServiceResponse<List<Article>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return await GetAdminArticles();
        }
        private async Task<Article> GetArticleById(int id)
        {
            return await _context.Articles.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<ServiceResponse<List<Article>>> GetAdminArticles()
        {
            try
            {
                var articles = await _context.Articles
               .Where(c => !c.Deleted)
               .ToListAsync();
                return new ServiceResponse<List<Article>>
                {
                    Data = articles
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
        }

        public async Task<ServiceResponse<List<Article>>> GetArticles()
        {
            var articles = await _context.Articles
                .Where(c => !c.Deleted && c.Visible)
                .ToListAsync();
            return new ServiceResponse<List<Article>>
            {
                Data = articles
            };
        }

        public async Task<ServiceResponse<List<Article>>> UpdateArticle(Article article)
        {
            var dbArticle = await GetArticleById(article.Id);
            if (dbArticle == null)
            {
                return new ServiceResponse<List<Article>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            dbArticle.Title = article.Title;
            dbArticle.Description = article.Description;
            dbArticle.Visible = article.Visible;

            await _context.SaveChangesAsync();

            return await GetAdminArticles();
        }

        public async Task<ServiceResponse<Article>> GetArticleAsync(int articleId)
        {
            var response = new ServiceResponse<Article>();
            Article article = null;

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                article = await _context.Articles
                    .FirstOrDefaultAsync(p => p.Id == articleId && !p.Deleted);
            }
            else
            {
                article = await _context.Articles
                    .FirstOrDefaultAsync(p => p.Id == articleId && !p.Deleted && p.Visible);
            }

            if (article == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this product does not exist.";
            }
            else
            {
                response.Data = article;
            }

            return response;
        }
    }
}
