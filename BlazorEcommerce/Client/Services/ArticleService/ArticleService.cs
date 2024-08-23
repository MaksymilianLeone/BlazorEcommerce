
using BlazorEcommerce.Shared;
using static System.Net.WebRequestMethods;

namespace BlazorEcommerce.Client.Services.ArticleService
{
    public class ArticleService : IArticleService
    {
        private readonly HttpClient _http;
        public ArticleService(HttpClient http)
        {
            _http = http;
        }
        public List<Article> Articles { get; set; }
        public List<Article> AdminArticles { get; set; }
        public string Message { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string LastSearchText { get; set; }

        public event Action ArticleChanged;

        public async Task<Article> CreateArticle(Article article)
        {
            Article articles = new Article();
            try
            {
                var result = await _http.PostAsJsonAsync("api/Article", article);
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Handle unauthorized access
                    Message = "You are not authorized to view these articles.";
                    return articles;
                }

                var newArticle = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<Article>>()).Data;

                return newArticle;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching articles: {ex.Message}");
                Message = "Error fetching articles.";
            }
            return articles;
        }

        public Article CreateNewArticle()
        {
            var newArticle = new Article { IsNew = true, Editing = true };
            AdminArticles.Add(newArticle);
            ArticleChanged.Invoke();
            return newArticle;
        }

        public async Task DeleteArticle(Article article)
        {
            var result = await _http.DeleteAsync($"api/Article/{article.Id}");
        }

        public async Task GetAdminArticles()
        {
            try
            {
                var response = await _http.GetAsync("api/Article/admin");
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<Article>>>();
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Handle unauthorized access
                    Message = "You are not authorized to view these articles.";
                    return;
                }

                AdminArticles = result.Data;
                CurrentPage = 1;
                PageCount = 0;
                if (AdminArticles.Count == 0)
                    Message = "No articles found.";
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching articles: {ex.Message}");
                Message = "Error fetching articles.";
            }
        }

        public async Task<ServiceResponse<Article>> GetArticle(int articleId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Article>>($"api/Article/{articleId}");
            return result;
        }

        public async Task GetArticles(string? categoryUrl = null)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Article>>>($"api/Article");
            if (result != null && result.Data != null)
                Articles = result.Data;

            CurrentPage = 1;
            PageCount = 0;

            if (Articles.Count == 0)
                Message = "No articles found";

            ArticleChanged.Invoke();
        }

        public async Task<ServiceResponse<List<Article>>> GetArticlesAsync()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Article>>>("api/Article");
            Articles = response.Data;
            ArticleChanged.Invoke();

            return response;
        }

        public async Task<List<string>> GetArticleSearchSuggestions(string searchText)
        {
            var result = await _http
               .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/Article/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task<Article> UpdateArticle(Article article)
        {
            var result = await _http.PutAsJsonAsync($"api/Article", article);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<Article>>();
            return content.Data;
        }
    }
}
