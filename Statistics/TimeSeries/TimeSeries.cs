using MathLibrary.LinearAlgebra;
using System.Linq;

namespace Statistics.TimeSeries
{
    public class TimeSeries : System.Collections.Generic.List<double>
    {
        

        public TimeSeries(double[] v)
        {
            if (v.Length < 1)
                throw new DivideByZeroException("Using population variance and must have at least 1 observation");
            this.AddRange(v);
        }

        public double[] ComputeAutocovarianceFunction()
        {
            double[] sampleAutocovarianceFunction = new double[this.Count];
            for (int h = 0; h < this.ToArray().Length; h++)
            {
                sampleAutocovarianceFunction[h] = this.ToArray().Take(this.Count - h).Select((x, t) =>
                {
                    return (this[t + h] - this.Average()) * (this[t] - this.ToArray().Variance(true));
                }).Sum() / this.Count;
            }

            Matrix covarianceMatrix = new Matrix(sampleAutocovarianceFunction.Length, sampleAutocovarianceFunction.Length);
            for (int i = 0; i < covarianceMatrix.Values.GetLength(0); i++)
            {
                for (int j = 0; j < covarianceMatrix.Values.GetLength(1); j++)
                {
                    covarianceMatrix[i, j] = sampleAutocovarianceFunction[System.Math.Abs(i - j)];
                }
            }

            return sampleAutocovarianceFunction;
        }

        public double[] ComputeSampleAutocorrelationFunction()
        {
            double[] sampleAutocorrelationFunction = new double[this.Count];
            double[] sampleAutocovarianceFunction = this.ComputeAutocovarianceFunction();

            for (int h = 0; h < sampleAutocovarianceFunction.Length; h++)
            {
                sampleAutocorrelationFunction[h] = sampleAutocovarianceFunction[h] / sampleAutocovarianceFunction[0];
            }

            Matrix autocorrelationMatrix = new Matrix(sampleAutocorrelationFunction.Length, sampleAutocorrelationFunction.Length);
            for (int i = 0; i < autocorrelationMatrix.Values.GetLength(0); i++)
            {
                for (int j = 0; j < autocorrelationMatrix.Values.GetLength(1); j++)
                {
                    autocorrelationMatrix[i, j] = sampleAutocorrelationFunction[System.Math.Abs(i - j)];
                }
            }

            return sampleAutocorrelationFunction;
        }

        public Matrix ComputeSamplePartialAutocorrelationMatrix()
        {
            Matrix partialAutocorrelationMatrix = new Matrix(this.Count, this.Count);
            double[] samplePartialAutocorrelationFunction = new double[this.Count];
            double[] sampleAutocorrectionFunction = this.ComputeSampleAutocorrelationFunction();
            samplePartialAutocorrelationFunction[0] = 1;
            partialAutocorrelationMatrix[0, 0] = 1;
            samplePartialAutocorrelationFunction[1] = sampleAutocorrectionFunction[1];
            partialAutocorrelationMatrix[1, 1] = sampleAutocorrectionFunction[1];
            Func<int, double> Pi_tau = delegate (int tau)
            {
                double numer = sampleAutocorrectionFunction[tau], denom = 1;
                double sum = 0;
                for (int i = 1; i < tau; i++)
                {
                    sum += partialAutocorrelationMatrix[tau - 1, i] * sampleAutocorrectionFunction[tau - i];
                }
                return (numer - sum) / (denom - sum);
            };
            Func<int, int, double> Pi_tau_j = delegate (int tau, int j)
            {
                return partialAutocorrelationMatrix[tau - 1, j]
                    - partialAutocorrelationMatrix[tau, tau]
                    * partialAutocorrelationMatrix[tau - 1, tau - j];
            };

            for (int i = 2; i < samplePartialAutocorrelationFunction.Length; i++)
            {
                partialAutocorrelationMatrix[i, i] = Pi_tau(i);
                samplePartialAutocorrelationFunction[i] = partialAutocorrelationMatrix[i, i];
                for (int j = 1; j < i; j++)
                {
                    partialAutocorrelationMatrix[i, j] = Pi_tau_j(i, j);
                }
            }

            return partialAutocorrelationMatrix;
        }

        private void ComputeQportmanteau()
        {
            throw new NotImplementedException();
        }
    }
}

