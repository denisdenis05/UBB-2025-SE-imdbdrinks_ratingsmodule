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
    class RatingServiceUpdateRatingTest
    {
        private RatingService _service;
        private Mock<IRatingRepository> _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IRatingRepository>();
            _service = new RatingService(_repository.Object);
        }

        [Test]
        public void UpdateRating_WhenValidRating_CallsRepositoryUpdate()
        {
            var rating = new Rating { ProductId = 1, UserId = 1, RatingValue = 4 };

            _repository.Setup(repository => repository.UpdateRating(It.IsAny<Rating>()))
                .Returns<Rating>(rating => rating);

            _service.UpdateRating(rating);

            _repository.Verify(repository => repository.UpdateRating(It.IsAny<Rating>()), Times.Once);
        }

        [Test]
        public void UpdateRating_WhenValidRating_ReturnsUpdatedRating()
        {
            var input = new Rating { ProductId = 1, UserId = 1, RatingValue = 4 };

            _repository.Setup(repository => repository.UpdateRating(It.IsAny<Rating>()))
                .Returns<Rating>(rating => rating);

            var result = _service.UpdateRating(input);

            Assert.That(result, Is.EqualTo(input));
        }

        [Test]
        public void UpdateRating_WhenInvalidRating_ThrowsArgumentException()
        {
            var invalidRating = new Rating { ProductId = 1, UserId = 1, RatingValue = 8 }; // Invalid

            var exception = Assert.Throws<ArgumentException>(() => _service.UpdateRating(invalidRating));

            Assert.That(exception.Message, Is.EqualTo("Invalid rating value."));
        }

    }
}
