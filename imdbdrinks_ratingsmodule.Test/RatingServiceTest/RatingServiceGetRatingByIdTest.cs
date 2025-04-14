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
            var expected = new Rating { RatingId = 1 };
            _repository.Setup(r => r.FindById(1)).Returns(expected);

            var result = _service.GetRatingById(1);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetRatingById_WhenRatingDoesNotExist_ReturnsNull()
        {
            _repository.Setup(r => r.FindById(It.IsAny<int>())).Returns((Rating)null);

            var result = _service.GetRatingById(999);

            Assert.That(result, Is.Null);
        }
    }
}
