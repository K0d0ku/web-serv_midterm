using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

//https://localhost:5001/Customer/Register
namespace KuroApi.Pages.Customer
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _http;

        public RegisterModel(IHttpClientFactory clientFactory)
        {
            _http = clientFactory.CreateClient();
        }

        [BindProperty]
        public string Nickname { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var customer = new
            {
                nickname = Nickname,
                email = Email,
                password = Password
            };

            try
            {
                var response = await _http.PostAsJsonAsync("https://localhost:5255/api/auth/register", customer);
                if (response.IsSuccessStatusCode)
                {
                    Message = "Registration successful!";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Message = $"Error: {error}";
                }
            }
            catch (System.Exception ex)
            {
                Message = $"Exception: {ex.Message}";
            }

            return Page();
        }
    }
}
