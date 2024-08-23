namespace BlazorEcommerce.Client.Services.ArticleService
{
    public interface IArticleService
    {
        event Action ArticleChanged;
        List<Article> Articles { get; set; }
        List<Article> AdminArticles { get; set;}
        string Message { get; set; }  
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchText { get; set; }
        Task GetArticles(string? categoryUrl =null);
        Task<ServiceResponse<Article>> GetArticle(int articleId);

        Task<List<string>> GetArticleSearchSuggestions(string searchText);

        Task GetAdminArticles();

        Task<Article> CreateArticle(Article article); 
        Task<Article> UpdateArticle(Article article);
        Task<ServiceResponse<List<Article>>> GetArticlesAsync();
        Task DeleteArticle(Article article);
        Article CreateNewArticle();
    }
}
