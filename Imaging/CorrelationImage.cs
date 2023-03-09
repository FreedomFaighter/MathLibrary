using System;
using System.Drawing;
namespace Imaging
{
	public class CorrelationImage
	{
		double[] correlationFunction;
		double[] confidenceBounds;
		Graphics thisGraphics;
		Bitmap thisBitmap;
		public CorrelationImage (double[] correlationFunction, double confBoundsNumer)
		{
			thisBitmap = new Bitmap(correlationFunction.Length * 21, 201);
			this.correlationFunction = correlationFunction;
			confidenceBounds = new double[2];
			this.confidenceBounds [0] = confBoundsNumer / correlationFunction.Length;
			this.confidenceBounds [1] = -this.confidenceBounds [0];
			this.thisGraphics = Graphics.FromImage (thisBitmap);
			StringFormat textFormat = new StringFormat ();
			textFormat.Alignment = StringAlignment.Near;
			//draw base image
			Pen black = new Pen (new SolidBrush (Color.Black));
			Pen green = new Pen (new SolidBrush (Color.Green));

			thisGraphics.DrawRectangle (black, new Rectangle (0, 0, this.correlationFunction.Length * 21, 201));
			thisGraphics.FillRectangle(Brushes.White, new Rectangle (1, 1, this.correlationFunction.Length * 21 - 1, 200));

			thisGraphics.DrawLine (black, new PointF (1, 100), new PointF (this.correlationFunction.Length * 21 - 1, 100));
			//draw correlation function
			for (int i = 0; i < correlationFunction.Length; i++) {
				if (System.Math.Abs (correlationFunction [i]) > confidenceBounds [0] && i != 0) {
					thisGraphics.DrawLine (green, new Point (i * 21 - 10, 100), new Point (i * 21 - 10, (int)(-correlationFunction [i] * 100 + 100)));
					thisGraphics.DrawString (string.Format ("{0}", i)
					                         , new Font ("Tahoma", 11), Brushes.Green
					                         , new PointF (i * 21 - 15, correlationFunction [i] > 0 ? 110 : 70), textFormat); 
				} else {
					thisGraphics.DrawLine (black, new Point (i * 21 - 10, 100), new Point (i * 21 - 10, (int)(-correlationFunction [i] * 100 + 100)));
				}
			}

			thisGraphics.DrawLine (green, new Point (1, (int)(this.confidenceBounds [0] * 100 + 100)), new Point (this.correlationFunction.Length * 21 - 1, (int)(this.confidenceBounds [0] * 100 + 100)));
			thisGraphics.DrawLine (green, new Point (1, (int)(this.confidenceBounds [1] * 100 + 100)), new Point (this.correlationFunction.Length * 21 - 1, (int)(this.confidenceBounds [1] * 100 + 100)));


			thisGraphics.Dispose ();
			if (thisBitmap.Size.Width > 48 * 21)
				thisBitmap = (Bitmap)resizeImage (thisBitmap, new Size (48 * 21, 201));

		}

		public double[] CorrelationFunction
		{
			get{ return this.correlationFunction;}
			set{ this.correlationFunction = value;}
		}

		public Bitmap ThisBitmap
		{
			get{ return this.thisBitmap;}
		}

		public void SaveToFile(string FileName)
		{
			thisBitmap.Save (FileName, System.Drawing.Imaging.ImageFormat.Png);
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

			Bitmap b = new Bitmap (destWidth, destHeight);
			Graphics g = Graphics.FromImage ((Image)b);
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;

			g.DrawImage (imgToResize, 0, 0, destWidth, destHeight);
			g.Dispose ();

			return (Image)b;
		}
	}
}

