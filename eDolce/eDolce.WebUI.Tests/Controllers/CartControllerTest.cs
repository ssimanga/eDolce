using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using eDolce.Core.Contracts;
using eDolce.Core.Models;
using eDolce.Core.ViewModels;
using eDolce.Services;
using eDolce.WebUI.Controllers;
using eDolce.WebUI.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eDolce.WebUI.Tests.Controllers
{
    [TestClass]
    public class CartControllerTest
    {
        [TestMethod]
        public void CanAddCartItems()
        {
            IRepository<Cart> carts = new MockContext<Cart>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();

            var httpContext = new MockHttpContext();

            ICartService cartService = new CartService(products, carts);
            IOrderService orderService = new OrderService(orders);

            var controller = new CartController(cartService, orderService,customers);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            controller.addToCart("i");

            Cart cart = carts.Collection().FirstOrDefault();

            Assert.IsNotNull(cart);
            Assert.AreEqual(1, cart.CartItems.Count);
            Assert.AreEqual("i", cart.CartItems.ToList().FirstOrDefault().ProductId);
        }
        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            IRepository<Cart> carts = new MockContext<Cart>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();

            products.Insert(new Product { Id = "1", Price = 10.00m });
            products.Insert(new Product { Id = "2", Price = 5.00m });

            Cart cart = new Cart();
            cart.CartItems.Add(new CartItem { ProductId = "1", Quantity = 2 });
            cart.CartItems.Add(new CartItem { ProductId = "2", Quantity = 1 });

            carts.Insert(cart);

            ICartService cartService = new CartService(products, carts);
            IOrderService orderService = new OrderService(orders);
            var controller = new CartController(cartService, orderService, customers);
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceCart") { Value = cart.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.CartSummary() as PartialViewResult;
            var cartSummary = (CartSummaryViewModel)result.ViewData.Model;

            Assert.AreEqual(3, cartSummary.CartCount);
            Assert.AreEqual(25.00m, cartSummary.CartTotal);

        }
        [TestMethod]
        public void CanCheckoutAndCreateOrder()
        {
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Customer> customers = new MockContext<Customer>();
            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            IRepository<Cart> carts = new MockContext<Cart>();
            Cart cart = new Cart();
            cart.CartItems.Add(new CartItem() { ProductId = "1", Quantity = 2, CartId = cart.Id });
            cart.CartItems.Add(new CartItem() { ProductId = "2", Quantity = 2, CartId = cart.Id });

            carts.Insert(cart);

            ICartService cartService = new CartService(products, carts);

            IRepository<Order> orders = new MockContext<Order>();
            IOrderService orderService = new OrderService(orders);

            customers.Insert(new Customer() { Id = "1", Email = "simangae@gmail.com", ZipCode = "9300" });
            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("simangae@gmail.com", "Forms"), null);

            var controller = new CartController(cartService, orderService, customers);
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceCart") { Value = cart.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            Order order = new Order();
            controller.Checkout(order);

            Assert.AreEqual(2, order.OrderItems.Count);
            Assert.AreEqual(0, cart.CartItems.Count);

            Order OrderInRep = orders.Find(order.Id);
            Assert.AreEqual(2, OrderInRep.OrderItems.Count);

        }
    }
   }

