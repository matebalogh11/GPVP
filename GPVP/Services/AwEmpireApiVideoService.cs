using GPVP.Entities;
using GPVP.HelperClasses;
using GPVP.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GPVP.Services
{
    public class AwEmpireApiVideoService : IVideoService
    {
        #region Fields



        #endregion

        #region Interface

        public Task<VideoPage> GetVideoBySecualOrientationAsync(string orientation)
        {
            throw new NotImplementedException();
        }

        public VideoPage GetVideosByPageNumber(int pageNumber)
        {
            throw new NotImplementedException();
        }

        public VideoPage GetVideosBySexualOrientation(string orientation)
        {
            throw new NotImplementedException();
        }

        public async Task<VideoPage> GetVideoByPageNumberAsync( long pageNumber = 1 )
        {
            try
            {
                string apiString = $"{Settings.Default.StaticAddress}&clientIp={IpAddressHelper.Instance.Ipv4Address}&pageIndex={pageNumber}";
                using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(apiString))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        return ConvertJsonResult(responseString);
                    }
                    else
                    {
                        //logging
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                //logging
                throw new Exception($"Exception during API request: {e.Message}");
            }
        }
        #endregion

        #region Private methods

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
            catch(Exception ex)
            {
                throw new Exception($"Exception during response json conversion:{ex.Message}");
            }
            return result;
        }

        #endregion
    }
}
