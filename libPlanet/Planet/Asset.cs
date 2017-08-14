using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Planet
{
    public class Asset
    {
        public String self;
        public String activate;
        public String type;
        public List<string> permissions = new List<string>();
        public String md5Digest; //ASK DATATYPE
        public String status;
        public DateTime expiresAt;
        public String location;
        public event EventHandler AssetActivated;
        public event EventHandler AssetDownloaded;





        public Asset(JObject JsonAsset)
        {
            this.self = JsonAsset["_links"]["_self"].ToString();
            this.activate = JsonAsset["_links"]["activate"].ToString();
            this.type = JsonAsset["type"].ToString();
            foreach (var permission in JsonAsset["_permissions"])
            {
                this.permissions.Add(permission.ToString());
            }
            this.status = JsonAsset["status"].ToString();
            if (JsonAsset["status"].ToString().Equals("activated"))
            {
                this.expiresAt = (DateTime)JsonAsset["expires_at"];
                this.location = JsonAsset["location"].ToString();
            }
            this.md5Digest = JsonAsset["md5_digest"].ToString();
        }


        /// <summary>
        /// Creates a dictionary of all Assets downloaded via the link
        /// </summary>
        /// <param name="link">The Asset link of the planetImage Object</param>
        /// <returns>A dictionary of Assets</returns>
        public static Dictionary<string, Asset> getAllAssets(String link)
        {

            WebRequest request = WebRequest.Create(new Uri(link));
            request.Method = "GET";

            Dictionary<string, Asset> assets = new Dictionary<string, Asset>();

            foreach (JProperty assetJson in JObject.Parse(API.request(request).Result).Properties())
            {
                Asset asset = new Asset((JObject)assetJson.Value);

                assets.Add(assetJson.Name, asset);
            }

            return assets;
        }


        public async void activateAsset()
        {
            WebRequest request = WebRequest.Create(activate);
            await API.request(request);

            request = WebRequest.Create(self);
            request.Method = "GET";
            var JsonAsset = JObject.Parse(API.request(request).Result);

            while (JsonAsset["status"].ToString().Equals("inactive"))
            {
                JsonAsset = JObject.Parse(API.request(request).Result);
                Thread.Sleep(5000);
            }

            Thread.Sleep(3000);

            this.expiresAt = (DateTime)JsonAsset["expires_at"];
            this.location = JsonAsset["location"].ToString();

            AssetActivated(this, EventArgs.Empty);
        }

        public async void downloadAsset(string filepath)
        {
            using (var client = new HttpClient()
            {
                Timeout = Timeout.InfiniteTimeSpan


            })
            using (var request = new HttpRequestMessage(HttpMethod.Get, location))


            using (
                Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync(),
                stream = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None, 3145728, true))
            {
                await contentStream.CopyToAsync(stream);
            }

            AssetDownloaded(this, EventArgs.Empty);
        }
    }
}
