using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyRealWorldUnitTest.Web.Controllers;
using UdemyRealWorldUnitTest.Web.Models;
using UdemyRealWorldUnitTest.Web.Repository;

namespace UdemyRealWorldUnitTest.Test
{
    public class ProductControllerTest
    {
        private readonly Mock<IRepository<Product>> _mockRepo;
        private readonly ProductsController _controller;
        private List<Product> products;

        public ProductControllerTest()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _controller = new ProductsController(_mockRepo.Object);

            products = new List<Product>()
            {
                new Product { Id = 1, Name = "Kalem", Stock = 50, Price = 100, Color = "Kırmızı" },
                new Product { Id = 1, Name = "Silgi", Stock = 50, Price = 100, Color = "Mavi" },
                new Product { Id = 1, Name = "Kulaklık", Stock = 50, Price = 100, Color = "Siyah" }
            };
        }



    }
}
