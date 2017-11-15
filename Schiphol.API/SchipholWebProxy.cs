using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Schiphol.FlightAPI
{
    public class SchipholWebProxy : IWebProxy
    {
        public string APP_ID { get; set; }
        public string APP_KEY { get; set;  }
        public string SchipholApiUrl { get; set; }

        public SchipholWebProxy() { }

        public SchipholWebProxy(string schipholApiUrl, string appId, string appKey)
        {
            APP_ID = appId;
            APP_KEY = appKey;
            SchipholApiUrl = schipholApiUrl;
        }

        public async Task<FlightApiResponse> Get(string route, string resourceVersion, int page, Dictionary<string, string> moreParams = null)
        {
            if (page < 0)
                throw new ArgumentOutOfRangeException(nameof(page), page, "Page should be higher or equal to 0.");

            if(string.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            FlightApiResponse result = new FlightApiResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(SchipholApiUrl);
                client.DefaultRequestHeaders.Add("ResourceVersion", resourceVersion);

                var other = ToQueryString(moreParams);
                var query = $"public-flights/{route}?app_id={APP_ID}&app_key={APP_KEY}&page={page}{other}";
                HttpResponseMessage response = await client.GetAsync(query);
                result.StatusCode = response.StatusCode;

                if (!response.IsSuccessStatusCode)
                    result.ErrorMessage = response.ReasonPhrase;
                else
                    result.Content = await response.Content.ReadAsStringAsync();

                return result;
            }
        }

        private static string ToQueryString(IDictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0) return string.Empty;

            var buffer = new StringBuilder();
            foreach (var key in parameters.Keys)
            {
                buffer.AppendFormat("&{0}={1}", key, HttpUtility.UrlEncode(parameters[key]));
            }

            return buffer.ToString();
        }
    }
}
