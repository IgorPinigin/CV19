//using CV19.Models.RoseWindModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace CV19.Models.CalculationModel
{
    class Calculation
    {
        public Calculation() 
        {
            
        }

        double RadiusMin = 0.005;

        public double GetRoseWind(Tuple<double, double> cent_p, Tuple<double, double> p)
        {
            //double Fi = RoseWind.Ang(cent_p.Item1, cent_p.Item2, p.Item1, p.Item2);
            //double Rose = RoseWind.Rose(Fi) / RoseWind.AntiRose("NW");
            //return Rose;
            return 1;
        }

        public double RModel(double X, double Xi, double Y, double Yi, double RadiusMax) 
        {            
            double Radius = Math.Sqrt(Math.Pow((X - Xi), 2) + Math.Pow((Y - Yi), 2));
            if (Radius < RadiusMin)
                return 1 / (Math.Pow(Radius, 2) + 2 * RadiusMax * Radius + 2 * Math.Pow(RadiusMax, 2));
            return 1 / (Math.Pow(Radius, 2) * Math.Pow(Math.E, (RadiusMax * 2 / Radius)) * GetRoseWind(new Tuple<double, double>(Xi, Yi), new Tuple<double, double>(X, Y))); 
        }

        public double ConcInPnt(Tuple<double, double> Pnt, double[,] SrcPnt, double RadiusMax = 0.5, double Tt = 1.0)
        {
            double Conc = 0.0;
            for (int i = 0; i < SrcPnt.GetLength(0); i++)
            {
                double RModelValue = RModel(Pnt.Item1, Pnt.Item2, SrcPnt[i, 0], SrcPnt[i, 1], RadiusMax);
                Conc += Tt * RModelValue;
            }
            return Conc;
        }

        public double LpInPoint(Tuple<double, double> Pnt, List<Tuple<double[,], double>> SrcPnt, double[,] A, double[] B, double Mix = 1.0)
        {
            List<double> CArr = new();
            foreach (Tuple<double[,], double> el in SrcPnt)
            {
                double Conc = Mix * ConcInPnt(Pnt, el.Item1, el.Item2);
                CArr.Add(Conc);
            }
           return  1;
        }






        private const double EarthRadiusKm = 6371;

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        private const double EarthRadius = 6371000; // радиус Земли в метрах
        private const double DegreesToRadians1 = Math.PI / 180.0; // коэффициент для перевода градусов в радианы

        public static double ConvertMetrsToCoordinates(double distanceInMeters)
        {
            // расчет шага по широте в градусах
            double latitudinalDistance = distanceInMeters / EarthRadius;
            double latitudinalDegrees = latitudinalDistance / DegreesToRadians1;

            return latitudinalDegrees;
        }
    }
}
