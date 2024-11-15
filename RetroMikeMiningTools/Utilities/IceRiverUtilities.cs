using Kucoin.Net.Objects.Models.Spot;
using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.Utilities
{
    public static class IceRiverUtilities
    {
        public static AsicCoin GetCurrentPool(string ip, string apiKey)
        {
            var result = new AsicCoin();
            var options = new RestClientOptions(String.Format("https://{0}/api", ip))
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            var client = new RestClient(options);
            var request = new RestRequest("/overview");
            request.AddHeader("Authorization", String.Format("Bearer {0}", apiKey));
            var response = client.Get(request);
            dynamic responseData = JsonConvert.DeserializeObject(response.Content);
            if (responseData != null && responseData.data != null && responseData.data.pools != null)
            {
                foreach (var pool in responseData.data.pools)
                {
                    var stratum = pool.addr;
                    var user = pool.user;
                    var password = pool.pass;
                    var connected = pool.connect;
                    var state = pool.state;

                    if (state=="1" || connected=="True")
                    {
                        result = new AsicCoin()
                        {
                            Pool = Convert.ToString(stratum),
                            PoolUser = Convert.ToString(user),
                            PoolPassword = Convert.ToString(password)
                        };
                        return result;
                    }
                }
            }
            return result;
        }

        public static void ReplacePools(string ip, string apiKey, AsicCoinConfig poolConfig)
        {
            var options = new RestClientOptions(String.Format("https://{0}/api", ip))
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            var client = new RestClient(options);
            var request = new RestRequest("/pool/list");
            request.AddHeader("Authorization", String.Format("Bearer {0}", apiKey));
            var response = client.Get(request);
            dynamic responseData = JsonConvert.DeserializeObject(response.Content);
            if (responseData != null && responseData.pools != null)
            {
                foreach (var existingPool in responseData.pools)
                {
                    int poolId = Convert.ToInt32(existingPool.id);

                    //Delete pool
                    var deleteRequest = new RestRequest("/pool/delete");
                    deleteRequest.AddHeader("Authorization", String.Format("Bearer {0}", apiKey));
                    deleteRequest.AddParameter("id", poolId);
                    var deleteResponse = client.Post(deleteRequest);
                }
            }

            if (poolConfig != null && !String.IsNullOrEmpty(poolConfig.Pool))
            {
                var createRequest = new RestRequest("/pool/create");
                createRequest.AddHeader("Authorization", String.Format("Bearer {0}", apiKey));
                createRequest.AddParameter("pool_address", poolConfig.Pool);
                createRequest.AddParameter("wallet", poolConfig.PoolUser);
                createRequest.AddParameter("pool_password", poolConfig.PoolPassword);
                var createResponse = client.Post(createRequest);
            }
        }
    }
}
