namespace SmartCrawler.Actor.SlackPublisher
{
    using System;

    using Newtonsoft.Json;

    public static class Slack
    {
        public static void Post(string message)
        {
            try
            {                
                var data = GetJson(message, AppSettings.SlackHookName, AppSettings.SlackHookEmoji);
                HttpPost(AppSettings.SlackHookUrl, data);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static string HttpPost(string uri, string parameters)
        {
            var req = System.Net.WebRequest.Create(uri);
            req.ContentType = "application/json";
            req.Method = "POST";
            var bytes = System.Text.Encoding.UTF8.GetBytes(parameters);
            req.ContentLength = bytes.Length;
            var os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();

            var resp = req.GetResponse();
            var sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        private static string GetJson(string text, string username, string icon_emoji)
        {
            return JsonConvert.SerializeObject(new { text, username, icon_emoji });
        }
    }
}