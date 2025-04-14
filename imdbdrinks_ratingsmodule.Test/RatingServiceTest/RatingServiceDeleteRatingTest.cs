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
    public class RatingServiceDeleteRatingTest
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
        public void DeleteRating_WhenCalled_InvokesRepositoryDeleteOnce()
        {
            var ratingId = 42;
            _service.DeleteRatingById(ratingId);

            _repository.Verify(repository => repository.DeleteRatingById(ratingId), Times.Once);
        }
    }
}
