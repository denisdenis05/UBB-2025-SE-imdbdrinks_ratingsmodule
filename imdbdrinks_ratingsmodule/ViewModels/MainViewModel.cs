using imdbdrinks_ratingsmodule.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Xaml.Controls;

namespace imdbdrinks_ratingsmodule.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IConfiguration configuration;
    private RatingViewModel ratingViewModel;
    private ReviewViewModel reviewViewModel;

    public IConfiguration Configuration => configuration;

    public RatingViewModel RatingViewModel
    {
        get => ratingViewModel;
        set => SetProperty(ref ratingViewModel, value);
    }

    public ReviewViewModel ReviewViewModel
    {
        get => reviewViewModel;
        set => SetProperty(ref reviewViewModel, value);
    }

    public Rating SelectedRating
    {
        get => ratingViewModel.SelectedRating;
    }

    public MainViewModel(IConfiguration configuration, RatingViewModel ratingViewModel, ReviewViewModel reviewViewModel)
    {
        this.configuration = configuration;
        this.ratingViewModel = ratingViewModel;
        this.reviewViewModel = reviewViewModel;

        InitializeData();
    }

    private void InitializeData()
    {
        const int defaultProductId = 100;
        ratingViewModel.LoadRatingsForProduct(defaultProductId);
    }

    public void HandleRatingSelection(ListView listView)
    {
        if (listView?.SelectedIndex >= 0)
        {
            var selectedRating = ratingViewModel.Ratings[listView.SelectedIndex];
            ratingViewModel.SelectedRating = selectedRating;
            reviewViewModel.LoadReviewsForRating(selectedRating.RatingId);
        }
    }

    public void ClearSelectedRating()
    {
        ratingViewModel.SelectedRating = null;
    }
} 