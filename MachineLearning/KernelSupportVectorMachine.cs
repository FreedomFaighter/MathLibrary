using System;

namespace MachineLearning
{
	[Serializable]
	public class KernelSupportVectorMachine : SupportVectorMachine
	{
		private IKernel kernel;

		public KernelSupportVectorMachine (IKernel kernel, int inputs):base(inputs)
		{
			if (kernel == null)
				throw new ArgumentNullException ("kernel");

			this.kernel = kernel;
		}

		public IKernel Kernel
		{
			get { return kernel;}
			set{ kernel = value;}
		}

		public override double Compute (double[] inputs)
		{
			double s = Threshold;

			for (int i = 0; i<SupportVectors.Length; i++)
				s += Weights [i] * kernel.Function (SupportVectors [i], inputs);

			return s;
		}
	}
}

