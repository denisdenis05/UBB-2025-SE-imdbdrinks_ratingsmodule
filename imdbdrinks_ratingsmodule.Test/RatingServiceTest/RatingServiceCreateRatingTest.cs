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
    public class RatingServiceCreateRatingTest
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
        public void CreateRating_WhenValidRating_SetsRatingAsActive()
        {
            var rating = new Rating { ProductId = 1, UserId = 1, RatingValue = 4 };

            _repository.Setup(repository => repository.AddRating(It.IsAny<Rating>()))
                .Returns<Rating>(rating => rating);

            var result = _service.CreateRating(rating);

            Assert.That(result.IsActive, Is.True);
        }

        [Test]
        public void CreateRating_WhenValidRating_SetsRatingDateToNow()
        {
            var rating = new Rating { ProductId = 1, UserId = 1, RatingValue = 4 };

            _repository.Setup(repository => repository.AddRating(It.IsAny<Rating>()))
                .Returns<Rating>(rating => rating);

            var result = _service.CreateRating(rating);

            Assert.That(result.RatingDate, Is.Not.EqualTo(default(DateTime)));
        }

        [Test]
        public void CreateRating_WhenValidRating_CallsSaveOnRepository()
        {
            var rating = new Rating { ProductId = 1, UserId = 1, RatingValue = 4 };

            _repository.Setup(repository => repository.AddRating(It.IsAny<Rating>()))
                .Returns<Rating>(rating => rating);

            _service.CreateRating(rating);

            _repository.Verify(repository => repository.AddRating(It.IsAny<Rating>()), Times.Once);
        }

        [Test]
        public void CreateRating_WhenInvalidRating_ThrowsArgumentException()
        {
            var rating = new Rating { ProductId = 1, UserId = 1, RatingValue = 7 }; // invalid

            var exception = Assert.Throws<ArgumentException>(() => _service.CreateRating(rating));

            Assert.That(exception.Message, Is.EqualTo("Invalid rating value."));
        }
    }
}
