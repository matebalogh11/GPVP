using GPVP.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GPVP.Services
{
    public class AwEmpireApiVideoService : IVideoService
    {
        #region Fields

        private HttpClient client;
        string apiString;
        IAddressService ipService;

        #endregion

        public AwEmpireApiVideoService()
        {
            ipService = new BasicIPService();

            apiString = string.Format(
                "https://pt.potawe.com/api/video-promotion/v1/list?psid=dobokocka&pstool=421_1&accessKey=6f693a549638e75423bd71e0ee4dc7b0&ms_notrack=1&campaign_id=&type=&sexualOrientation=straight&forcedPerformers=&limit=25&primaryColor=%238AC437&labelColor=%23212121&clientIp={0}",
                ipService.GetLocalIpv4());

            client = new HttpClient();
            //client.BaseAddress = new Uri(apiString);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Interface

        public Video GetVideoById(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<VideoPage> GetVideos( int pageNumber = 1)
        {
            var result = new VideoPage();
            var configuredString = configureApiString(pageNumber);

            HttpResponseMessage response = await client.GetAsync(configuredString);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(responseString);

                JObject data = (JObject)jObject["data"];
                JArray videos = (JArray)data["videos"];
                var page = data["pagination"];

                result.Videos = JsonConvert.DeserializeObject<Video[]>(videos.ToString());
                result.Pagination = JsonConvert.DeserializeObject<Page>(page.ToString());
                return result;
            }
            return result;
        }

        private string configureApiString( int pageNumber = 1 )
        {
            apiString = $"{apiString}&pageIndex={pageNumber}";

            return apiString;
        }

        #endregion
    }
}
