using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Maps.MapControl.WPF;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV19.Models
{
    internal class MapModel
    {
        public static List<Location> CalculateZone(ObservableCollection<PNZAPoint> pNZAPoints, int stepInMetrs)
        {
            Location maxLat = new Location(-90, 0); // начальные значения для поиска максимальных и минимальных координат
            Location minLat = new Location(90, 0);
            Location maxLng = new Location(0, -180);
            Location minLng = new Location(0, 180);
            foreach (var point in pNZAPoints)
            {
                if (point.Location.Latitude > maxLat.Latitude)
                    maxLat = point.Location;
                if (point.Location.Latitude < minLat.Latitude)
                    minLat = point.Location;
                if (point.Location.Longitude > maxLng.Longitude)
                    maxLng = point.Location;
                if (point.Location.Longitude < minLng.Longitude)
                    minLng = point.Location;
            }
            List<Location> zone = new List<Location>();

            double step = CalculationModel.Calculation.ConvertMetrsToCoordinates(stepInMetrs / Math.Sqrt(2));
            Location leftUp = new Location(maxLat.Latitude + step, minLng.Longitude - step);
            Location leftDown = new Location(minLat.Latitude - step, minLng.Longitude - step);
            Location rightDown = new Location(minLat.Latitude - step, maxLng.Longitude + step);
            Location rightUp = new Location(maxLat.Latitude + step, maxLng.Longitude + step);

            zone.Add(leftUp);
            zone.Add(leftDown);
            zone.Add(rightDown);
            zone.Add(rightUp);

            return zone;
        }
        public static List<GridPolygon> DrawZone(List<Location> zone, int stepInMetrs, Collection<PNZAPoint> pNZAPoints)
        {
            List<GridPolygon> polygons = new List<GridPolygon>();
            double latitudinalStep = (CalculationModel.Calculation.ConvertMetrsToCoordinates(stepInMetrs) / 1.51);
            double longitudinalStep = CalculationModel.Calculation.ConvertMetrsToCoordinates(stepInMetrs);
            double latitude = zone[0].Latitude;
            double longitude = zone[0].Longitude;

            while (latitude > zone[1].Latitude)
            {
                while (longitude < zone[2].Longitude)
                {
                    Location leftUp = new Location(latitude, longitude);
                    Location leftDown = new Location(latitude - latitudinalStep, longitude);
                    Location rightDown = new Location(latitude - latitudinalStep, longitude + longitudinalStep);
                    Location rightUp = new Location(latitude, longitude + longitudinalStep);

                    if (rightDown.Latitude < zone[1].Latitude || rightDown.Longitude > zone[2].Longitude)
                        break;

                    GridPolygon polygon = new GridPolygon(leftUp, leftDown, rightDown, rightUp);
                    double pointsSum = 0;
                    double pointDistances = 0;
                    foreach (PNZAPoint point in pNZAPoints)
                    {
                        if (point.IsSelected)
                        {
                            pointsSum += Convert.ToDouble(point.Value) / Math.Pow(CalculationModel.Calculation.CalculateDistance(point.Location.Latitude, point.Location.Longitude, polygon.Center.Latitude, polygon.Center.Longitude), 2);
                            pointDistances += 1 / Math.Pow(CalculationModel.Calculation.CalculateDistance(point.Location.Latitude, point.Location.Longitude, polygon.Center.Latitude, polygon.Center.Longitude), 2);
                        }
                    }
                    polygon.Value = pointsSum / pointDistances;
                    polygons.Add(polygon);
                    longitude += longitudinalStep;
                }
                longitude = zone[0].Longitude;
                latitude -= latitudinalStep;
            }
            return polygons;
        }

    }
}
