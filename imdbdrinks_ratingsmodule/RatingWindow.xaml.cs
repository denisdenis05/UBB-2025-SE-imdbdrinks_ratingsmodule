namespace imdbdrinks_ratingsmodule;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using imdbdrinks_ratingsmodule.ViewModels;
using imdbdrinks_ratingsmodule.Domain;

public sealed partial class RatingWindow : Window
{
    private readonly RatingViewModel ratingViewModel;

    public RatingWindow(RatingViewModel viewModel)
    {
        this.InitializeComponent();
        this.ratingViewModel = viewModel;
        this.rootGrid.DataContext = viewModel;
    }

    private void Bottle_Click(object sender, TappedRoutedEventArgs e)
    {
        if (sender is not Image clickedImage)
            return;

        if (clickedImage.DataContext is not Bottle clickedBottle)
            return;

        const int bottleRatingToIndexOffset = 1;
        int clickedBottleNumber = ratingViewModel.Bottles.IndexOf(clickedBottle) + bottleRatingToIndexOffset;

        ratingViewModel.UpdateBottleRating(clickedBottleNumber);
    }

    private void RateButton_Click(object sender, RoutedEventArgs e)
    {
        ratingViewModel.AddRating();

        this.Close();
    }
}
