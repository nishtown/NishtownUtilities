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
        

        public string apikey { private get; set; }

        public IPGeolocationInformation GetInfo(IPAddress IP)
        {
            IPGeolocationInformation ipg = IP as IPGeolocationInformation;

            string key = this.apikey;
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
                    if (xob != null)
                    {
                        var x = xob.SelectSingleNode("Response");
                        if (x != null)
                        {
                            ipg.StatusCode = x["statusCode"].InnerText;
                            ipg.StatusMessage = x["statusMessage"].InnerText;
                            ipg.CountryCode = x["countryCode"].InnerText;
                            ipg.CountryName = x["countryName"].InnerText;
                            ipg.RegionName = x["regionName"].InnerText;
                            ipg.CityName = x["cityName"].InnerText;
                            ipg.ZipCode = x["zipCode"].InnerText;
                            ipg.Latitude = x["latitude"].InnerText;
                            ipg.Longitude = x["longitude"].InnerText;
                            ipg.TimeZone = x["timeZone"].InnerText;

                        }
                    }
                }
            }
            catch
            {
                return null; 
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return ipg;
        }
    }

    public class IPGeolocationInformation: IPAddress
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string TimeZone { get; set; }

        public IPGeolocationInformation(byte[] address)
            : base(address)
        {

        }

        public IPGeolocationInformation(long address)
            : base(address)
        {

        }
        public IPGeolocationInformation(byte[] address, long scopeid)
            : base(address, scopeid)
        {

        }
    }
}
