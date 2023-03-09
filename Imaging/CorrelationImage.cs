using System.Drawing;
using System.Drawing.Imaging;

namespace Imaging
{
    public class CorrelationImage
    {
        double[] correlationFunction;
        double[] confidenceBounds;
        Graphics correlationImageGraphics;
        Bitmap correlationImageBitmap;
        Font imageFont = new("Arial", 11);
        PointF imagePointF;
        public CorrelationImage(double[] correlationFunction, double confBoundsNumer, int imageWidth, int imageHeight, Color background, Brush backgroundFill, Color lineColor)
        {
            correlationImageBitmap = new Bitmap(correlationFunction.Length * imageWidth, imageHeight);
            this.correlationFunction = correlationFunction;
            confidenceBounds = new double[2];
            this.confidenceBounds[0] = confBoundsNumer / correlationFunction.Length;
            this.confidenceBounds[1] = -this.confidenceBounds[0];
            this.correlationImageGraphics = Graphics.FromImage(correlationImageBitmap);
            StringFormat textFormat = new StringFormat();
            textFormat.Alignment = StringAlignment.Near;
            //draw base image
            Pen backgroundPen = new Pen(new SolidBrush(background));
            Pen linePen = new Pen(new SolidBrush(lineColor));

            correlationImageGraphics.DrawRectangle(backgroundPen, new Rectangle(0, 0, this.correlationFunction.Length * imageWidth, 201));
            correlationImageGraphics.FillRectangle(backgroundFill, new Rectangle(1, 1, this.correlationFunction.Length * imageWidth - 1, 200));
            correlationImageGraphics.DrawLine(backgroundPen, new PointF(1, 100), new PointF(this.correlationFunction.Length * imageWidth - 1, 100));
            //draw correlation function

            for (int i = 0; i < correlationFunction.Length; i++)
            {
                if (System.Math.Abs(correlationFunction[i]) > confidenceBounds[0] && i != 0)
                {
                    imagePointF = new PointF(i * imageWidth - 15, correlationFunction[i] > 0 ? 110 : 70);
                    correlationImageGraphics.DrawLine(linePen, new Point(i * imageWidth - 10, 100), new Point(i * imageWidth - 10, (int)((-correlationFunction[i] * 1) * 100)));
                    correlationImageGraphics.DrawString($"{i}"
                                             , imageFont, linePen.Brush
                                             , imagePointF, textFormat);
                }
                else
                {
                    correlationImageGraphics.DrawLine(backgroundPen, new Point(i * imageWidth - 10, 100), new Point(i * imageWidth - 10, (int)((-correlationFunction[i] * 1) * 100)));
                }
            }

            correlationImageGraphics.DrawLine(linePen, new Point(1, (int)((this.confidenceBounds[0] + 1) * 100)), new Point(this.correlationFunction.Length * imageWidth - 1, (int)((this.confidenceBounds[0] + 1) * 100)));
            correlationImageGraphics.DrawLine(linePen, new Point(1, (int)((this.confidenceBounds[1] + 1) * 100)), new Point(this.correlationFunction.Length * imageWidth - 1, (int)((this.confidenceBounds[1] * 1) * 100)));


            correlationImageGraphics.Dispose();
            if (correlationImageBitmap.Size.Width > 48 * imageWidth)
                correlationImageBitmap = (Bitmap)resizeImage(correlationImageBitmap, new Size(48 * imageWidth, 201));

        }

        public double[] CorrelationFunction
        {
            get { return this.correlationFunction; }
            set { this.correlationFunction = value; }
        }

        public Bitmap ThisBitmap
        {
            get { return this.correlationImageBitmap; }
        }

        public void SaveToFile(string fileName, ImageFormat imageFormat)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Append))
            {
                correlationImageBitmap.Save(fs, imageFormat);
            }
        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent;
            float nPercentW = ((float)size.Width / (float)sourceWidth), nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            using (Bitmap b = new Bitmap(destWidth, destHeight))
            using (Graphics g = Graphics.FromImage((Image)b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                return (Image)b;
            }
        }
    }
}

