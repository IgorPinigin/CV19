using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maps.MapControl.WPF;
using System.Linq;
using System.Threading.Tasks;
using Geocoding.Microsoft;

namespace CV19.Models
{
    public class GeoCodingModel
    {
        public static async Task<(double, double)> GetCityCoordinates(string cityName)
        {
            var geocoder = new BingMapsGeocoder("eZNgmmR40DyTunPmOrBp~kolAxjG20pwiJF7fIm2QRw~AgWBvZTh2hKwZ5-mh8lAl9VPhPh9CcU0YgZtlG62RHVmxwhFLrPTU1UecR96Old1");
            var addresses = await geocoder.GeocodeAsync(cityName);
            if (addresses.Any())
            {
                var location = addresses.First().Coordinates;
                return (location.Latitude, location.Longitude);
            }
            else
            {
                Location def = new Location(50.406829, 80.232370);
                return (def.Latitude, def.Longitude);
            }
        }
    }
}
