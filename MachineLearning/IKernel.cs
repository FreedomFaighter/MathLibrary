using System;

namespace MachineLearning
{
	public interface IKernel
	{
		double Function(double[] x, double[] y);
	}
}

