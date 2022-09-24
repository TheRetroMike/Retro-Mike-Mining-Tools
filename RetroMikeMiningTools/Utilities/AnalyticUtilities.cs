using Newtonsoft.Json;
using RestSharp;
using System.Reflection;
using System.Text;

namespace RetroMikeMiningTools.Utilities
{
    public static class AnalyticUtilities
    {
        public static void Startup()
        {
            try
            {
                var client = new RestClient(Common.Constants.IP_API);
                var request = new RestRequest();
                var response = client.Get(request);
                dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
                var country = responseContent.country.ToString();
                var region = responseContent.regionName.ToString();
                var city = responseContent.city.ToString();

                StringBuilder message = new StringBuilder();
                message.AppendLine("Application Started");
                message.AppendLine(String.Format("City: {0}", city));
                message.AppendLine(String.Format("Region: {0}", region));
                message.AppendLine(String.Format("Country: {0}", country));
                message.AppendLine(String.Format("Version: {0}", Assembly.GetEntryAssembly()?.GetName()?.Version));
                Common.Logger.Push(message.ToString());
            }
            catch { }
        }
    }
}
