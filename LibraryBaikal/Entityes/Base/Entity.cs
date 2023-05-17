using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryBaikal.Entityes.Base
{
    public class Entity
    {
        public int Id { get; set; }

    }
    public class Town : Entity 
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }



}
