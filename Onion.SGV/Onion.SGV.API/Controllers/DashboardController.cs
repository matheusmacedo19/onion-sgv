using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onion.SGV.API.Data;
using Onion.SGV.API.Models;
using Onion.SGV.API.Services;
using Onion.SGV.API.Services.Interfaces;

namespace Onion.SGV.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public DashboardController(IClientService clientService, IProductService productService, IOrderService orderService)
        {
            _clientService = clientService;
            _productService = productService;
            _orderService = orderService;
        }
        // GET: api/<DashboardController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<OrderDashboard> orderDashboardList = new List<OrderDashboard>();
                List<Order> orderList = _orderService.GetAll();
                foreach (Order order in orderList)
                {
                    OrderDashboard orderDashboard = new OrderDashboard();
                    orderDashboard.Document = order.Client.Document;
                    orderDashboard.Socialname = order.Client.SocialName;
                    orderDashboard.Cep = order.Client.Cep;
                    orderDashboard.ProductName = order.Product.Name;
                    orderDashboard.OrderId = order.Id;
                    orderDashboard.OrderDate = order.OrderDate;
                    orderDashboardList.Add(orderDashboard);
                }
                return Ok(orderDashboardList);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<DashboardController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Order order = _orderService.Get(id);
            OrderDashboard orderDashboard = new OrderDashboard();
            orderDashboard.Document = order.Client.Document;
            orderDashboard.Socialname = order.Client.SocialName;
            orderDashboard.Cep = order.Client.Cep;
            orderDashboard.ProductName = order.Product.Name;
            orderDashboard.OrderId = order.Id;
            orderDashboard.OrderDate = order.OrderDate;
            
            return Ok(orderDashboard);
        }

    }
}
