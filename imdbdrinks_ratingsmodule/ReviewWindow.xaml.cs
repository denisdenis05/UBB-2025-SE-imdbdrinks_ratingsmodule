using System;
using imdbdrinks_ratingsmodule.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace imdbdrinks_ratingsmodule;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ReviewWindow : Window
{
    private readonly IConfiguration configuration;
    private readonly RatingViewModel ratingViewModel;
    private readonly ReviewViewModel reviewViewModel;

    public ReviewWindow(IConfiguration configuration, RatingViewModel ratingViewModel, ReviewViewModel reviewViewModel)
    {
        this.configuration = configuration;
        this.ratingViewModel = ratingViewModel;
        this.reviewViewModel = reviewViewModel;

        this.InitializeComponent();
        this.rootGrid.DataContext = reviewViewModel;
        this.reviewViewModel.RequestClose += CloseWindow;
    }

    private async void SubmitReview_Click(object sender, RoutedEventArgs e)
    {
        if (ratingViewModel.SelectedRating != null)
        {
            reviewViewModel.AddReview(ratingViewModel.SelectedRating.RatingId);
        }
    }

    private void GenerateAIReview_Click(object sender, RoutedEventArgs e)
    {
        var aiReviewWindow = new AIReviewWindow(configuration, OnAIReviewGenerated);
        aiReviewWindow.Activate();
    }

    private void OnAIReviewGenerated(string aiReview)
    {
        reviewViewModel.ReviewContent = aiReview;
    }

    public void CloseWindow(object sender, EventArgs e)
    {
        this.Close();
    }
}