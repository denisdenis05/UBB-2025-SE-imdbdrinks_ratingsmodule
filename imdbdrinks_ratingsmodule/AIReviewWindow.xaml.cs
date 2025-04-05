using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace imdbdrinks_ratingsmodule
{
    public sealed partial class AIReviewWindow : Window
    {
        private readonly string API_Key;

        private readonly string[] SpinnerGifs = new[]
        {
            "Assets/pizzaSpin.gif",
            "Assets/pizzaSpin2.gif",
            "Assets/pizzaSpin3.gif",
            "Assets/pizzaSpin4.gif"
        };
        private readonly Random _random = new();

        private readonly HttpClient _client;
        //string ApiKey; // Replace with your OpenRouter API Key
        private const string ApiUrl = "https://openrouter.ai/api/v1/chat/completions";
        private const string Model = "deepseek/deepseek-r1-zero:free";
        private readonly Action<string> _onReviewGenerated;
        private string _apiKeyLoadError;

        public AIReviewWindow(IConfiguration config, Action<string> onReviewGenerated)
        {
            API_Key = config["OPENROUTER_API_KEY"];
            if (string.IsNullOrEmpty(API_Key))
            {
                throw new InvalidOperationException("API key is missing or empty.");
            }

            this.InitializeComponent();
            _client = new HttpClient();
            _onReviewGenerated = onReviewGenerated;
        }


        private async void SubmitReview_Click(object sender, RoutedEventArgs e)
        {
            string userKeywords = AIReviewTextBox.Text.ToLower();

            if (string.IsNullOrWhiteSpace(userKeywords))
            {
                ShowDialog("Error", "Please enter some words to generate a review.");
                return;
            }

            try
            {
                string selectedGif = SpinnerGifs[_random.Next(SpinnerGifs.Length)];
                Spinner.Source = new BitmapImage(new Uri($"ms-appx:///{selectedGif}"));
                Spinner.Visibility = Visibility.Visible;

                string aiGeneratedReview = await GenerateReviewFromOpenRouter(userKeywords);
                ShowDialog("AI-Generated Review", aiGeneratedReview);
            }
            finally
            {
                Spinner.Visibility = Visibility.Collapsed; 
            }

        }

        private async Task<string> GenerateReviewFromOpenRouter(string keywords)
        {
            try
            {
             
                string prompt = $"Write a short and natural-sounding review (2–3 sentences) that includes the following words:  \"{keywords}\". The review should describe the drink realistically, as if written by a genuine customer. Do not mention the name of the drink anywhere—just focus on how it tastes, feels, or is experienced. All given words must be used exactly as they are, and the review should feel coherent and honest.\r\n";

                var requestBody = new
                {
                    model = Model,
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    }
                };

                string jsonRequest = JsonSerializer.Serialize(requestBody);
                var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl)
                {
                    Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", API_Key);
                request.Headers.Add("HTTP-Referer", "https://your-site.com"); // Optional
                request.Headers.Add("X-Title", "IMDBDrinks App"); // Optional

                HttpResponseMessage response = await _client.SendAsync(request);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    string aiRaw = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                    string cleaned = aiRaw.Replace("\\boxed{", "").Replace("```", "").Trim();

                    if (cleaned.EndsWith("}"))
                        cleaned = cleaned.Substring(0, cleaned.Length - 1);

                    if (cleaned.StartsWith("\"") && cleaned.EndsWith("\""))
                        cleaned = cleaned.Substring(1, cleaned.Length - 2).Trim();

                    return cleaned;
                    //return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                }
                else
                {
                    return $"Error: {response.StatusCode} \n Full Response: {jsonResponse}";
                }
            }
            catch (HttpRequestException ex)
            {
                return $"Network Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Unexpected Error: {ex.Message}";
            }
        }

        private async void ShowDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();

            _onReviewGenerated?.Invoke(content);
            this.Close();

        }

    }
}