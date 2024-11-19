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
        List<Category> categories = new List<Category>();
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
    }
}
