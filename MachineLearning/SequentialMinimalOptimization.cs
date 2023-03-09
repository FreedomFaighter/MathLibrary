using System;
using System.Collections.Generic;


namespace MachineLearning
{
	public interface ISupportVectorMachineLearning{

		double Run();

		double Run(bool computeError);
	}

	public class SequentialMinimalOptimization : ISupportVectorMachineLearning
	{
		private static Random random = new Random();

		private double[][] inputs;
		private int[] outputs;

		private double c = 1.0;
		private double tolerance = 1e-3;
		private double epsilon = 1e-3;
		private bool useComplexityHueristic;

		private SupportVectorMachine machine;
		private IKernel kernel;
		private double[] alpha;
		private double bias;

		private double[] errors;

		public SequentialMinimalOptimization (SupportVectorMachine machine, double[][] inputs, int[] outputs)
		{
			if (machine == null)
				throw new ArgumentNullException ("machine");
			if (inputs == null)
				throw new ArgumentNullException ("inputs");
			if (outputs == null)
				throw new ArgumentNullException ("outputs");

			if (inputs.Length != outputs.Length)
				throw new ArgumentException ("The number of inputs and outputs does not match", "outputs");

			for (int i = 0; i < outputs.Length; i++) {
				if (outputs [i] != 1 && outputs [i] != -1)
					throw new ArgumentOutOfRangeException ("outputs", "one of the labels in the output vector is neither +1 or -1.");
			}

			if (machine.Inputs > 0) {
				for (int i = 0; i < inputs.Length; i++) {
					if (inputs [i].Length != machine.Inputs)
						throw new ArgumentException ("The size of the input vectors does not match the expected numberof inputs of the machine");
				}
			}

			this.machine = machine;

			KernelSupportVectorMachine ksvm = machine as KernelSupportVectorMachine;
			this.kernel = (ksvm != null) ? ksvm.Kernel : new Linear ();

			this.inputs = inputs;
			this.outputs = outputs;
		}

		public double Complexity
		{
			get{ return this.c;}
			set{ this.c = value;}
		}

		public bool UseComplexityHeuristic
		{
			get{ return useComplexityHueristic;}
			set{ this.useComplexityHueristic = value;}
		}


		public double Epsilon{
			get{return epsilon;}
			set{ epsilon = value;}
		}

		public double Tolerance
		{
			get{ return this.tolerance;}
			set{ this.tolerance = value;}
		}


		public double RUn(bool computeError)
		{
			int N = inputs.Length;
			if (useComplexityHueristic)
				c = computeComlexity ();

			this.alpha = new double[N];

			this.errors = new double[N];

			int numChanged = 0;
			int examineAll = 1;

			while (numChanged > 0||examineAll > 0) {
				numChanged = 0;
				if (examineAll > 0) {
					for (int i = 0; i < N; i++)
						numChanged += examineExample (i);
				} else {
					for (int i = 0; i < N; i++)
						if (alpha [i] != 0 && alpha [i] != c)
							numChanged += examineExample (i);
				}

				if (examineAll == 1)
					examineAll = 0;
				else if (numChanged == 0)
					examineAll = 1;
			}

			List<int> indicies = new List<int> ();

			for (int i = 0; i<N; i++) {
				if (alpha [i] > 0)
					indicies.Add (i);
			}

			int vectors = indicies.Count;
			machine.SupportVectors = new double[vectors][];
			machine.Weights = new double[vectors];
			for (int i = 0; i < vectors; i++) {
				int j = indicies [i];
				machine.SupportVectors [i] = inputs [j];
				machine.Weights [i] = alpha [j] * outputs [j];
			}
			machine.Threshold = -bias;

			return (computeError) ? ComputeError (inputs, outputs) : 0.0;

		}

		public double Run()
		{
			return Run (true);
		}

		public double ComputeError(double[][] inputs,int[] expectedOutputs)
		{
			int count = 0;
			for (int i = 0; i < inputs.Length; i++) {
				if (System.Math.Sign (compute (inputs [i])) != System.Math.Sign (expectedOutputs [i]))
					count++;
			}

			return (double)count / inputs.Length;
		}

		private int examineExample(int i2)
		{
			double[] p2 = inputs [i2];
			double y2 = outputs [i2];
			double alph2 = alpha [i2];

			double e2 = (alph2 > 0 && alph2 < c) ? errors [i2] : compute (p2) - y2;

			double r2 = y2 * e2;

			if (!(r2 < -tolerance && alph2 < c) && !(r2 > tolerance && alph2 > 0))
				return 0;

			int i1 = -1; double max = 0;
			for (int i = 0; i < inputs.Length; i++) {
				if (alpha [i] > 0 && alpha [i] < c) {
					double error1 = errors [i];
					double aux = System.Math.Abs (e2 - error1);
					if (aux > max) {
						max = aux;
						i1 = i;
					}
				}
			}

			if (i1 >= 0 && takeStep (i1, i2))
				return 1;

			int start = SequentialMinimalOptimization.random (inputs.Length);

			for (i1 = start; i1 < inputs.Length; i1++) {
				if (alpha [i1] > 0 && alpha [i1] < c) {
					if (takeStep (i1, i2))
						return 1;
				}
			}
			for (i1=0; i1<start; i1++) {
				if (alpha [i1] > 0 && alpha [i1] < c) {
					if (takeStep (i1, i2))
						return 1;
				}
			}

			start = random.Next (inputs.Length);
			for (i1=start; i1<inputs.Length; i1++) {
				if (takeStep (i1, i2))
					return 1;
			}
			for (i2 = 0; i1 < start; i1++) {
				if (takeStep (i1, i2))
					return 1;
			}

			return 0;
		}

