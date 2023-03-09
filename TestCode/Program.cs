using System;
using Statistics.TimeSeries;
using Imaging;

namespace TestCode
{
	class MainClass
	{
		static double[] testData = new double[]{-5.1,5.0,4.0,-3.5,7,-8.0,1.7,3,7,-1.1,3,8,5.0,5.1,4.1,-3.4,6.9,8.1,-1.6,3.1,7.1,1.2,-2.9,7.9};
		public static void Main (string[] args)
		{
			TimeSeries ts = new TimeSeries (MainClass.testData);
			CorrelationImage ci1 = new CorrelationImage (ts.SampleAutocorrelationFunction);
			CorrelationImage ci2 = new CorrelationImage (ts.SamplePartialAutocorrelationFunction);
			TimeSeriesImage tsi = new TimeSeriesImage (testData);
			for (int i = 1; i < testData.Length; i++) {
				System.Console.WriteLine ("Lag: {0}, Value: {1}, ACF:{2}, PACF:{3}", i, testData [i], ts.SampleAutocorrelationFunction [i]
				                          , ts.SamplePartialAutocorrelationFunction[i]);
			}
			ci1.SaveToFile(System.Environment.CurrentDirectory + "test1.png");
			ci2.SaveToFile(System.Environment.CurrentDirectory + "test2.png");
			tsi.SaveToFile (System.Environment.CurrentDirectory + "tsTest.png");
		}
	}
}
