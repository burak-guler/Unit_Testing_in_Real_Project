﻿using Microsoft.AspNetCore.Mvc;
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
                new Product { Id = 2, Name = "Silgi", Stock = 50, Price = 100, Color = "Mavi" },
                new Product { Id = 3, Name = "Kulaklık", Stock = 50, Price = 100, Color = "Siyah" }
            };
        }

        //Index methodunun testleri.

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnProductList()
        {
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(products);

            var result = await _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            var productList = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);

            Assert.Equal(3, productList.Count());
        }

        //Details methodunun testleri

        //id parametresinin null olması

        [Fact]
        public async void Details_IdIsNull_ReturnRedirectToIndexAction()
        {
            var result = await _controller.Details(null);

            var redirect = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirect.ActionName);

        }

        //product versinin null dönmesi
        [Fact]
        public async void Details_IdInValid_ReturnNotFound()
        {
            Product product = null;

            _mockRepo.Setup(x=>x.GetById(0)).ReturnsAsync(product);

            var result = await _controller.Details(0);

            var redirect = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, redirect.StatusCode);
        }

        //product modelinin sayfaya gönderilmesinin testi.

        [Theory]
        [InlineData(1)]
        public async void Details_ValidId_ReturnProduct(int productId)
        {
            Product product = products.First(x => x.Id == 1);

            _mockRepo.Setup(x => x.GetById(productId)).ReturnsAsync(product);

            var result = await _controller.Details(productId);

            var viewResult = Assert.IsType<ViewResult>(result);

            var resultProduct = Assert.IsAssignableFrom<Product>(viewResult.Model); 
            
            Assert.Equal(productId, resultProduct.Id);  
            Assert.Equal(product.Name, resultProduct.Name);  

        }

        //CREATE methodunun testleri

        //Get Methodu testi

        [Fact]
        public void Create_ActionExecutes_ReturnView()
        {
            var result = _controller.Create();

            Assert.IsType<ViewResult>(result);
        }

        //Post methodu testleri

        [Fact]
        public async void CreatePost_InValidModelState_ReturnView()
        {
            _controller.ModelState.AddModelError("Name", "Name alanı gereklidir.");

            var result = await _controller.Create(products.FirstOrDefault());

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsType<Product>(viewResult.Model);
        }

        [Fact]
        public async void CreatePost_ValidModelState_ReturnRedirectToIndexAction()
        {
            var result = await _controller.Create(products.First());

            var redirect = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirect.ActionName);
        }

        //repodaki create methodunun çalışıp çalışmadığının testi 

        [Fact] 
        public async void CreatePost_ValidModelState_CreateMethodExecute()
        {
            Product newProduct = null;

            _mockRepo.Setup(x => x.Create(It.IsAny<Product>())).Callback<Product>(x => newProduct = x);

            var result = await _controller.Create(products.First());

            _mockRepo.Verify(x => x.Create(It.IsAny<Product>()), Times.Once);

            Assert.Equal(products.First().Id, newProduct.Id);
        }

        //repodaki create methodunun çalışmadığı testi 

        [Fact]
        public async void CreatePost_InValidModelState_NeverExecute()
        {
            _controller.ModelState.AddModelError("Name", "");

            var result = await _controller.Create(products.First());

            _mockRepo.Verify(x => x.Create(It.IsAny<Product>()), Times.Never);
        }

        //EDİT methodu testleri

        [Fact]
        public async void Edit_IdIsNull_ReturnRedirectToIndexAction()
        {
            var result = await _controller.Edit(null);

            var redirect = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(0)]
        public async void Edit_IdInValid_ReturnNotFound(int productId)
        {
            Product product = null;

            _mockRepo.Setup(x => x.GetById(productId)).ReturnsAsync(product);

            var result = await _controller.Edit(productId); 

            var redirect = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, redirect.StatusCode);
        }

        [Theory]
        [InlineData(2)]
        public async void Edit_ActionExecutes_ReturnProduct(int productId)
        {
           

            _mockRepo.Setup(x => x.GetById(productId)).ReturnsAsync(products.First(x=>x.Id==productId));

            var result = await _controller.Edit(productId);

            var viewResult = Assert.IsType<ViewResult>(result);

            var product = Assert.IsAssignableFrom<Product>(viewResult.Model);

            Assert.Equal(products.First(x => x.Id == productId).Name, product.Name);
           
        }

        [Theory]
        [InlineData(1)]
        public void EditPost_IdIsNotEqualProduct_ReturnNotFound(int productId)
        {
            var result = _controller.Edit(2,products.First(x=>x.Id==productId));   

            var redirect = Assert.IsType<NotFoundResult>(result);   
        }

        [Theory]
        [InlineData(1)]
        public void EditPost_InValidModelState_ReturnView(int productId)
        {
            _controller.ModelState.AddModelError("Name", "");

            var result = _controller.Edit(productId,products.First(x=>x.Id==productId));

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsType<Product>(viewResult.Model);
        }

        [Theory]
        [InlineData(1)]
        public void EdirPost_ValidModelState_ReturnRedirectToIndexAction(int productId)
        {
            var result = _controller.Edit(productId, products.First(x => x.Id == productId));

            var redirect = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(1)]
        public void EditPost_ValidModelState_UpdateMethodExecute(int productId)
        {
            var product = products.First(x => x.Id == productId);

            _mockRepo.Setup(x => x.Update(product));

            _controller.Edit(productId, product);

            _mockRepo.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async void Delete_IdIsNull_ReturnNotFound()
        {
            var result = await _controller.Delete(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(0)]
        public async void Delete_IdIsNotEqualProduct_ReturnNotFound(int productId)
        {
            Product product = null;

            _mockRepo.Setup(x => x.GetById(productId)).ReturnsAsync(product);

            var result = await _controller.Delete(productId);   

            Assert.IsType<NotFoundResult>(result);  
        }

        [Theory]
        [InlineData(1)]
        public async void Delete_ActionExecutes_ReturnProduct(int productId)
        {
            var product = products.First(x=>x.Id==productId);

            _mockRepo.Setup(x=>x.GetById(productId)).ReturnsAsync(product);

            var result = await _controller.Delete(productId); 

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Product>(viewResult.Model);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteConfirmed_ActionExecutes_ReturnRedirectToIndexAction(int productId)
        {
            var result = await _controller.DeleteConfirmed(productId);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteConfirmed_ActionExecutes_DeleteMethodExecute(int productId)
        {
            var product = products.First(x=>x.Id==productId);

            _mockRepo.Setup(x => x.Delete(product));

            await _controller.DeleteConfirmed(productId);

            _mockRepo.Verify(x => x.Delete(It.IsAny<Product>()), Times.Once);
        }
    }
}
