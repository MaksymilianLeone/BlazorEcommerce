namespace BlazorEcommerce.Server.Services.ArticleService
{
    public interface IArticleService
    {
        Task<ServiceResponse<List<Article>>> GetArticlesAsync();
        Task<ServiceResponse<List<Article>>> GetArticles();
        Task<ServiceResponse<Article>> GetArticleAsync(int articleId);
        Task<ServiceResponse<List<Article>>> GetArticlesByCategory(string categoryUrl);
        Task<ServiceResponse<List<string>>> GetArticleSearchSuggestions(string searchText);
        Task<ServiceResponse<List<Article>>> GetFeaturedArticles();
        Task<ServiceResponse<List<Article>>> GetAdminArticles();
        Task<ServiceResponse<Article>> CreateArticle(Article article);
        Task<ServiceResponse<Article>> UpdateArticle(Article article);
        Task<ServiceResponse<bool>> DeleteArticle(int articleId);
    }
}
