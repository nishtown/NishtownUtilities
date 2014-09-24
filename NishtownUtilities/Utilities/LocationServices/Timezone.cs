using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Net;

namespace Nishtown.Utilities.LocationServices
{
    public class Timezone
    {
        public string Location { get; private set; }
        public string TimezoneRegion { get; private set; }
        public string TimezoneName { get; private set; }
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }
        public double Dst { get; private set; }
        public double Gmt { get; private set; }
        public string Map { get; private set; }

        private string api = "";

        public Timezone();

        public Timezone(string apikey, string address)
        {
            api = apikey;
            GPSPos gps = new GPSPos(api);
            Query(gps.Locate(address));
        }

        public void Query(GPS gps)
        {
            Longitude = gps.Longitude;
            Latitude = gps.Latitude;
            Location = gps.Location;

            var requestUri = string.Format("https://maps.googleapis.com/maps/api/timezone/xml?location={0}&key={1}&timestamp={2}", Latitude + "," + Longitude, api, UnixTimestampFromDateTime(DateTime.Now));
            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());
            var result = xdoc.Element("TimeZoneResponse");

            var rawoffset = Convert.ToDouble(result.Element("raw_offset").Value);
            var dstoffset = Convert.ToDouble(result.Element("dst_offset").Value);
            TimezoneRegion = result.Element("time_zone_id").Value;
            TimezoneName = result.Element("time_zone_name").Value;

            if (rawoffset != 0)
            {
                rawoffset = rawoffset / 60 / 60;
            }
            if (dstoffset != 0)
            {
                dstoffset = dstoffset / 60 / 60;
            }

            Dst = dstoffset;
            Gmt = rawoffset;

            Map = string.Format("https://maps.googleapis.com/maps/api/staticmap?center={0}&zoom=2&size=400x400&markers={1}&key={2}", Longitude + "," + Latitude, Longitude + "," + Latitude, api);
        }

        private long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }
    }

}
