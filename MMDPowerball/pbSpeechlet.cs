using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace MMDPowerball
{
    public class pbSpeechlet : Speechlet
    {
        public override SpeechletResponse OnIntent(IntentRequest intentRequest, Session session)
        {
            Trace.TraceInformation("OnIntent");
            return BuildSpeechletResponse("Powerball on Azure", GetPowerballNumbers(), true);
        }

        public override SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session)
        {
            return BuildSpeechletResponse("Powerball on Azure", GetPowerballNumbers(), true);
        }

        public override void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session)
        {

        }

        public override void OnSessionStarted(SessionStartedRequest sessionStartedRequest, Session session)
        {
        }
        private string GetPowerballNumbers()
        {
            var client = new HttpClient();
            var endpoint = "[KimonoLabs API call]";
            var response = client.GetAsync(endpoint).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            dynamic jData = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
            dynamic pBall = jData.results.collection1[0];
            string speech = "<speak>{0} "
                            + "<break />{1}{7}{2}{7}{3}{7}{4}{7}{5}<break />"
                            + "powerball {6}"
                            + "</speak>";

            return string.Format(speech,
                pBall.drawingDate,
                pBall.number1,
                pBall.number2,
                pBall.number3,
                pBall.number4,
                pBall.number5,
                pBall.powerBall,
                "<break time='300ms' />");
        }




        private SpeechletResponse BuildSpeechletResponse(string title, string output, bool shouldEndSession)
        {
            Trace.TraceInformation(output);

            // Create the Simple card content.
            SimpleCard card = new SimpleCard();
            card.Title = String.Format("SessionSpeechlet - {0}", title);
            card.Subtitle = String.Format("SessionSpeechlet - Sub Title");
            card.Content = String.Format("SessionSpeechlet - {0}", output);

            SsmlOutputSpeech speech = new SsmlOutputSpeech()
            {
                Ssml = output
            };

            // Create the speechlet response.
            SpeechletResponse response = new SpeechletResponse();
            response.ShouldEndSession = shouldEndSession;
            response.OutputSpeech = speech;
            response.Card = card;
            return response;
        }
    }
}