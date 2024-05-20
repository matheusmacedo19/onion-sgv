using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Onion.SGV.API.Utils;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;

namespace Onion.SGV.API.Models
{
    public class OrderDashboard
    {
        [MaxLength(14)]
        public string Document {  get; set; }
        public string Socialname { get; set; }
        [MaxLength(8)]
        public string Cep { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public async Task<double> RetrieveTaxDelivery(string cep, double price)
        {
            try
            {
                Location location = await GetLocation(cep);
                string region = await GetRegion(location.Ibge);
                double taxDelivery = 0;

                if (location.Uf == "SP")
                {
                    return price + 0;
                }

                switch (region)
                {
                    case "Norte":
                    case "Nordeste":
                        taxDelivery = price + (price * 0.3);
                        break;
                    case "Centro-Oeste":
                    case "Sul":
                        taxDelivery = price + (price * 0.2);
                        break;
                    case "Sudeste":
                        taxDelivery = price + (price * 0.1);
                        break;
                    default:
                        taxDelivery = 0;
                        break;
                }

                return taxDelivery;
            }
            catch (Exception ex)
            {
                var e = new Exception(string.Format($"Ocorreu um erro ao calcular a taxa de entrega: {ex.Message}"));
                return 0;
            }
        }
        public async Task<DateTime> EstimateDeliveryDate(string cep, DateTime orderDate)
        {
            try
            {
                Location location = await GetLocation(cep);
                string region = await GetRegion(location.Ibge);

                DateTime deliveryDate = default;

                switch (region)
                {
                    case "Norte":
                    case "Nordeste":
                        deliveryDate = BusinessDays.AddBusinessDays(orderDate, 10);
                        break;
                    case "Centro-Oeste":
                    case "Sul":
                        deliveryDate = BusinessDays.AddBusinessDays(orderDate, 5);
                        break;
                    case "Sudeste":
                        deliveryDate = BusinessDays.AddBusinessDays(orderDate, 1);
                        break;
                }

                return deliveryDate;

            }
            catch (HttpRequestException ex)
            {
                var e = new Exception(string.Format($"Erro na solicitação HTTP: {ex.Message}"));
                throw;
            }
            catch (JsonException ex)
            {
                var e = new Exception(string.Format($"Erro na desserialização JSON: {ex.Message}"));
                throw;
            }
            catch (Exception ex)
            {
                var e = new Exception(string.Format($"Erro inesperado: {ex.Message}"));
                throw;
            }
        }

        private static async Task<Location> GetLocation(string cep)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://viacep.com.br/ws/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string url = $"{cep}/json/";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        Location? content = await response.Content.ReadFromJsonAsync<Location>();
                        if (content != null)
                            return content;
                    }
                    return new Location();
                }
            }
            catch (HttpRequestException ex)
            {
                
                var e = new Exception(string.Format($"Erro na solicitação HTTP: {ex.Message}"));
                throw;
            }
            catch (JsonException ex)
            {
                
                var e = new Exception(string.Format($"Erro na desserialização JSON: {ex.Message}"));
                throw;
            }
            catch (Exception ex)
            {
                var e = new Exception(string.Format($"Erro inesperado: {ex.Message}"));
                throw;
            }
        }

        private static async Task<string> GetRegion(string id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://servicodados.ibge.gov.br");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string url = $"/api/v1/localidades/municipios/{id}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadFromJsonAsync<Ibge>();
                        if (content != null)
                        {
                            return content.Microrregiao.Mesorregiao.Uf.Regiao.Nome;
                        }
                    }
                    return string.Empty;
                }
            }
            catch (HttpRequestException ex)
            {
                var e = new Exception(string.Format($"Erro na solicitação HTTP: {ex.Message}"));
                throw;
            }
            catch (JsonException ex)
            {
                var e = new Exception(string.Format($"Erro na desserialização JSON: {ex.Message}"));
                throw;
            }
            catch (Exception ex)
            {
                var e = new Exception(string.Format($"Erro inesperado: {ex.Message}"));
                throw;
            }
        }

    }
}
