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
    public class ReviewServiceDeleteReviewTest
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
        public void DeleteReviewById_WhenCalled_InvokesRepositoryDelete()
        {
            _service.DeleteReviewById(123);

            _repository.Verify(r => r.DeleteReviewById(123), Times.Once);
        }
    }
}
