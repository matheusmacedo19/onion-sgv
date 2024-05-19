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

        public async Task<double> RetrieveTaxDelivery(long cep)
        {
            Location location = await GetLocation(cep);
            Task<string> region = GetRegion(location.Ibge);
            double taxDelivery = 0;
            
            if(location.Uf == "SP")
            {
                return 0;
            }
            
            switch (region.Result)
            {
                case "Norte": case "Nordeste":
                     taxDelivery = ProductPrice * 30/100;
                    break;
                case "Centro-Oeste": case "Sul":
                    taxDelivery = ProductPrice * 20/100;
                    break;
                case "Sudeste":
                    taxDelivery = ProductPrice * 10 / 100;
                    break;
            }
            return taxDelivery;
        }
        public async Task<DateTime> EstimateDeliveryDate(long cep)
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
                        deliveryDate = BusinessDays.AddBusinessDays(OrderDate, 10);
                        break;
                    case "Centro-Oeste":
                    case "Sul":
                        deliveryDate = BusinessDays.AddBusinessDays(OrderDate, 5);
                        break;
                    case "Sudeste":
                        deliveryDate = BusinessDays.AddBusinessDays(OrderDate, 1);
                        break;
                }
                return deliveryDate;

            }catch (Exception ex)
            {
                throw;
            }
        }

        private static async Task<Location> GetLocation(long cep)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("viacep.com.br/ws/");
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
                client.BaseAddress = new Uri("https://servicodados.ibge.gov.br/api/v1/localidades/municipios");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Regiao? region = new Regiao();
                string url = "/" + id;
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
