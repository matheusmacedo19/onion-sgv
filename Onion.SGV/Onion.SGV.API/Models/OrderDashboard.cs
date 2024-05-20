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
                Console.WriteLine($"Ocorreu um erro ao calcular a taxa de entrega: {ex.Message}");
                return 0;
            }
        }
        public async Task<DateTime> EstimateDeliveryDate(string cep, DateTime orderDate)
        {
            try
            {
                Location location =  await GetLocation(cep);
                Task<string> region = GetRegion(location.Ibge);
                DateTime deliveryDate = new DateTime();
                switch (region.Result)
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

            }catch (Exception ex)
            {
                throw;
            }
        }

        private static async Task<Location> GetLocation(string cep)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://viacep.com.br/ws/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string url = cep + "/json/";
                HttpResponseMessage response = await client.GetAsync(url);
                Task<string> jsonString;
                if (response.IsSuccessStatusCode)
                {
                    jsonString = response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Location>(jsonString.Result);
                }
                return null;

            }catch (Exception ex)
            {
                throw;
            }
        }
        
        private static async Task<string> GetRegion(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://servicodados.ibge.gov.br");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Regiao? region = new Regiao();
                string url = "/api/v1/localidades/municipios/" + id;
                HttpResponseMessage response = await client.GetAsync(url);
                Task<string> jsonString;
                if (response.IsSuccessStatusCode)
                {
                    jsonString = response.Content.ReadAsStringAsync();
                    var parsedObject = JObject.Parse(jsonString.Result);
                    var regionJson = parsedObject["microrregiao"]["mesorregiao"]["UF"]["regiao"].ToString();
                    region = JsonConvert.DeserializeObject<Regiao>(regionJson);
                }
                if(region != null )
                {
                    return region.Nome;
                }else
                {
                    return string.Empty;
                }
            }catch (Exception ex) 
            { 
                throw;
            }

        }
        
    }
}
