using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using UdemyRealWorldUnitTest.Web.Models;
using UdemyRealWorldUnitTest.Web.Repository;

namespace UdemyRealWorldUnitTest.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly  IRepository<Product> _repository;
        IMemoryCache _memoryCache;

        public ProductsController(IRepository<Product> repository,IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }
       

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _repository.GetAll();

            _memoryCache.Set("ProductList",products);
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var  productList = _memoryCache.Get<List<Product>>("ProductList");
            var productCache=  productList.Where(x=>x.Id==id).FirstOrDefault();

            var cacheIsSelected = _memoryCache.TryGetValue("ProductList", out List<Product> value);
            if (_memoryCache.TryGetValue("ProductList", out List<Product> data))
            {
                var cacheData = data;
            }

            var product = await _repository.GetById((int)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Stock,Color")] Product product)
        {
            if (ModelState.IsValid)
            {
                
                await _repository.Create(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var product = await _repository.GetById((int)id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Price,Stock,Color")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                _repository.Update(product);
                
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.GetById((int)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _repository.GetById((int)id);

            _repository.Delete(product);    

            return RedirectToAction((nameof(Index)));
        }

        private bool ProductExists(int id)
        {
          var product = _repository.GetById(id);
            if (product==null)
            {
                return false;
            }
            else
                return true;
        }
    }
}
