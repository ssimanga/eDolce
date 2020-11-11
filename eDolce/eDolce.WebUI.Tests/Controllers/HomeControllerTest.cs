using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eDolce.WebUI;
using eDolce.WebUI.Controllers;
using eDolce.Core.Contracts;
using eDolce.Core.Models;
using eDolce.WebUI.Tests.Mocks;
using eDolce.Core.ViewModels;

namespace eDolce.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            //// Arrange
            //HomeController controller = new HomeController();

            //// Act
            //ViewResult result = controller.Index() as ViewResult;

            //// Assert
            //Assert.IsNotNull(result);
        }
        [TestMethod]
        public void DoesItReturnProducts()
        {
            IRepository<Product> productContext = new Mocks.MockContext<Product>();
            IRepository<ProductCategory> productCategoryContext = new Mocks.MockContext<ProductCategory>();

            productContext.Insert(new Product());
            HomeController controller = new HomeController(productContext, productCategoryContext);

            var result = controller.Index() as ViewResult;
            var viewModel = (ProductListingViewModel)result.ViewData.Model;

            Assert.AreEqual(1, viewModel.Products.Count());
        }

       
    }
}
