using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet
{
    public class PlanetImage
    {

        public string imagePath;
        public string self;
        public string assetLink;
        public Dictionary<string, Asset> assets = new Dictionary<string, Asset>();
        public string thumbnail;
        public List<string> permissions = new List<string>();
        public List<List<float>> geometry = new List<List<float>>();
        public string shapeType;
        public string id;
        public DateTime acquired;
        public double anomalousPixels;
        public double cloudCover;
        public int columns;
        public int espgCode;
        public bool groundControl;
        public double gsd;
        public int grid_cell;
        public string instrument;
        public string itemType;
        public double originX;
        public double originY;
        public int pixelResolution;
        public string provider;
        public DateTime published;
        public int rows;
        public string satelliteId;
        public string stripId;
        public double sunAzimuth;
        public double sunElevation;
        public DateTime updated;
        public double usableData;
        public double viewAngle;
        public string type;

        public PlanetImage(JObject imageInfo)
        {


            this.self = (String)imageInfo["_links"]["_self"];
            this.assetLink = (String)imageInfo["_links"]["assets"];
            this.thumbnail = (String)imageInfo["_links"]["thumbnail"];
            foreach (var permission in imageInfo["_permissions"])
            {
                this.permissions.Add(permission.ToString());
            }
            foreach (var coordinate in imageInfo["geometry"]["coordinates"][0])
            {
                this.geometry.Add((new List<float> { (float)coordinate[0], (float)coordinate[1] }));
            }
            this.id = imageInfo["id"].ToString();
            this.acquired = (DateTime)imageInfo["properties"]["acquired"];
            this.anomalousPixels = (double)imageInfo["properties"]["anomalous_pixels"];
            this.cloudCover = (double)imageInfo["properties"]["cloud_cover"];
            this.columns = (int)imageInfo["properties"]["columns"];
            this.espgCode = (int)imageInfo["properties"]["epsg_code"];
            this.grid_cell = (int)imageInfo["properties"]["grid_cell"];
            this.gsd = (double)imageInfo["properties"]["gsd"];
            this.itemType = imageInfo["properties"]["item_type"].ToString();
            this.originX = (double)imageInfo["properties"]["origin_x"];
            this.originY = (double)imageInfo["properties"]["origin_y"];
            this.pixelResolution = (int)imageInfo["properties"]["pixel_resolution"];
            this.provider = imageInfo["properties"]["provider"].ToString();
            this.published = (DateTime)imageInfo["properties"]["published"];
            this.rows = (int)(int)imageInfo["properties"]["rows"];
            this.satelliteId = imageInfo["properties"]["satellite_id"].ToString();
            this.stripId = imageInfo["properties"]["strip_id"].ToString();
            this.sunAzimuth = (double)imageInfo["properties"]["sun_azimuth"];
            this.sunElevation = (double)imageInfo["properties"]["sun_elevation"];
            this.updated = (DateTime)imageInfo["properties"]["updated"];
            this.usableData = (double)imageInfo["properties"]["usable_data"];
            this.viewAngle = (double)imageInfo["properties"]["view_angle"];


        }



        public void getAssets()
        {
            this.assets = Asset.getAllAssets(assetLink);
        }

    }
}
