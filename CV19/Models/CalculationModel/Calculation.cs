using CV19.Models.RoseWindModel;
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

        double radius_min = 0.005;

        public double GetRoseWind(Tuple<double, double> cent_p, Tuple<double, double> p)
        {
            double fi = RoseWind.Ang(cent_p.Item1, cent_p.Item2, p.Item1, p.Item2);
            double rose = rs_w.Rose(fi) / rs_w.AntiRose("NW");
            return rose;
        }

        public double RModel(double x, double xi, double y, double yi, double radius_max) 
        {            
            double radius = Math.Sqrt(Math.Pow((x - xi), 2) + Math.Pow((y - yi), 2));
            if (radius < radius_min)
                return 1 / (Math.Pow(radius, 2) + 2 * radius_max * radius + 2 * Math.Pow(radius_max, 2));
            return 1 / (Math.Pow(radius, 2) * Math.Pow(Math.E, (radius_max * 2 / radius)) * GetRoseWind(new Tuple<double, double>(xi, yi), new Tuple<double, double>(x, y))); 
        }

        public double ConcInPnt(Tuple<double, double> pnt, double[,] src_pnt, double radius_max = 0.5, double tt = 1.0)
        {
            double conc = 0.0;
            for (int i = 0; i < src_pnt.GetLength(0); i++)
            {
                double rModelValue = RModel(pnt.Item1, pnt.Item2, src_pnt[i, 0], src_pnt[i, 1], radius_max);
                conc += tt * rModelValue;
            }
            return conc;
        }

        public double LpInPoint(Tuple<double, double> pnt, List<Tuple<double[,], double>> src_pnt, double[,] a, double[] b, double mix = 1.0)
        {
            List<double> c_arr = new();
            foreach (Tuple<double[,], double> el in src_pnt)
            {
                double conc = mix * ConcInPnt(pnt, el.Item1, el.Item2);
                c_arr.Add(conc);
            }

            

            

            return /*чт-то*/ ?? -1;
        }
        //def get_rw(cent_p, p):
        //    fi = self.rs_w.ang(cent_p[0], cent_p[1], p[0], p[1])
        //    rose = self.rs_w.rose(fi) / self.rs_w.anti_rose("NW")
        //    return rose
    }
}
