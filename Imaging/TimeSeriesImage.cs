using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
namespace Imaging
{
    public class TimeSeriesImage : IDisposable
    {
        Bitmap thisBitmap;

        public Bitmap ThisBitmap
        {
            get { return this.thisBitmap; }
        }

        public TimeSeriesImage(double[] timeSeries)
        {
            double max = timeSeries.Max();
            double min = timeSeries.Min();
            int ticksUpward = (int)(max / 10) + 1;
            int ticksDownward = (int)(min / 10) + 1;

            int width = timeSeries.Length * 15;
            int height;
            if (min > 0)
                height = (int)(max * 15);
            else if (max < 0)
                height = (int)(min * -15);
            else
            {
                height = (int)((max - min) * 15);
            }
            this.thisBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(this.thisBitmap))
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

        public void SaveToFile(string fileName)
        {
            this.thisBitmap.Save(fileName, ImageFormat.Png);
        }

        #region IDisposable implementation

        public void Dispose()
        {
            this.thisBitmap.Dispose();
            this.thisBitmap = null;
        }

        #endregion
    }
}

