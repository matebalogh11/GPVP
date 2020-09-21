using System;
using System.Collections.Generic;
using System.Linq;
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
                    string uriString = "http://www." + PreviewImages[0].Substring(2);
                    return new Uri(uriString, UriKind.Absolute);
                }
                return new Uri("/Resources/Images/no-image.png", UriKind.Relative);
            }
        }

        public BitmapImage CachedImage { get; set; }

        public bool CachedImgIsNull
        {
            get { return CachedImage == null; }
        }

        public string TagPeek
        {
            get
            {
                if ( Tags != null && Tags.Count() > 0 )
                {
                    return Tags.Count() < 3 ? string.Join(", ", Tags.Take(Tags.Count())) : string.Join(", ", Tags.Take(3)) + "...";
                }
                return string.Empty;
            }
        }

        public string DurationString => $"{Duration} min.";
    }
}
