using imdbdrinks_ratingsmodule.Constants.ErrorMessages;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imdbdrinks_ratingsmodule.Test
{
    public class ReviewServiceAddReviewTest
    {
        private ReviewService _service;
        private Mock<IReviewRepository> _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IReviewRepository>();
            _service = new ReviewService(_repository.Object);
        }

        [Test]
        public void AddReview_WhenValid_SetsActiveAndSaves()
        {
            var review = new Review
            {
                RatingId = 1,
                UserId = 1,
                Content = "Awesome product!"
            };

            _repository.Setup(r => r.AddOrUpdateReview(It.IsAny<Review>())).Returns<Review>(r => r);

            var result = _service.AddReview(review);

            Assert.That(result.IsActive, Is.True);
        }

        [Test]
        public void AddReview_WhenContentTooLong_ThrowsArgumentException()
        {
            var timesToRepeat = 600;
            var review = new Review
            {
                Content = new string('a', timesToRepeat) // Invalid
            };

            var ex = Assert.Throws<ArgumentException>(() => _service.AddReview(review));
            Assert.That(ex.Message, Is.EqualTo(ReviewServiceErrorMessages.InvalidReview));
        }

        [Test]
        public void AddReview_WhenEmptyContent_ThrowsArgumentException()
        {
            var review = new Review
            {
                Content = "   " // Invalid
            };

            var ex = Assert.Throws<ArgumentException>(() => _service.AddReview(review));
            Assert.That(ex.Message, Is.EqualTo(ReviewServiceErrorMessages.InvalidReview));
        }
    }
}
