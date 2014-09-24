using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml.Linq;

namespace Nishtown.Utilities.LocationServices
{
    public class GPS
    {
        public GPS();
        public string Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        
    }

    public class GPSPos
    {
        public string apikey { private get; set; }

        public GPSPos()
        {

        }

        public GPSPos(string key)
        {
            apikey = key;
        }

        public GPS Locate(string address)
        {
            var requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false&key={1}", Uri.EscapeDataString(address), api);
            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());
            var result = xdoc.Element("GeocodeResponse").Element("result");
            var locationElement = result.Element("geometry").Element("location");

            var lng = Convert.ToDouble(locationElement.Element("lng").Value);
            var lat = Convert.ToDouble(locationElement.Element("lat").Value);

            return new GPS() { Latitude = lat, Longitude = lng, Location = address };
        }
    }
}
