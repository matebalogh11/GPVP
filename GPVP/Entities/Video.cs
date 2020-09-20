using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GPVP.Entities
{
    public class Video
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string ProfileImage { get; set; }
        public string CoverImage { get; set; }
        public string[] PreviewImages { get; set; }

        public string TargetUrl { get; set; }
        public string DetailsUrl { get; set; }
        public string Quality { get; set; }
        public bool IsHd { get; set; }
        public string Uploader { get; set; }
        public string UploaderLink { get; set; }

        public Uri ImgString
        {
            get 
            {
                if (PreviewImages.Length > 0)
                {
                    string asd = "http://www." + PreviewImages[0].Substring(2);
                    return new Uri(asd, UriKind.Absolute);
                }

                return new Uri(""); //TODO default local image
            }
        }

        public BitmapImage CachedImage { get; set; }

        public bool CachedImgIsNull
        {
            get { return CachedImage == null; }
        }
    }

}
