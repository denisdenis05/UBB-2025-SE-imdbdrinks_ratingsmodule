using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;

namespace imdbdrinks_ratingsmodule.ViewModels;

public class ReviewViewModel : ViewModelBase
{
    public event EventHandler RequestClose;
    private readonly ReviewService reviewService;
    private ObservableCollection<Review> reviews;
    private Review selectedReview;
    private string reviewContent;
    private const int defaultUserId = 999;

    public virtual ObservableCollection<Review> Reviews
    {
        get => reviews;
        set => SetProperty(ref reviews, value);
    }

    public virtual Review SelectedReview
    {
        get => selectedReview;
        set => SetProperty(ref selectedReview, value);
    }

    public virtual string ReviewContent
    {
        get => reviewContent;
        set => SetProperty(ref reviewContent, value);
    }

    public ReviewViewModel(ReviewService reviewService)
    {
        this.reviewService = reviewService;
        Reviews = new ObservableCollection<Review>();
    }

    public virtual void LoadReviewsForRating(int ratingId)
    {
        var reviews = reviewService.GetReviewsByRating(ratingId);
        Reviews.Clear();
        foreach (var review in reviews)
        {
            Reviews.Add(review);
        }
    }

    public virtual void AddReview(int ratingId)
    {

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
        try {
            reviewService.AddReview(newReview);
        }
        catch (Exception ex)
        {
            return;
        }
        LoadReviewsForRating(ratingId);
        ReviewContent = string.Empty;
        CloseWindow();
    }

    public virtual void ClearReviewContent()
    {
        ReviewContent = string.Empty;
    }
    private void CloseWindow()
    {
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}