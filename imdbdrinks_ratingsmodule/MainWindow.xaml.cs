using System;
using imdbdrinks_ratingsmodule.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace imdbdrinks_ratingsmodule;

public sealed partial class MainWindow : Window
{
    private readonly MainViewModel viewModel;

    public MainWindow(MainViewModel viewModel)
    {
        this.InitializeComponent();
        this.viewModel = viewModel;
        this.rootGrid.DataContext = viewModel;
    }

    private async void AddReview_Click(object sender, RoutedEventArgs e)
    {
        if (viewModel.SelectedRating != null)
        {
            var reviewWindow = new ReviewWindow(
                viewModel.Configuration,
                viewModel.RatingViewModel,
                viewModel.ReviewViewModel);
            reviewWindow.Activate();
        }
        else
        {
            await NoRatingSelectedDialog.ShowAsync();
        }
    }

    private void AddRating_Click(object sender, RoutedEventArgs e)
    {
        viewModel.ClearSelectedRating();
        var ratingWindow = new RatingWindow(viewModel.RatingViewModel);
        ratingWindow.Activate();
    }

    private void RatingSelection_Changed(object sender, RoutedEventArgs e)
    {
        if (sender is ListView listView)
        {
            viewModel.HandleRatingSelection(listView);
        }
    }
}
