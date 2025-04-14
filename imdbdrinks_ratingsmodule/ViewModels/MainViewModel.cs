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
        HandleRatingSelectionInternal(listView?.SelectedIndex ?? -1);
    }
    
    // This overload is for testing
    internal void HandleRatingSelectionInternal(int selectedIndex)
    {
        if (selectedIndex >= 0 && selectedIndex < ratingViewModel.Ratings.Count)
        {
            var selectedRating = ratingViewModel.Ratings[selectedIndex];
            ratingViewModel.SelectedRating = selectedRating;
            reviewViewModel.LoadReviewsForRating(selectedRating.RatingId);
        }
    }

    public void ClearSelectedRating()
    {
        ratingViewModel.SelectedRating = null;
    }
} 