using GPVP.Properties;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GPVP.HelperClasses
{
    public static class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitializeClinet()
        {
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri(Settings.Default.BaseAddress)
            };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
