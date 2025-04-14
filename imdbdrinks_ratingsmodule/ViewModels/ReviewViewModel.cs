using System.Collections.ObjectModel;
using System.Diagnostics;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;

namespace imdbdrinks_ratingsmodule.ViewModels;

public class ReviewViewModel : ViewModelBase
{
    private readonly ReviewService reviewService;
    private ObservableCollection<Review> reviews;
    private Review selectedReview;
    private string reviewContent;
    private const int defaultUserId = 999;

    public ObservableCollection<Review> Reviews
    {
        get => reviews;
        set => SetProperty(ref reviews, value);
    }

    public Review SelectedReview
    {
        get => selectedReview;
        set => SetProperty(ref selectedReview, value);
    }

    public string ReviewContent
    {
        get => reviewContent;
        set => SetProperty(ref reviewContent, value);
    }

    public ReviewViewModel(ReviewService reviewService)
    {
        this.reviewService = reviewService;
        Reviews = new ObservableCollection<Review>();
    }

    public void LoadReviewsForRating(int ratingId)
    {
        var reviews = reviewService.GetReviewsByRating(ratingId);
        Reviews.Clear();
        foreach (var review in reviews)
        {
            Reviews.Add(review);
        }
    }

    public void AddReview(int ratingId)
    {
        Debug.WriteLine(ReviewContent);

        if (string.IsNullOrWhiteSpace(ReviewContent))
        {
            return;
        }

        var newReview = new Review
        {
            RatingId = ratingId,
            UserId = defaultUserId,
            Content = ReviewContent,
            IsActive = true
        };

        reviewService.AddReview(newReview);
        LoadReviewsForRating(ratingId);
        ReviewContent = string.Empty;
    }

    public void ClearReviewContent()
    {
        ReviewContent = string.Empty;
    }
}