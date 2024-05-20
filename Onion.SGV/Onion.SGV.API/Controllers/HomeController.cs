using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;
using Onion.SGV.API.Data;
using Onion.SGV.API.Models;
using Onion.SGV.API.Services;
using Onion.SGV.API.Services.Interfaces;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Onion.SGV.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public HomeController(IClientService clientService, IProductService productService, IOrderService orderService)
        {
            _clientService = clientService;
            _productService = productService;
            _orderService = orderService;
        }

        // POST api/<HomeController>
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            try
            {
                List<Client> clientList = new List<Client>();
                List<Order > orderList = new List<Order>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                if (file == null || file.Length == 0)
                    return BadRequest("Nenhum arquivo enviado.");
                
                using(var stream =  new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorkbook workbook = package.Workbook;
                        if (workbook != null && workbook.Worksheets.Count > 0)
                        {
                            ExcelWorksheet worksheet = workbook.Worksheets[0];
                            int rowCount = worksheet.Dimension.Rows;

                            for (int row = 2; row <= rowCount; row++)
                            {
                                if (worksheet.Cells[row, 1].Value == null || string.IsNullOrEmpty(worksheet.Cells[row, 1].Value.ToString()))
                                {
                                    break;
                                }
                                Client client = new Client();
                                Product product = new Product();
                                Order order = new Order();
                                
                                //Client
                                client.Document = worksheet.Cells[row, 1].Value?.ToString().Replace(".", "").Replace("-", ""); // Coluna 1
                                client.SocialName = worksheet.Cells[row, 2].Value?.ToString(); // Coluna 2 (supondo inteiros)
                                client.Cep = worksheet.Cells[row, 3].Value?.ToString().Replace(".", "").Replace("-","");
                                
                                //Product
                                string? productName = worksheet.Cells[row, 4].Value?.ToString();
                                product = await _productService.Get(productName); 
                                
                                //Order
                                order.ProductId = product.Id;
                                order.Product = product;
                                order.Id = Convert.ToInt32(worksheet.Cells[row, 5].Value);
                                order.OrderDate = (DateTime)Convert.ToDateTime(worksheet.Cells[row, 6].GetValue<DateTime>());
                                order.ProductId = product.Id;
                                order.ClientId = client.Document;
                                
                                orderList.Add(order);
                                clientList.Add(client);
                            }
                            foreach (var item in clientList)
                            {
                                _clientService.Add(item);
                            }

                            foreach (var item in orderList)
                            {
                                _orderService.Add(item);
                            }

                            return Ok();
                        }
                        else
                        {
                            return BadRequest("O arquivo não contém nenhuma planilha.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao processar o arquivo: {ex.Message}");
            }
        }
    }
}
