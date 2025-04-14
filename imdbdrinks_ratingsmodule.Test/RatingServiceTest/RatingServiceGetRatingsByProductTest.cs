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
    public class RatingServiceGetRatingsByProductTest
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
        public void GetRatingsByProduct_WhenRatingsExist_ReturnsCorrectCount()
        {
            var ratings = new List<Rating> { new Rating(), new Rating() };
            _repository.Setup(r => r.FindByProductId(1)).Returns(ratings);

            var result = _service.GetRatingsByProduct(1);

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetRatingsByProduct_WhenNoRatingsExist_ReturnsEmptyCollection()
        {
            _repository.Setup(r => r.FindByProductId(1)).Returns(new List<Rating>());

            var result = _service.GetRatingsByProduct(1);

            Assert.That(result, Is.Empty);
        }
    }
}
