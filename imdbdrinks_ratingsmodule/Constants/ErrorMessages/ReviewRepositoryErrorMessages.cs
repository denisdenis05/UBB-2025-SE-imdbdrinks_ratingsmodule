// <copyright file="ReviewRepositoryErrorMessages.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule.Constants.ErrorMessages
{
    /// <summary>
    /// Error messages for the review repository.
    /// </summary>
    public static class ReviewRepositoryErrorMessages
    {
        /// <summary>
        /// Error message for when a single review reader contains multiple entries.
        /// </summary>
        public const string ExhaustSingleReviewReaderMultipleReviewsFound =
            "Multiple reviews found for a unique ID.";

        /// <summary>
        /// Error message for when a reader given to the exhaust function is invalid.
        /// </summary>
        public const string ExhaustSingleReviewReaderInvalidReader =
            "Invalid reader passed.";
    }
}
