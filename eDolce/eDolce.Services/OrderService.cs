using eDolce.Core.Contracts;
using eDolce.Core.Models;
using eDolce.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDolce.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> orderContext;
        public OrderService(IRepository<Order> orderContext)
        {
            this.orderContext = orderContext;
        }
        public void CreateOrder(Order baseOrder, List<CartItemViewModel> cartItems)
        {
           foreach(var item in cartItems)
            {
                baseOrder.OrderItems.Add(new OrderItem()
                {
                    ProductId = item.Id,
                    Image = item.Image,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                });
            }
            orderContext.Insert(baseOrder);
            orderContext.Commit();
        }
    }
}
