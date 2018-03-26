using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Raincoat.BE
{
    public class SlideshowBE
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageValue { get; set; }
        public int Order { get; set; }
        public string ImagePath
        {
            get
            {
                return string.Concat(hostPath, string.Format("Content/images/slides/{0}.jpg", Id));
            }
        }
        public string hostPath { get; set; }

        public SlideshowBE()
        {
            Id = -1;
            Name = string.Empty;
            ImageValue = string.Empty;
            Order = -1;
            hostPath = string.Empty;
        }
    }
}