
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared;

namespace ReportApi.Services
{
    public class BaseService
    {
        public string MeterApiUrl = "";

        private readonly IOptions<MeterApiConfiguration> _configurationService;
        private IHttpContextAccessor _httpContextAccessor;

        public BaseService(IOptions<MeterApiConfiguration> configurationService, IHttpContextAccessor httpContextAccessor)
        {
            _configurationService = configurationService;
            _httpContextAccessor = httpContextAccessor;
            MeterApiUrl = _configurationService.Value.MeterAPIUrl;
        }



        public GenericResult Get(string path)
        {
            using (var handler = new HttpClientHandler { Credentials = new System.Net.NetworkCredential() })
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    return true;
                };
                handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                using (var client = new HttpClient(handler))
                {
                    string jsonStringResult = null;
                    client.DefaultRequestHeaders.Accept.Clear();
                    var task = client.GetAsync(MeterApiUrl + path)
                        .ContinueWith((taskwithresponse) =>
                        {
                            var response = taskwithresponse.Result;
                            var jsonString = response.Content.ReadAsStringAsync();
                            jsonString.Wait();
                            jsonStringResult = jsonString.Result;

                        });
                    task.Wait();
                    dynamic result;

                    result = JsonConvert.DeserializeObject<dynamic>(jsonStringResult);

                    GenericResult resultGet = JsonConvert.DeserializeObject<GenericResult>(result + "");
                    return resultGet;
                }
            }
        }



    }
}
