//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;

//namespace CV19.Models.RoseWindModel
//{
//    class RoseWind
//    {
//        public RoseWind()
//        {
//        }
//        public double Ang(double Xi, double Yi, double X, double Y)
//        {
//            double Rs = Math.Atan((Y - Yi) / (X - Xi));
//            if ((X - Xi) < 0)
//            {
//                Rs += Math.PI;
//            }
//            if (Rs < 0)
//            {
//                Rs += Math.PI * 2;
//            }
//            return Rs;
//        }
//        public double Rose(double Fi)
//        {
//            string Json;

//            using (StreamReader sr = File.OpenText(InputFile))
//            {
//                Json = sr.ReadToEnd();
//            }

//            dynamic Obj = JsonConvert.DeserializeObject(Json);
//            dynamic RoseW = Obj;

//            if (Fi < 0)
//            {
//                Fi += 2 * Math.PI;
//            }
//            if (Fi > 2 * Math.PI)
//            {
//                Fi -= 2 * Math.PI;
//            }

//            double DlAng = 2 * Math.PI / 8;
//            List<double> Ff = new List<double>();

//            for (int i = 1; i < 9; i++)
//            {
//                Ff.Add(i * DlAng);
//            }

//            string[] Sw = new string[] { "W", "NW", "N", "NE", "E", "SE", "S", "SW" };
//            List<int> SortRW = new List<int>();

//            foreach (string key in Sw)
//            {
//                int value;
//                if (RoseW.TryGetValue(key, out value))
//                {
//                    SortRW.Add(value);
//                }
//            }

//            double Rs = 0.0;

//            for (int i = 6; i >= 0; i--)
//            {
//                double F = (i + 1) * DlAng;
//                if (Fi <= F)
//                {
//                    double Rs1 = (SortRW[i] * (Fi - (F - DlAng)) + SortRW[(i + 1) % 8] * (F - Fi)) / DlAng;
//                    Rs = Rs1;
//                    break;
//                }
//            }

//            return Rs;
//        }
//    }
//}
