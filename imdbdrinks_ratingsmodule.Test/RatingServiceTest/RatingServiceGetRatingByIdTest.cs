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
    public class RatingServiceGetRatingByIdTest
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
        public void GetRatingById_WhenRatingExists_ReturnsRating()
        {
            var ratingId = 1;
            var expected = new Rating { RatingId = ratingId };
            _repository.Setup(r => r.GetRatingById(ratingId)).Returns(expected);

            var result = _service.GetRatingById(ratingId);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetRatingById_WhenRatingDoesNotExist_ReturnsNull()
        {
            var ratingId = 999;
            _repository.Setup(r => r.GetRatingById(It.IsAny<int>())).Returns((Rating)null);

            var result = _service.GetRatingById(ratingId);

            Assert.That(result, Is.Null);
        }
    }
}