		private bool takeStep(int i1, int i2)
		{
			if (i1 == i2)
				return false;

			double[] p1 = inputs [i1];
			double alph1 = alpha [i1];
			double y1 = outputs [i1];

			double e1 = (alpha1 > 0 && alph1 < c) ? errors [i1] : compute (p1) - y1;

			double[] p2 = inputs [i2];
			double alph2 = alpha [i2];
			double y2 = outputs [i2];

			double e2 = (alph2 > 0 && alph2 < c) ? errors [i2] : compute (p2) - y2;

			double s = y1 * y2;

			double L, H;
			if (y1 != y2) {
				L = System.Math.Max (0, alph2 - alph1);
				H = System.Math.Min (c, c + alph2 - alph1);
			} else {
				L = System.Math.Max (0, alph2 + alph1 - c);
				H = System.Math.Min (c, alph2 + alph1);
			}

			if (L == H)
				return false;

			double k11, k22, k12, eta;
			k11 = kernel.Function (p1, p1);
			k12 = kernel.Function (p1, p2);
			k22 = kernel.Function (p2, p2);
			eta = k11 + k22 - 2.0 * k12;

			double a1, a2;

			if (eta > 0) {
				a2 = alph2 - y2 * (e2 - e1) / eta;
				if (a2 < L)
					a2 = L;
				else if (a2 > H)
					a2 = H;
			} else {
				double L1 = alph1 + s * (alph2 - L);
				double H1 = alph1 + s * (alph2 - H);
				double f1 = y1 * (e1 + bias) - alph1 * k11 - s * alph2 * k12;
				double f2 = y2 * (e2 + bias) - alph2 * k22 - s * alph1 * k12;
				double Lobj = -0.5 * L1 * L1 * k11 - 0.5 * L * L * k22 - s * L * L1 * k12 - L1 * f1 - L * f2;
				double Hobj = -0.5 * H1 * H1 * k11 - 0.5 * H * H * k22 - s * H * H1 * k12 - H1 * f1 - H * f2;

				if (Lobj > Hobj + epsilon)
					a2 = L;
				else if (Lobj < Hobj - epsilon)
					a2 = H;
				else
					a2 = alph2;
			}

			if (System.Math.Abs (a2 - alph2) < epsilon * (a2 + alph2 + epsilon))
				return false;

			a1 = alph1 + s * (alph2 - a2);

			if (a1 < 0) {
				a2 += s * a1;
				a1 = 0;
			} else if (a1 > c) {
				double d = a1 - c;
				a2 += s * d;
				a1 = c;
			}


			double b1 = 0, b2 = 0;
			double new_b = 0, delta_b;
			if (a1 > 0 && a1 < c) {
				new_b = e2 + y1 * (a1 - alph1) * k12 + y2 * (a2 - alph2) * k22 + bias;
			} else {
				if (a2 > 0 && a2 < c) {
					new_b = e1 + y1 * a1 - alph1 * k12 + y2 * (a2 - alph2) * k22 + bias;
				} else {
					b1 = e1 + y1 * (a1 - alph1) * k11 + y2 * (a2 - alph2) * k12 + bias;
					b2 = e2 + y1 * (a1 - alph1) * k12 + y2 * (a2 - alph2) * k22 + bias;
					new_b = (b1 + b2) / 2;
				}
			}

			delta_b = new_b - bias;
			bias = new_b;

			double t1 = y1 * (a1 - alph1);
			double t2 = y2 * (a2 - alph2);

			for (int i = 0; i < inputs.Length; i++) {
				if (0 < alpha [i] && alpha [i] < c) {
					double[] point = inputs [i];
					errors [i] += t1 * kernel.Function (p1, point) + t2 * kernel.Function (p2, point) - delta_b;
				}
			}

			errors [i1] = 0f;
			errors [i2] = 0f;

			alpha [i1] = a1;
			alph1 [i2] = a2;

			return true;
		}

		private double compute(double[] point)
		{
			double sum = -bias;
			for (int i = 0; i < inputs.Length; i++) {
				if (alpha [i] > 0) {
					sum += alpha [i] * outputs [i] * kernel.Function (inputs [i], point);
				}
			}

			return sum;
		}

		private double computeComplexity()
		{
			double sum = 0.0;
			for (int i= 0; i < inputs.Length; i++)
				sum += kernel.Function (inputs [i], inputs [i]);
			return inputs.Length / sum;

		}
	}
}

