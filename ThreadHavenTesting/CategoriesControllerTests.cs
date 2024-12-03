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

        #region "Create"
        // model stat validation fails
        [TestMethod]
        public async Task CreatePost_ReturnsCreateView()
        {
            // Arrange
            controller.ModelState.AddModelError("Name", "Name is required");
            // Act
            var result = await controller.Create(new Category());
            // Assert
            Assert.AreEqual("Create", ((ViewResult)result).ViewName);
        }
        // test for reditection when valid
        [TestMethod]
        public async Task CreatePost_RedirectsToIndex()
        {
            // Arrange
            var newCategory = new Category { CategoryId = 4, Name = "Fourth Category" };
            // Act
            var result = await controller.Create(newCategory);
            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
        [TestMethod]
        public async Task CreatePost_RedirectsToIndexAction() { 
// Arrange
var newCategory = new Category { CategoryId = 4, Name = "Fourth Category" };
        // Act
        var result = await controller.Create(newCategory);
        // Assert
        var redirectToActionResult = (RedirectToActionResult)result;
        Assert.AreEqual("Index", redirectToActionResult.ActionName);
}

                        //test for catagory id
                        [TestMethod]
        public async Task CreatePost_IncreasesCategoryID()
        {
            // Arrange
            var newCategory = new Category { CategoryId = 4, Name = "Fourth Category" };
            // Act
            await controller.Create(newCategory);
            // Assert
            Assert.AreEqual(4, _context.Categories.Count());
        }
        #endregion

        #region "DeleteForGet"

        [TestMethod]
        public void DeleteNoIdReturnsNotFound()
        {
            // arrange (not needed)

            // act
            var result = (ViewResult)controller.Delete(null).Result;

            // assert
            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void DeleteInvalidIdReturnsNotFound()
        {
            // arrange (not needed)

            // act - pass in an id NOT in our mock db above
            var result = (ViewResult)controller.Delete(-1).Result;

            // assert
            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void DeleteValidIdReturnsView()
        {
            // act - pass in valid id from mock db
            var result = (ViewResult)controller.Delete(46).Result;

            // assert - do we get the Details view?
            Assert.AreEqual("Delete", result.ViewName);
        }

        #endregion

        #region "DeleteForPost"


        [TestMethod]
        public void DeleteConfirmedNoIdReturnsNotFound()
        {
            // arrange (not needed)

            // act
            var result = (ViewResult)controller.DeleteConfirmed(null).Result;

            // assert
            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void DeleteConfirmedInvalidIdReturnsNotFound()
        {
            // arrange (not needed)

            // act - pass in an id NOT in our mock db above
            var result = (ViewResult)controller.DeleteConfirmed(-1).Result;

            // assert
            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void DeleteConfirmedValidIdReturnsView()
        {
            // act - pass in valid id from mock db
            var result = (RedirectToActionResult)controller.DeleteConfirmed(46).Result;

            // assert - do we get the Details view?
            Assert.AreEqual("Index", result.ActionName);
        }

        #endregion

        #region "Create/Get"
        [TestMethod]
        public void CreateReturnView()
        {
            // arrange (not needed)

            //act
            var result = (ViewResult)controller.Index().Result;


            // assert
            Assert.AreEqual("Create", result.ViewName);
        }



        #endregion

        #region "Edit/Get"
        [TestMethod]
        public void EditNoIdReturnsNotFound()
        {
            // arrange (not needed)

            //act
            var result = (ViewResult)controller.Edit(-1).Result;


            // assert
            Assert.AreEqual("404", result.ViewName);
        }
        [TestMethod]
        public void EditValidIdReturnsView()
        {
            // arrange (not needed)

            //act - pass in a id Not in our mock db above
            var result = (ViewResult)controller.Edit(null).Result;


            // assert
            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void EditCategoryNullReturnsNotFound()
        {
            // arrange (not needed)

            //act - pass in a id Not in our mock db above
            var result = (ViewResult)controller.Edit(-1).Result;


            // assert
            Assert.AreEqual("404", result.ViewName);
        }
        [TestMethod]
        public void EditValidIdReturnsCategory()
        {
            //act - pass in valid id from mock db
            var result = (ViewResult)controller.Edit(46).Result;


            //assert
            Assert.AreEqual("Edit", result.ViewName);
        }


        #endregion

    }
}
