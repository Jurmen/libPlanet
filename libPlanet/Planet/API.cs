using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Planet
{
    public static class API
    {
        public static string API_KEY;
        const string URLPREFIX = "https://api.planet.com/data/v1/";


        /// <summary>
        /// Executes a request
        /// </summary>
        /// <param name="request">The request that is supposed to be executed</param>
        /// <returns>The Response of the API</returns>
        public static async Task<string> request(WebRequest request)
        {
            request.Credentials = new NetworkCredential(API_KEY, "");

            using (WebResponse result = await request.GetResponseAsync())
            {
                using (StreamReader sr = new StreamReader(result.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }

        }



        /// <summary>
        /// Sends a API request to Planet API containing a search filter
        /// </summary>
        /// <param name="filter">A JObject containing filterdata for the Planet API</param>
        /// <returns>A list containing PlanetImages</returns>
        public static List<PlanetImage> search(JObject filter)
        {
            WebRequest webRequest = WebRequest.Create(URLPREFIX + "quick-search");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(webRequest.GetRequestStreamAsync().Result))
            {
                streamWriter.Write(filter);
                streamWriter.Flush();
            }

            List<PlanetImage> searchResult = new List<PlanetImage>();

            foreach (JObject image in JObject.Parse(request(webRequest).Result)["features"])
            {
                PlanetImage planetImage = new PlanetImage(image);
                searchResult.Add(planetImage);

            }

            return searchResult;
        }
    }
}
