using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minsa.VPA.Modelos
{
    public class jsonResponseCODI
    {
        [JsonProperty("returnCodes")]
        public ReturnCodes ReturnCodes { get; set; }

        [JsonProperty("imageResponse")]
        public string ImageResponse { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }
    }

    public class ReturnCodes
    {
        [JsonProperty("returnCode")]
        public string ReturnCode { get; set; }

        [JsonProperty("application")]
        public string Application { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}