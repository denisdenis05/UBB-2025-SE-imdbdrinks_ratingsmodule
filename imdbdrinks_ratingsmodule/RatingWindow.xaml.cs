using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace imdbdrinks_ratingsmodule
{
    /// <summary>
    /// Window that allows users to provide a rating for a drink product.
    /// Displays a set of bottle images that can be clicked to select a rating score.
    /// </summary>
    public sealed partial class RatingWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Collection of bottle images representing the rating scale.
        /// </summary>
        public ObservableCollection<BottleIcon> RatingBottles { get; set; }
        
        /// <summary>
        /// The user's selected rating score (1-5).
        /// </summary>
        public int UserRatingScore { get; set; }

        /// <summary>
        /// Text to display the current rating selection.
        /// </summary>
        public string RatingFeedbackText { get; set; }

        // Resource paths for bottle images
        private readonly string _emptyBottleImagePath = "ms-appx:///Assets/Bottle.png";
        private readonly string _filledBottleImagePath = "ms-appx:///Assets/FullBottle.png";

        /// <summary>
        /// Event that is triggered when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        // View model for rating operations
        private readonly RatingViewModel _ratingViewModel;

        /// <summary>
        /// Initializes a new instance of the RatingWindow class.
        /// </summary>
        /// <param name="viewModel">The rating view model to use for data operations.</param>
        public RatingWindow(RatingViewModel viewModel)
        {
            this.InitializeComponent();
            RatingBottles = new ObservableCollection<BottleIcon>();
            RatingFeedbackText = "Select a rating from 1 to 5";

            // Initialize 5 bottles in an empty (unrated) state
            for (int i = 0; i < 5; i++)
            {
                RatingBottles.Add(new BottleIcon { ImageSource = _emptyBottleImagePath });
            }

            rootGrid.DataContext = this;
            _ratingViewModel = viewModel;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Handles the click event when a bottle icon is tapped.
        /// Updates the bottle display and saves the rating score.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments containing information about the tap.</param>
        private void Bottle_Click(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Image clickedImage && clickedImage.DataContext is BottleIcon clickedBottle)
            {
                int selectedIndex = RatingBottles.IndexOf(clickedBottle);

                // Update all bottles based on the selected rating
                for (int i = 0; i < RatingBottles.Count; i++)
                {
                    // Fill bottles up to and including the clicked one, empty the rest
                    RatingBottles[i].ImageSource = i <= selectedIndex 
                        ? _filledBottleImagePath 
                        : _emptyBottleImagePath;
                }
                
                // Store the rating value (1-5)
                UserRatingScore = selectedIndex + 1;
                
                // Update the feedback text
                RatingFeedbackText = $"You selected {UserRatingScore} out of 5 bottles";
                OnPropertyChanged(nameof(RatingFeedbackText));
            }
        }

        /// <summary>
        /// Handles the click event when the Rate button is clicked.
        /// Creates and saves a new rating based on the user's selection.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments for the button click.</param>
        private async void RateButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate that a rating was selected
            if (UserRatingScore == 0)
            {
                // Display a message dialog to inform the user they need to select a rating
                ContentDialog noRatingDialog = new ContentDialog
                {
                    Title = "No Rating Selected",
                    Content = "Please select a rating by clicking on one of the bottles before submitting.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                
                await noRatingDialog.ShowAsync();
                return;
            }

            // Create a new rating object with the user's selection
            Rating newRating = new Rating
            {
                ProductId = 100, // TODO: Replace with actual product ID from context
                RatingValue = UserRatingScore,
                UserId = _ratingViewModel.Ratings.Count + 1 // TODO: Replace with actual user ID
            };

            // Save the rating via the view model
            _ratingViewModel.AddRating(newRating);
            
            // Show a confirmation message before closing
            ContentDialog confirmationDialog = new ContentDialog
            {
                Title = "Rating Submitted",
                Content = $"Thank you! You rated this drink {UserRatingScore} out of 5 bottles.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            
            await confirmationDialog.ShowAsync();

            // Close the rating window
            this.Close();
        }
    }

    /// <summary>
    /// Represents a bottle icon in the rating system.
    /// Implements property change notification to support UI updates.
    /// </summary>
    public class BottleIcon : INotifyPropertyChanged
    {
        private string _imageSource;
        
        /// <summary>
        /// Gets or sets the image source path for the bottle icon.
        /// </summary>
        public string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSource)));
            }
        }

        /// <summary>
        /// Event that is triggered when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
