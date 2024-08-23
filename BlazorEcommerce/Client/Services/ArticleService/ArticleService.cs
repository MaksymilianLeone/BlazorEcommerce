
using BlazorEcommerce.Client.Pages.Admin;
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
        public List<Article> AdminArticles { get ; set ; }

        public event Action OnChange;

        public async Task AddArticle(Article article)
        {
            var response = await _http.PostAsJsonAsync("api/Article/admin", article);
            AdminArticles = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Article>>>()).Data;
            await GetArticles();
            OnChange.Invoke();
        }

        public Article CreateNewArticle()
        {
            var newArticle = new Article { IsNew = true, Editing = true };
            AdminArticles.Add(newArticle);
            OnChange.Invoke();
            return newArticle;
        }

        public async Task DeleteArticle(int articleId)
        {
            var response = await _http.DeleteAsync($"api/Article/admin/{articleId}");
            AdminArticles = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Article>>>()).Data;
            await GetArticles();
            OnChange.Invoke();
        }

        public async Task GetAdminArticles()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Article>>>("api/Article/admin");
            if (response != null && response.Data != null)
                AdminArticles = response.Data;
        }

        public async Task GetArticles()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Article>>>("api/Article");
            if (response != null && response.Data != null)
                Articles = response.Data;
        }

        public async Task UpdateArticle(Article article)
        {
            var response = await _http.PutAsJsonAsync("api/Category/admin", article);
            AdminArticles = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Article>>>()).Data;
            await GetArticles();
            OnChange.Invoke();
        }
    }
}
