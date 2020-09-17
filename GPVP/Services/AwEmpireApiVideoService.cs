using GPVP.Entities;
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
        private HttpClient client;
        string apiString;
        IAddressService ipService;

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

        public  Video GetVideoById(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Video>> GetVideos()
        {
            IEnumerable<Video> result;
            HttpResponseMessage response = await client.GetAsync(apiString);
            if (response.IsSuccessStatusCode)
            {
                var chek = await response.Content.ReadAsStringAsync();
            }
            throw new NotImplementedException();
        }
    }
}
