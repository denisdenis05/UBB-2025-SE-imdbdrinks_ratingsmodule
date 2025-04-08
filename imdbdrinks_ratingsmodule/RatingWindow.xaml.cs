using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;

namespace imdbdrinks_ratingsmodule
{
    public sealed partial class RatingWindow : Window
    {
        private int _selectedRating;
        private readonly RatingViewModel _ratingViewModel;

        public RatingWindow(RatingViewModel viewModel)
        {
            this.InitializeComponent();
            _ratingViewModel = viewModel;
            _selectedRating = 0;
        }

        private void UpdateBottles(int rating)
        {
            // Update visibility of all bottles based on the selected rating
            Bottle1Full.Visibility = rating >= 1 ? Visibility.Visible : Visibility.Collapsed;
            Bottle2Full.Visibility = rating >= 2 ? Visibility.Visible : Visibility.Collapsed;
            Bottle3Full.Visibility = rating >= 3 ? Visibility.Visible : Visibility.Collapsed;
            Bottle4Full.Visibility = rating >= 4 ? Visibility.Visible : Visibility.Collapsed;
            Bottle5Full.Visibility = rating >= 5 ? Visibility.Visible : Visibility.Collapsed;

            // Update the rating text
            _selectedRating = rating;
            RatingTextBlock.Text = $"{_selectedRating} bottle{(_selectedRating != 1 ? "s" : "")}";
            RatingTextBlock.Visibility = _selectedRating > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Bottle1_Click(object sender, TappedRoutedEventArgs e)
        {
            UpdateBottles(1);
        }

        private void Bottle2_Click(object sender, TappedRoutedEventArgs e)
        {
            UpdateBottles(2);
        }

        private void Bottle3_Click(object sender, TappedRoutedEventArgs e)
        {
            UpdateBottles(3);
        }

        private void Bottle4_Click(object sender, TappedRoutedEventArgs e)
        {
            UpdateBottles(4);
        }

        private void Bottle5_Click(object sender, TappedRoutedEventArgs e)
        {
            UpdateBottles(5);
        }

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            // Only proceed if a rating has been selected
            if (_selectedRating == 0)
            {
                return;
            }

            Rating rating = new Rating
            {
                ProductId = 100, // mock value, should be replaced with actual product id
                RatingValue = _selectedRating,
                UserId = _ratingViewModel.Ratings.Count + 1 // mock value, should be replaced with actual user id
            };

            _ratingViewModel.AddRating(rating);
            this.Close();
        }
        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
