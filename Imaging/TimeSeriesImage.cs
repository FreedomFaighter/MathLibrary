using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
namespace Imaging
{
    public class TimeSeriesImage : IDisposable
    {
        Bitmap timeSeriesBitmap;

        public Bitmap TimeSeriesBitmap
        {
            get { return timeSeriesBitmap; }
        }

        public TimeSeriesImage(double[] timeSeries, int widthSeperatingImage, int heightOfImage)
        {
            double max = timeSeries.Max();
            double min = timeSeries.Min();

            int width = timeSeries.Length * widthSeperatingImage;
            int height;
            if (min > 0)
                height = (int)(max * heightOfImage);
            else if (max < 0)
                height = (int)(min * -heightOfImage);
            else
            {
                height = (int)((max - min) * heightOfImage);
            }
            timeSeriesBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(timeSeriesBitmap))
            {
                Pen black = new Pen(Color.Black);
                Pen green = new Pen(Color.Green);
                g.DrawRectangle(black, new Rectangle(0, 0, width, height));
                g.FillRectangle(Brushes.White, new Rectangle(1, 1, width - 1, height - 1));
                int XaxisYcoordinate = (int)(height * 0.5);
                g.DrawLine(black, new Point(0, XaxisYcoordinate), new Point(width, XaxisYcoordinate));
                for (int i = 0; i < timeSeries.Length; i++)
                {
                    g.DrawRectangle(green, new Rectangle(i * 15 + 1, XaxisYcoordinate - (int)System.Math.Round(timeSeries[i] * 10), 2, 2));
                }
                for (int i = 0; i < timeSeries.Length - 1; i++)
                {
                    g.DrawLine(black, new PointF(i * 15 + 2, XaxisYcoordinate - (int)System.Math.Round(timeSeries[i] * 10))
                                                 , new PointF((i + 1) * 15 + 2, XaxisYcoordinate - (int)System.Math.Round(timeSeries[i + 1] * 10)));
                }

            }
        }

        public void SaveToFile(string fileName, ImageFormat imageFormat)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Append))
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

