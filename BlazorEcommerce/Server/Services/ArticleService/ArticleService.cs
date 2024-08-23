


using BlazorEcommerce.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.ArticleService
{
    public class ArticleService : IArticleService
    {
        private readonly DataContext _context;

        public ArticleService(DataContext context)
        {
            _context = context;
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

            article.Deleted = true;
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
            dbArticle.Visible = article.Visible;

            await _context.SaveChangesAsync();

            return await GetAdminArticles();
        }
    }
}
