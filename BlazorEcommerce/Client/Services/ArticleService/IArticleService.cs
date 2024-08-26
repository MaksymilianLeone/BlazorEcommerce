namespace BlazorEcommerce.Client.Services.ArticleService
{
    public interface IArticleService
    {
        event Action OnChange;
        List<Article> Articles { get; set; }
        List<Article> AdminArticles { get; set; }
        Task GetArticles();
        Task<ServiceResponse<Article>> GetArticle(int articleId);
        Task GetAdminArticles();
        Task AddArticle(Article article);
        Task UpdateArticle(Article article);
        Task DeleteArticle(int articleId);
        Article CreateNewArticle();
    }
}
