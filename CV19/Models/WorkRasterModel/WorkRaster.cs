using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSGeo.GDAL;

namespace CV19.Models.WorkRasterModel
{
    internal class WorkRaster
        
    {
        //public static double Non_dt { get; private set; }

        public void ToMulti(string PathFs, string PathOutput, int CountLay)
        {
           
            string[] BandsF = Directory.GetFiles(PathFs);

            string Fb1 =BandsF.First(el => el.Contains("B1"));
            Dataset Band1 = Gdal.Open(Fb1, Access.GA_ReadOnly);

            double[] Geotransform = new double[6];
            Band1.GetGeoTransform(Geotransform);
            string Projection = Band1.GetProjection();

            int XSize = Band1.RasterXSize;
            int YSize = Band1.RasterYSize;

            string FOut = Path.GetFileNameWithoutExtension(BandsF[0]).Split('_').First();

            Driver Drv = Gdal.GetDriverByName("GTiff");
            Dataset GeoF = Drv.Create(Path.Combine(PathOutput, FOut + ".tif"), XSize, YSize, CountLay, OSGeo.GDAL.DataType.GDT_Float32, new string[] { "COMPRESS=LZW" });

            GeoF.SetGeoTransform(Geotransform);
            GeoF.SetProjection(Projection);

            Band Band = Band1.GetRasterBand(1);
            float[] BandData = new float[XSize * YSize];
            Band.ReadRaster(0, 0, XSize, YSize, BandData, XSize, YSize, 0, 0);
            Band OutBand = GeoF.GetRasterBand(1);
            OutBand.WriteRaster(0, 0, XSize, YSize, BandData, XSize, YSize, 0, 0);
            OutBand.SetNoDataValue(-999.0);

            for (int i = 1; i < CountLay; i++)
            {
                string F = BandsF.First(el => el.Contains("B" + (i + 1)));
                Dataset BandI = Gdal.Open(F, Access.GA_ReadOnly);

                Band = BandI.GetRasterBand(1);
                BandData = new float[XSize * YSize];
                Band.ReadRaster(0, 0, XSize, YSize, BandData, XSize, YSize, 0, 0);
                OutBand = GeoF.GetRasterBand(i + 1);
                OutBand.WriteRaster(0, 0, XSize, YSize, BandData, XSize, YSize, 0, 0);
                OutBand.SetNoDataValue(-999.0);

                BandI.Dispose();
            }

            GeoF.Dispose();
            Band1.Dispose();
        }

    }
}
