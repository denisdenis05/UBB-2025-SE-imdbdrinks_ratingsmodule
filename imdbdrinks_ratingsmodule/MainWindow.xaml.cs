// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule
{
    using System;
    using imdbdrinks_ratingsmodule.ViewModels;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// Represents the main window of the application.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model for the main window.</param>
        public MainWindow(MainViewModel viewModel)
        {
            this.InitializeComponent();
            this.viewModel = viewModel;
            this.rootGrid.DataContext = viewModel;
        }

        /// <summary>
        /// Handles the click event for the Add Review button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private async void AddReview_Click(object sender, RoutedEventArgs e)
        {
            if (this.viewModel.SelectedRating != null)
            {
                var reviewWindow = new ReviewWindow(
                    this.viewModel.Configuration,
                    this.viewModel.RatingViewModel,
                    this.viewModel.ReviewViewModel);
                reviewWindow.Activate();
            }
            else
            {
                await this.NoRatingSelectedDialog.ShowAsync();
            }
        }

        /// <summary>
        /// Handles the click event for the Add Rating button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void AddRating_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.ClearSelectedRating();
            var ratingWindow = new RatingWindow(this.viewModel.RatingViewModel);
            ratingWindow.Activate();
        }

        /// <summary>
        /// Handles the rating selection changed event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="eventArguments">The event arguments.</param>
        private void RatingSelection_Changed(object sender, RoutedEventArgs eventArguments)
        {
            if (sender is ListView listView)
            {
                this.viewModel.HandleRatingSelection(listView);
            }
        }
    }
}
