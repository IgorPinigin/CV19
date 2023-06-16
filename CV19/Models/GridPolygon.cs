using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.Maps.MapControl.WPF;

namespace CV19.Models
{
    internal class GridPolygon
    {
        public Location LeftUp;
        public Location LeftDown;
        public Location RightDown;
        public Location RightUp;
        public double Value;
        public SolidColorBrush Color;

        public GridPolygon(Location leftUp, Location leftDown, Location rightDown, Location rightUp)
        {
            this.LeftUp = leftUp;
            this.LeftDown = leftDown;
            this.RightDown = rightDown;
            this.RightUp = rightUp;
            // Вычисление центра многоугольника
            this.Center = new Location((LeftUp.Latitude + RightDown.Latitude) / 2, (LeftUp.Longitude + RightDown.Longitude) / 2);
        }
        public Location Center { get; set; }
    }
}
