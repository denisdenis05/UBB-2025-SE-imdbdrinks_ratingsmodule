// <copyright file="RatingRepositoryErrorMessages.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule.Constants.ErrorMessages
{
    /// <summary>
    /// Static class that holds the error messages related to rating repository issues.
    /// </summary>
    public static class RatingRepositoryErrorMessages
    {
        /// <summary>
        /// Error message indicating that the rating repository is not initialized.
        /// </summary>
        public const string ExhaustSingleRatingReaderMultipleRatingsFound = "Multiple ratings found when expecting a single rating";

        /// <summary>
        /// Error message indicating that the rating repository is not initialized.
        /// </summary>
        public const string ExhaustSingleRatingReaderInvalidReader = "No rating found when expecting a single rating";
    }
}
