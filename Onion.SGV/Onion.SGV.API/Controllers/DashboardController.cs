using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onion.SGV.API.Data;
using Onion.SGV.API.Models;
using Onion.SGV.API.Services;
using Onion.SGV.API.Services.Interfaces;
using System.Net;

namespace Onion.SGV.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public DashboardController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // GET: api/<DashboardController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<OrderDashboard> orderDashboardList = new List<OrderDashboard>();
                IEnumerable<Order> orderList = await _orderService.GetAll();
                foreach (Order order in orderList)
                {
                    OrderDashboard orderDashboard = new OrderDashboard();
                    orderDashboard.Document = order.Client?.Document;
                    orderDashboard.Socialname = order.Client?.SocialName;
                    orderDashboard.Cep = order.Client?.Cep;
                    orderDashboard.ProductName = order.Product?.Name;
                    orderDashboard.OrderId = order.Id;
                    orderDashboard.OrderDate = orderDashboard.EstimateDeliveryDate(order.Client?.Cep, order.OrderDate).Result;
                    orderDashboard.ProductId = order.ProductId.Value;
                    orderDashboard.ProductPrice = orderDashboard.RetrieveTaxDelivery(order.Client?.Cep, order.Product.Price).Result;
                    orderDashboardList.Add(orderDashboard);
                }
                return Ok(orderDashboardList);

            }catch(Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao buscar lista de pedidos: {ex.Message}");
            }
        }

        // GET api/<DashboardController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Order order = await _orderService.Get(id);
                OrderDashboard orderDashboard = new OrderDashboard();
                orderDashboard.Document = order.Client?.Document;
                orderDashboard.Socialname = order.Client?.SocialName;
                orderDashboard.Cep = order.Client?.Cep;
                orderDashboard.ProductName = order.Product?.Name;
                orderDashboard.OrderId = order.Id;
                orderDashboard.OrderDate = order.OrderDate;
            
                return Ok(orderDashboard);

            }catch(Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao buscar item: {ex.Message}");
            }
        }

    }
}
