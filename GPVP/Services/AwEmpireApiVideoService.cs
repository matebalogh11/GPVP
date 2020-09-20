using GPVP.Entities;
using GPVP.HelperClasses;
using GPVP.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            StartLoadingVideosAsync();
        }

        #region Interface

        public Video GetVideoById(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<VideoPage> GetVideos( int pageNumber = 1 )
        {
            //var cachedVid = TryGetVideoFromCache(pageNumber);
            //if (cachedVid != null)
            //    return cachedVid;

            return await GetVideosFromApi(pageNumber);
        }

        #endregion

        #region Private methods

        private string configureApiString( int pageNumber = 1 )
        {
            var sb = new StringBuilder(apiString);
            sb.Append($"&pageIndex={pageNumber}");
            sb.Append("&limit=25");

            return sb.ToString();
        }

        private VideoPage TryGetVideoFromCache( int pageId )
        {
            return VideoCache.Instance.GetVideoPage(pageId);
        }

        private async Task<VideoPage> GetVideosFromApi(int pageNumber = 1)
        {
            var configuredString = configureApiString(pageNumber);

            HttpResponseMessage response = await client.GetAsync(configuredString);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return ConvertJsonResult(responseString);
            }
            else
            {
                //exception
                return null;
            }
        }

        private VideoPage ConvertJsonResult( string response )
        {
            var result = new VideoPage { Videos = new List<Video>(), Pagination = new Page() };
            try
            {
                JObject responseObject = JObject.Parse(response);

                JObject dataJson = (JObject)responseObject[Settings.Default.ApiDataString];
                JArray videoJsonArray = (JArray)dataJson[Settings.Default.ApiVideoString];
                var pageJson = dataJson[Settings.Default.ApiPaginationString];

                result.Videos = JsonConvert.DeserializeObject<Video[]>(videoJsonArray.ToString());
                result.Pagination = JsonConvert.DeserializeObject<Page>(pageJson.ToString());
            }
            catch( JsonReaderException jrex )
            {
                //different exceptions throw
            }
            catch(KeyNotFoundException knfex)
            {

            }
            catch(Exception ex)
            {

            }
            return result;
        }

        private void StartLoadingVideosAsync()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler((object sender, DoWorkEventArgs e) =>
            {
                while (VideoCache.Instance.CachedVideoCount < Settings.Default.PageNumberToCache)
                {
                    VideoCache.Instance.UpdateVideoCache(GetVideosFromApi(VideoCache.Instance.CachedVideoCount + 1).Result);
                }
            });
            bw.RunWorkerAsync();
        }

        #endregion

    }
}
