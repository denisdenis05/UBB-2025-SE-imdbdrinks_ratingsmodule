namespace imdbdrinks_ratingsmodule.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using imdbdrinks_ratingsmodule.Constants.ErrorMessages;
    using imdbdrinks_ratingsmodule.Domain;
    using imdbdrinks_ratingsmodule.Queries;
    using Microsoft.Data.SqlClient;

    public static class DatabaseRatingRepositoryHelper
    {
        public static SqlCommand CreateAddRatingCommand(SqlConnection connection, Rating rating)
        {
            SqlCommand addCommand = new (RatingQueries.AddRatingQuery, connection);
            addCommand.Parameters.AddWithValue("@ProductId", rating.ProductId);
            addCommand.Parameters.AddWithValue("@UserId", rating.UserId);
            addCommand.Parameters.AddWithValue("@RatingValue", rating.RatingValue);
            addCommand.Parameters.AddWithValue("@RatingDate", rating.RatingDate);
            addCommand.Parameters.AddWithValue("@IsActive", rating.IsActive);

            return addCommand;
        }

        public static SqlCommand CreateUpdateRatingCommand(SqlConnection connection, Rating rating)
        {
            SqlCommand updateCommand = new (RatingQueries.UpdateRatingQuery, connection);
            updateCommand.Parameters.AddWithValue("@RatingId", rating.RatingId);
            updateCommand.Parameters.AddWithValue("@ProductId", rating.ProductId);
            updateCommand.Parameters.AddWithValue("@UserId", rating.UserId);
            updateCommand.Parameters.AddWithValue("@RatingValue", rating.RatingValue);
            updateCommand.Parameters.AddWithValue("@RatingDate", rating.RatingDate);
            updateCommand.Parameters.AddWithValue("@IsActive", rating.IsActive);

            return updateCommand;
        }

        public static SqlCommand CreateCheckIfRatingWithIdExistsCommand(SqlConnection connection, int ratingId)
        {
            SqlCommand existsCommand = new(RatingQueries.CheckIfRatingWithIdExistsQuery, connection);
            existsCommand.Parameters.AddWithValue("@RatingId", ratingId);
            return existsCommand;
        }

        public static SqlCommand CreateGetRatingsByProductIdCommand(SqlConnection connection, int productId)
        {
            SqlCommand getRatingsCommand = new (RatingQueries.GetRatingsByProductIdQuery, connection);
            getRatingsCommand.Parameters.AddWithValue("@ProductId", productId);

            return getRatingsCommand;
        }

        public static SqlCommand CreateGetRatingByIdCommand(SqlConnection connection, int ratingId)
        {
            SqlCommand getRatingCommand = new (RatingQueries.GetRatingByIdQuery, connection);
            getRatingCommand.Parameters.AddWithValue("@RatingId", ratingId);

            return getRatingCommand;
        }

        public static SqlCommand CreateGetAllRatingsCommand(SqlConnection connection)
        {
            SqlCommand getAllRatingsCommand = new (RatingQueries.GetAllRatingsQuery, connection);
            return getAllRatingsCommand;
        }

        public static SqlCommand CreateDeleteRatingById(SqlConnection connection, int ratingId)
        {
            SqlCommand deleteCommand = new (RatingQueries.DeleteRatingQuery, connection);
            deleteCommand.Parameters.AddWithValue("@RatingId", ratingId);

            return deleteCommand;
        }

        public static IEnumerable<Rating> ExhaustRatingReader(SqlDataReader reader)
        {
            var ratings = new List<Rating>();
            while (reader.Read())
            {
                ratings.Add(new Rating
                {
                    RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    RatingValue = reader.GetDouble(reader.GetOrdinal("RatingValue")),
                    RatingDate = reader.GetDateTime(reader.GetOrdinal("RatingDate")),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                });
            }

            return ratings;
        }
    }
}
