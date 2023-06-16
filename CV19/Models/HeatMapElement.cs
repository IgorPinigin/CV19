using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CV19.Models
{
    public class HeatMapElement
    {
        private double minValue;
        public double MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        private double maxValue;
        public double MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        private string unit;
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
    }
}
