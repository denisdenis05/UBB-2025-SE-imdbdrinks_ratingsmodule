// <copyright file="AIReviewWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media.Imaging;

    /// <summary>
    /// Represents a window for generating AI reviews using OpenRouter API.
    /// </summary>
    public sealed partial class AIReviewWindow : Window
    {
        private const string ApiUrl = "https://openrouter.ai/api/v1/chat/completions";
        private const string Model = "deepseek/deepseek-r1-zero:free";

        private readonly string apiKey;
        private readonly string[] spinnerGifs = new[]
        {
            "Assets/pizzaSpin.gif",
            "Assets/pizzaSpin2.gif",
            "Assets/pizzaSpin3.gif",
            "Assets/pizzaSpin4.gif",
        };

        private readonly Random random = new ();
        private readonly HttpClient client;
        private readonly Action<string> onReviewGenerated;

        /// <summary>
        /// Initializes a new instance of the <see cref="AIReviewWindow"/> class.
        /// </summary>
        /// <param name="configuration">Injected configuration file used for the api key.</param>
        /// <param name="onReviewGenerated">Action to invoke once review is generated.</param>
        /// <exception cref="InvalidOperationException">Exception called if config api key is missing.</exception>
        public AIReviewWindow(IConfiguration configuration, Action<string> onReviewGenerated)
        {
            this.apiKey = configuration["OPENROUTER_API_KEY"] ?? throw new InvalidOperationException("API key is missing or empty.");
            this.InitializeComponent();
            this.client = new HttpClient();
            this.onReviewGenerated = onReviewGenerated;
        }

        /// <summary>
        /// Handles the click event for the SubmitReview button.
        /// </summary>
        /// <param name="sender">Sender object for the event.</param>
        /// <param name="e">Arguments of event.</param>
        private async void SubmitReview_Click(object sender, RoutedEventArgs e)
        {
            string userKeywords = this.AIReviewTextBox.Text.ToLower();

            if (string.IsNullOrWhiteSpace(userKeywords))
            {
                this.ShowDialog("Error", "Please enter some words to generate a review.");
                return;
            }

            try
            {
                string selectedGif = this.spinnerGifs[this.random.Next(this.spinnerGifs.Length)];
                this.Spinner.Source = new BitmapImage(new Uri($"ms-appx:///{selectedGif}"));
                this.Spinner.Visibility = Visibility.Visible;

                string aiGeneratedReview = await this.GenerateReviewFromOpenRouter(userKeywords);
                this.ShowDialog("AI-Generated Review", aiGeneratedReview);
            }
            finally
            {
                this.Spinner.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Generates a review using the OpenRouter API based on the provided keywords.
        /// </summary>
        /// <param name="keywords">The keywords used for the prompt.</param>
        /// <returns>A string task since async.</returns>
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
                        new { role = "user", content = prompt, },
                    },
                };

                string jsonRequest = JsonSerializer.Serialize(requestBody);
                var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl)
                {
                    Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json"),
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.apiKey);
                request.Headers.Add("HTTP-Referer", "https://your-site.com"); // Optional
                request.Headers.Add("X-Title", "IMDBDrinks App"); // Optional

                HttpResponseMessage response = await this.client.SendAsync(request);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    string? aiRaw = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                    string? cleaned = aiRaw?.Replace("\\boxed{", string.Empty).Replace("```", string.Empty).Trim();

                    if (cleaned is null)
                    {
                        return "Error: AI response is null.";
                    }

                    if (cleaned.EndsWith('}'))
                    {
                        cleaned = cleaned.Substring(0, cleaned.Length - 1);
                    }

                    if (cleaned.StartsWith("\"") && cleaned.EndsWith("\""))
                    {
                        cleaned = cleaned.Substring(1, cleaned.Length - 2).Trim();
                    }

                    return cleaned;
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
                XamlRoot = this.Content.XamlRoot,
            };
            await dialog.ShowAsync();

            this.onReviewGenerated?.Invoke(content);
            this.Close();
        }
    }
}