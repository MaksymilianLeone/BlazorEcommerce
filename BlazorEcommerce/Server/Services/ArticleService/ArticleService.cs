
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

        public async Task<ServiceResponse<Article>> CreateArticle(Article article)
        {
            try
            {
                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
                return new ServiceResponse<Article>
                {
                    Data = article,
                    Message = "Article Created!",
                    Success = true
                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<Article>
                {
                    Data = article,
                    Message = ex.Message,
                    Success = false
                };
                throw;
            }
        }

        public async Task<ServiceResponse<bool>> DeleteArticle(int articleId)
        {
            var dbProduct = await _context.Articles.FindAsync(articleId);
            if (dbProduct == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Article not found."
                };
            }

            _context.Articles.Remove(dbProduct);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<List<Article>>> GetAdminArticles()
        {
            ServiceResponse<List<Article>> Data = new ServiceResponse<List<Article>>();

            var articles = await _context.Articles
           .Where(c => !c.Deleted)
           .ToListAsync();
            return new ServiceResponse<List<Article>>
            {
                Data = articles
            };
        }

        public async Task<ServiceResponse<Article>> GetArticleAsync(int articleId)
        {
            var response = new ServiceResponse<Article>();
            Article article = null;


            article = await _context.Articles
                .FirstOrDefaultAsync(p => p.Id == articleId && !p.Deleted);



            if (article == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this article does not exist.";
            }
            else
            {
                response.Data = article;
            }

            return response;
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
        public async Task<ServiceResponse<List<Article>>> GetArticlesAsync()
        {
            var response = new ServiceResponse<List<Article>>
            {
                Data = await _context.Articles
                  .Where(p => p.Visible && !p.Deleted)
                  .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<Article>>> GetArticlesByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Article>>
            {
                Data = await _context.Articles
                   .Where(p => p.Visible && !p.Deleted)
                   .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetArticleSearchSuggestions(string searchText)
        {
            var articles = await FindNewsBySearchText(searchText);

            List<string> result = new List<string>();

            foreach (var article in articles)
            {
                if (article.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(article.Title);
                }

                if (article.Description != null)
                {
                    var punctuation = article.Description.Where(char.IsPunctuation)
                        .Distinct().ToArray();
                    var words = article.Description.Split()
                        .Select(s => s.Trim(punctuation));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                            && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<List<Article>>> GetFeaturedArticles()
        {
            var response = new ServiceResponse<List<Article>>
            {
                Data = await _context.Articles
                .Where(p => p.Featured && p.Visible && !p.Deleted)
                .ToListAsync()
            };

            return response;

        }


        public async Task<ServiceResponse<Article>> UpdateArticle(Article article)
        {
            var dbProduct = await _context.Articles
                 .FirstOrDefaultAsync(p => p.Id == article.Id);

            if (dbProduct == null)
            {
                return new ServiceResponse<Article>
                {
                    Success = false,
                    Message = "Article not found."
                };
            }

            dbProduct.Title = article.Title;
            dbProduct.Description = article.Description;
            dbProduct.Visible = article.Visible;
            dbProduct.Featured = article.Featured;


            await _context.SaveChangesAsync();
            return new ServiceResponse<Article> { Data = article };
        }
        private async Task<List<Article>> FindNewsBySearchText(string searchText)
        {
            return await _context.Articles
                                .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) ||
                                    p.Description.ToLower().Contains(searchText.ToLower()) &&
                                    p.Visible && !p.Deleted)
                                .ToListAsync();
        }
    }
}
