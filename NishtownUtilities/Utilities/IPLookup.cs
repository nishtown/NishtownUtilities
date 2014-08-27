using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Xml;

namespace Nishtown.Utilities
{
    public class IPLookup
    {
        //access the following properties after a successful lookup.
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string IpAddress { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string TimeZone { get; set; }

        public string apikey { get; set; }

        public bool GetInfo(string IP)
        {
            string key = this.apikey; //replace with your actual key. 
            string url = "http://api.ipinfodb.com/v3/ip-city/?format=xml&key=" + key + "&ip=";

            HttpWebResponse res = null;

            try
            {
                var req = WebRequest.Create(url + IP) as HttpWebRequest;

                XmlDocument xob = null;

                res = req.GetResponse() as HttpWebResponse;
                if (req.HaveResponse == true && res != null)
                {
                    xob = new XmlDocument();
                    xob.Load(res.GetResponseStream());
                }
                else return false;

                if (xob != null)
                {
                    var x = xob.SelectSingleNode("Response");
                    if (x != null)
                    {
                        StatusCode = x["statusCode"].InnerText;
                        StatusMessage = x["statusMessage"].InnerText;
                        IpAddress = x["ipAddress"].InnerText;
                        CountryCode = x["countryCode"].InnerText;
                        CountryName = x["countryName"].InnerText;
                        RegionName = x["regionName"].InnerText;
                        CityName = x["cityName"].InnerText;
                        ZipCode = x["zipCode"].InnerText;
                        Latitude = x["latitude"].InnerText;
                        Longitude = x["longitude"].InnerText;
                        TimeZone = x["timeZone"].InnerText;

                    }
                }
                else return false;

            }
            catch { return false; }
            finally
            {
                if (res != null)
                { res.Close(); }
            }
            return true;
        }
    }
}
