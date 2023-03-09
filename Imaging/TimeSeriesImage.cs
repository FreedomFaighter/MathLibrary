using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
namespace Imaging
{
    public class TimeSeriesImage : IDisposable
    {
        Bitmap timeSeriesBitmap;
        int rectangleScaleForY = 10;
        int rectangleScaleForX = 15;
        public Bitmap TimeSeriesBitmap
        {
            get { return timeSeriesBitmap; }
        }

        public TimeSeriesImage(double[] timeSeries, int widthSeperatingImage, int heightScaleOfImage, Color backgroundColor, Color rectangleColor, Brush backgroundFill)
        {
            double max = timeSeries.Max();

            double min = timeSeries.Min();

            int width = timeSeries.Length * widthSeperatingImage;

            int height;

            if (min > ((double)Decimal.Zero))
                height = (int)(max * heightScaleOfImage);
            else if (max < ((double)Decimal.Zero))
                height = (int)(min * -heightScaleOfImage);
            else
            {
                height = (int)((max - min) * heightScaleOfImage);
            }

            timeSeriesBitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(timeSeriesBitmap))
            {
                Pen black = new Pen(backgroundColor);
                Pen green = new Pen(rectangleColor);
                g.DrawRectangle(black, new Rectangle(0, 0, width, height));
                g.FillRectangle(backgroundFill, new Rectangle(1, 1, width - 1, height - 1));
                int XaxisYcoordinate = (int)(height * 0.5);
                g.DrawLine(black, new Point(0, XaxisYcoordinate), new Point(width, XaxisYcoordinate));
                for (int i = 0; i < timeSeries.Length; i++)
                {
                    g.DrawRectangle(green, new Rectangle(i * rectangleScaleForX + 1, XaxisYcoordinate - (int)System.Math.Round(timeSeries[i] * rectangleScaleForY), 2, 2));
                }
                for (int i = 0; i < timeSeries.Length - 1; i++)
                {
                    g.DrawLine(black, new PointF(i * rectangleScaleForX + 2, XaxisYcoordinate - (int)System.Math.Round(timeSeries[i] * rectangleScaleForY))
                                                 , new PointF((i + 1) * rectangleScaleForX + 2, XaxisYcoordinate - (int)System.Math.Round(timeSeries[i + 1] * rectangleScaleForY)));
                }
            }
        }

        public void SaveToFile(string fileName, ImageFormat imageFormat)
        {
            using(System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Append))
            {
                timeSeriesBitmap.Save(fs, imageFormat);
            }
        }

        #region IDisposable implementation

        public void Dispose()
        {
            timeSeriesBitmap.Dispose();
        }

        #endregion
    }
}

