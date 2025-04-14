namespace imdbdrinks_ratingsmodule;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using imdbdrinks_ratingsmodule.ViewModels;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.ViewHelpers;

public sealed partial class RatingWindow : Window
{
    private readonly RatingViewModel ratingViewModel;
    private const int BottleRatingToIndexOffset = 1;

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

        if (clickedImage.DataContext is not BottleAsset clickedBottle)
            return;

        int clickedBottleNumber = ratingViewModel.Bottles.IndexOf(clickedBottle) + BottleRatingToIndexOffset;

        ratingViewModel.UpdateBottleRating(clickedBottleNumber);
    }

    private void RateButton_Click(object sender, RoutedEventArgs e)
    {
        ratingViewModel.AddRating();

        this.Close();
    }
}
