using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MediaTonic.AnimalGame.Tests.Extensions
{
    //https://stackoverflow.com/questions/37750451/send-http-post-message-in-asp-net-core-using-httpclient-postasjsonasync
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        { }
    }
}
