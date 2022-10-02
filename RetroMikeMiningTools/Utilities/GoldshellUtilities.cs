using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.Utilities
{
    public class GoldshellUtilities
    {
        public static AsicCoin GetCurrentPool(GoldshellAsicConfig asic)
        {
            AsicCoin result = null;
            try
            {
                if (!String.IsNullOrEmpty(asic.ApiBasePath) && !String.IsNullOrEmpty(asic.ApiPassword))
                {
                    string poolApi = "/mcb/pools";
                    var client = new RestClient(asic.ApiBasePath);
                    var request = new RestRequest(poolApi, Method.Get);
                    var response = client.Get(request);
                    List<GoldshellAsicPool> pools = JsonConvert.DeserializeObject<List<GoldshellAsicPool>>(response.Content);
                    var currentPool = pools.Where(x => x.Active).OrderBy(x => x.PoolPriority).Take(1).FirstOrDefault();
                    if (currentPool != null)
                    {
                        result = new AsicCoin()
                        {
                            Pool = currentPool.Url,
                            PoolUser = currentPool.User,
                            PoolPassword = currentPool.Password,
                        };
                    }
                }
            }
            catch { }
            return result;
        }
    }
}
