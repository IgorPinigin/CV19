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


    }
}
