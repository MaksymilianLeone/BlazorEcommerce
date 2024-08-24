namespace BlazorEcommerce.Server.Services.ArticleService
{
    public interface IArticleService
    {
        Task<ServiceResponse<List<Article>>> GetArticles();
        Task<ServiceResponse<Article>> GetArticleAsync(int articleId);
        Task<ServiceResponse<List<Article>>> GetAdminArticles();
        Task<ServiceResponse<List<Article>>> AddArticle(Article article);
        Task<ServiceResponse<List<Article>>> UpdateArticle(Article article);
        Task<ServiceResponse<List<Article>>> DeleteArticle(int id);
    }
}
