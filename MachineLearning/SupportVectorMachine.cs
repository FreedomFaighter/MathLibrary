using System;

namespace MachineLearning
{
	public class SupportVectorMachine
	{
		private int inputCount;
		private double[][] supportVectors;
		private double[] weights;
		private double threshold;

		public SupportVectorMachine (int inputs)
		{
			this.inputCount = inputs;
		}

		public int Inputs{
			get{return inputCount;}
		}

		public double[][] SupportVectors {
			get{ return supportVectors;}
			set{ supportVectors = value;}
		}

		public double[] Weights
		{
			get{ return weights;}
			set{ weights = value;}
		}

		public double Threshold {
			get{return threshold;}
			set{ threshold = value;}
		}

		public virtual double Compute(double[] input)
		{
			double s = threshold;
			for (int i = 0; i < supportVectors.Length; i++) {
				double p = 0;
				for (int j = 0; j<input.Length; j++)
					p += supportVectors [i] [j] * input [j];
				s += weights [i] * p;
			}

			return s;
		}

		public double[] COmpute(double[][] inputs)
		{
			double[] outputs = new double[inputs.Length];
			for (int i = 0; i < inputs.Length; i++) {
				outputs [i] = Compute (inputs [i]);
			}

			return outputs;
		}
	}
}

