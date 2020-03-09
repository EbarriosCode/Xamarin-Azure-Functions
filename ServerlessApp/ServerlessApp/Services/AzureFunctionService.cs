using Newtonsoft.Json;
using ServerlessApp.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessApp.Services
{
    public class AzureFunctionService
    {
        public async Task<Response> PesoIdealAzureFunctionAsync<T>(string url, PesoIdealParams model)
        {
            try
            {
                var client = new HttpClient();
                
                var json = JsonConvert.SerializeObject(model);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync(url, contentJson);
                var content = await response.Content.ReadAsStringAsync();                

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = (int)response.StatusCode,
                        Message = response.ReasonPhrase
                    };
                }

                var resp = JsonConvert.DeserializeObject<PesoIdealResult>(content);               
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    StatusCode = (int)response.StatusCode,
                    DataResult = resp
                }; 
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
