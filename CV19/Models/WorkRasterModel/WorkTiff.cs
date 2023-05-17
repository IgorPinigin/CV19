using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using OSGeo.GDAL;

namespace CV19.Models.WorkRasterModel
{
    public static class WorkTiff
    {
        private static float _non_dt = -999f;

        public static void ToMulti(string path_fs, string path_output, int count_lay)
        {
            string[] bands_f = Directory.GetFiles(path_fs);

            string fb_1 = bands_f.FirstOrDefault(el => el.Contains("B1"));
            Dataset band_1 = Gdal.Open(fb_1, Access.GA_ReadOnly);

            double[] geotransform = new double[6];
            band_1.GetGeoTransform(geotransform);
            string projection = band_1.GetProjectionRef();

            int x_size = band_1.RasterXSize;
            int y_size = band_1.RasterYSize;

            string f_out = System.IO.Path.GetFileNameWithoutExtension(bands_f[0]).Split('_')[0];

            Driver drv = Gdal.GetDriverByName("GTiff");
            Dataset geo_f = drv.Create(System.IO.Path.Combine(path_output, $"{f_out}.tif"), x_size, y_size, count_lay, DataType.GDT_Float32, new string[] { "COMPRESS=LZW" });

            geo_f.SetGeoTransform(geotransform);
            geo_f.SetProjection(projection);

            Band band = geo_f.GetRasterBand(1);
            byte[] data = new byte[x_size * y_size * sizeof(float)];
            band_1.GetRasterBand(1).ReadRaster(0, 0, x_size, y_size, data, x_size, y_size, 0, 0);
            band.WriteRaster(0, 0, x_size, y_size, data, x_size, y_size, 0, 0);
            band.SetNoDataValue(_non_dt);

            for (int i = 1; i < count_lay; i++)
            {
                string f = bands_f.FirstOrDefault(el => el.Contains($"B{i + 1}"));
                Dataset band_i = Gdal.Open(f, Access.GA_ReadOnly);

                band = geo_f.GetRasterBand(i + 1);
                data = new byte[x_size * y_size * sizeof(float)];
                band_i.GetRasterBand(1).ReadRaster(0, 0, x_size, y_size, data, x_size, y_size, 0, 0);
                band.WriteRaster(0, 0, x_size, y_size, data, x_size, y_size, 0, 0);
                band.SetNoDataValue(_non_dt);

                band_i.Dispose();
            }

            geo_f.FlushCache();
            geo_f.Dispose();
            band_1.Dispose();
        }
        public static void ToMultiAll(string pathFs, string pathOutput)
        {
            string[] bandsF = Directory.GetFiles(pathFs)
                .Where(f => (f.EndsWith(".tif") || f.EndsWith(".TIF")) && !f.Contains("B8"))
                .OrderBy(f => f.Length)
                .ToArray();

            Console.WriteLine(string.Join(", ", bandsF));

            string fb1 = bandsF.FirstOrDefault(f => f.Contains("B1"));
            Dataset band1 = Gdal.Open(System.IO.Path.Combine(pathFs, fb1), Access.GA_ReadOnly);

            double[] geotransform = new double[6];
            band1.GetGeoTransform(geotransform);
            string projection = band1.GetProjection();

            int xSize = band1.RasterXSize;
            int ySize = band1.RasterYSize;

            Driver drv = Gdal.GetDriverByName("GTiff");
            Dataset geoF = drv.Create(pathOutput, xSize, ySize, bandsF.Length, DataType.GDT_Float32, options: new[] { "COMPRESS=LZW" });

            geoF.SetGeoTransform(geotransform);
            geoF.SetProjection(projection);

            double[] band1Array = new double[xSize * ySize];
            band1.GetRasterBand(1).ReadRaster(0, 0, xSize, ySize, band1Array, xSize, ySize, 0, 0);
            float[] band1FloatArray = Array.ConvertAll(band1Array, item => (float)item);
            Band geoFBand1 = geoF.GetRasterBand(1);
            geoFBand1.WriteRaster(0, 0, xSize, ySize, band1FloatArray, xSize, ySize, 0, 0);
            geoFBand1.SetNoDataValue(WorkTiff._non_dt);

            for (int i = 1; i < bandsF.Length; i++)
            {
                Dataset band = Gdal.Open(System.IO.Path.Combine(pathFs, bandsF[i]), Access.GA_ReadOnly);
                double[] bandArray = new double[xSize * ySize];
                band.GetRasterBand(1).ReadRaster(0, 0, xSize, ySize, bandArray, xSize, ySize, 0, 0);
                float[] bandFloatArray = Array.ConvertAll(bandArray, item => (float)item);
                Band geoFBand = geoF.GetRasterBand(i + 1);
                geoFBand.WriteRaster(0, 0, xSize, ySize, bandFloatArray, xSize, ySize, 0, 0);
                geoFBand.SetNoDataValue(WorkTiff._non_dt);
            }
        }
        //public static float[,] GetIngx(Func<float[][], float[,]> func, string path_b1, string path_b2, bool do_coars = false)
        //{
        //    using (var _b1 = Gdal.Open(path_b1, Access.GA_ReadOnly))
        //    {
        //        if (_b1 == null)
        //            throw new IOException("Couldn't open file : " + path_b1);

        //        var b1 = _b1.GetRasterBand(1).ReadAsArray().Convert<float>();

        //        using (var _b2 = Gdal.Open(path_b2, Access.GA_ReadOnly))
        //        {
        //            if (_b2 == null)
        //                throw new IOException("Couldn't open file : " + path_b2);

        //            var b2 = _b2.GetRasterBand(1).ReadAsArray().Convert<float>();

        //            Func<float[,], float[,]> _coarsen = (val) =>
        //            {
        //                var group_dim = 2;
        //                var old_dim = val.Shape();
        //                var new_dim = new[] { old_dim[0] / group_dim, old_dim[1] / group_dim };

        //                return new float[new_dim[0], new_dim[1]].Select2D((i, j) =>
        //                    val.Slice2D(i * group_dim, j * group_dim, i * group_dim + group_dim, j * group_dim + group_dim).Average());
        //            };

        //            float[,] b_coarsen;
        //            if (do_coars)
        //            {
        //                b_coarsen = _coarsen(b1);
        //                var geotransform = _b1.GetGeoTransform();
        //                var projection = _b1.GetProjection();
        //                var path_out = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path_b1), "vg_sentinel.tif");

        //                WorkTiff.ToTif(b_coarsen, geotransform, projection, path_out);
        //            }
        //            else
        //                b_coarsen = b1;

        //            return func(new[] { b_coarsen, b2 });
        //        }
        //    }
        //}
    }
}
