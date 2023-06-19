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

    //    double RadiusMin = 0.005;
    //    private List<Tuple<double, double, double>> src_pnt;

    //    public double GetRoseWind(Tuple<double, double> cent_p, Tuple<double, double> p)
    //    {
    //        //double Fi = RoseWind.Ang(cent_p.Item1, cent_p.Item2, p.Item1, p.Item2);
    //        //double Rose = RoseWind.Rose(Fi) / RoseWind.AntiRose("NW");
    //        //return Rose;
    //        return 1;
    //    }

    //    public double RModel(double X, double Xi, double Y, double Yi, double RadiusMax) 
    //    {            
    //        double Radius = Math.Sqrt(Math.Pow((X - Xi), 2) + Math.Pow((Y - Yi), 2));
    //        if (Radius < RadiusMax)
    //            return 1 / (Math.Pow(Radius, 2) + 2 * RadiusMax * Radius + 2 * Math.Pow(RadiusMax, 2));
    //        return 1 / (Math.Pow(Radius, 2) * Math.Pow(Math.E, (RadiusMax * 2 / Radius)) * GetRoseWind(new Tuple<double, double>(Xi, Yi), new Tuple<double, double>(X, Y))); 
    //    }

    //    public double ConcInPnt(Tuple<double, double> Pnt, double[,] SrcPnt, double RadiusMax = 0.5, double Tt = 1.0)
    //    {
    //        double Conc = 0.0;
    //        for (int i = 0; i < SrcPnt.GetLength(0); i++)
    //        {
    //            double RModelValue = RModel(Pnt.Item1, Pnt.Item2, SrcPnt[i, 0], SrcPnt[i, 1], RadiusMax);
    //            Conc += Tt * RModelValue;
    //        }
    //        return Conc;
    //    }

    //    public double LpInPoint(Tuple<double, double> Pnt, List<Tuple<double[,], double>> SrcPnt, double[,] A, double[] B, double Mix = 1.0)
    //    {
    //        List<double> CArr = new();
    //        foreach (Tuple<double[,], double> el in SrcPnt)
    //        {
    //            double Conc = Mix * ConcInPnt(Pnt, el.Item1, el.Item2);
    //            CArr.Add(Conc);
    //        }
    //       return  1;
    //    }

    //      private double[] CalcField(Tuple<double[,], double[,]> grid_pnt, List<Tuple<double, double, double>> src_pnt, double[] tt)
    //{
    //    double[,] data_x = grid_pnt.Item1;
    //    double[,] data_y = grid_pnt.Item2;
    //    int numRows = data_x.GetLength(0);
    //    int numCols = data_x.GetLength(1);
    //    double[] dt_grid = new double[numRows * numCols];

    //    for (int i = 0; i < numRows; i++)
    //    {
    //        for (int j = 0; j < numCols; j++)
    //        {
    //            Tuple<double, double> point = Tuple.Create(data_x[i, j], data_y[i, j]);
    //            dt_grid[i * numCols + j] = src_pnt.Select((sp, index) => ConcInPnt(point, src_pnt, sp.Item3) * tt[index]).Sum();
    //        }
    //    }

    //    return dt_grid;
    //}

    //public double[] EstimateField(double[,] data_x, double[,] data_y, double[] tt)
    //{
    //    Tuple<double[,], double[,]> grid_pnt = Tuple.Create(data_x, data_y);
    //    return CalcField(grid_pnt, src_pnt, tt);
    //}

    //public double[] EstimateFieldMinMax(double[,] data_x, double[,] data_y, double[] tt)
    //{
    //    Tuple<double[,], double[,]> grid_pnt = Tuple.Create(data_x, data_y);
    //    double[] field = CalcField(grid_pnt, src_pnt, tt);
    //    double fieldMin = field.Min();
    //    double fieldMax = field.Max();
    //    return new double[] { fieldMin, fieldMax };
    //}

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
