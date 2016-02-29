using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MMDPowerball.Controllers
{
    public class AlexaController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            try
            {
                var speechlet = new pbSpeechlet();
                result = speechlet.GetResponse(Request);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (result.StatusCode != HttpStatusCode.OK)
                Trace.TraceInformation(result.ToString());

            return result;
        }
    }
}
