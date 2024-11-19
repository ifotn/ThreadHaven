using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadHaven.Controllers;
using ThreadHaven.Data;
using ThreadHaven.Models;

namespace ThreadHavenTesting
{
    [TestClass]
    public class CategoriesControllerTests
    {
        // create in-memory db to replace SQL Server for testing
        private ApplicationDbContext _context;
        //List<Category> categories = new List<Category>();
        CategoriesController controller;

        // startup method to run automatically before each test
        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            // populate categories into mock db
            _context.Categories.Add(new Category { CategoryId = 46, Name = "First Category" });
            _context.Categories.Add(new Category { CategoryId = 73, Name = "Second Category" });
            _context.Categories.Add(new Category { CategoryId = 88, Name = "Third Category" });
            _context.SaveChanges();

            // create controller and pass it mock db
            controller = new CategoriesController(_context);
        }

        #region "Index"
        [TestMethod]
        public void IndexReturnsView()
        {
            // arrange not needed => runs first in TestInitialize()

            // act
            var result = (ViewResult)controller.Index().Result;

            // assert
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void IndexReturnsCategories()
        {
            // arrange not needed => runs first in TestInitialize()

            // act
            var result = (ViewResult)controller.Index().Result;
            var model = (List<Category>)result.Model;

            // assert
            CollectionAssert.AreEqual(_context.Categories.ToList(), model);
        }
        #endregion

        #region "Details"
        [TestMethod]
        public void DetailsNoIdReturnsNotFound()
        {
            // arrange (not needed)

            // act
            var result = (ViewResult)controller.Details(null).Result;

            // assert
            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void DetailsInvalidIdReturnsNotFound()
        {
            // arrange (not needed)

            // act - pass in an id NOT in our mock db above
            var result = (ViewResult)controller.Details(-1).Result;

            // assert
            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void DetailsValidIdReturnsView()
        {
            // act - pass in valid id from mock db
            var result = (ViewResult)controller.Details(46).Result;

            // assert - do we get the Details view?
            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod]
        public void DetailsValidIdReturnsCategory()
        {
            // act - pass in valid id from mock db
            var result = (ViewResult)controller.Details(46).Result;

            // assert - do we get the correct category?
            Assert.AreEqual(_context.Categories.Find(46), result.Model);
        }
        #endregion
    }
}
