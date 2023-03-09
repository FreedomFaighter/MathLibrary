using System;
using System.Linq;
using Math.LinearAlgebra;
using Statistics;

namespace Statistics.TimeSeries
{
    public class TimeSeries
    {
        private double[] values;
        private double[] sampleAutocovarianceFunction;
        private double[] sampleAutocorrelationFunction;
        private double[] samplePartialAutocorrelationFunction;
        private double sampleMean, sampleVariance;
        private Matrix covarianceMatrix;
        private Matrix autocorrelationMatrix;
        private Matrix partialAutocorrelationMatrix;
        //private double Qportmanteautest;

        public double[] SampleAutocorrelationFunction
        {
            get { return this.sampleAutocorrelationFunction; }
        }

        public double[] SamplePartialAutocorrelationFunction
        {
            get { return this.samplePartialAutocorrelationFunction; }
        }

        public TimeSeries(double[] v)
        {
            values = v;
            sampleMean = values.Mean();
            sampleVariance = values.Variance();
            ComputeAutocovarianceFunction();
            ComputeSampleAutocorrelationFunction();
            ComputeSamplePartialAutocorrelationMatrix();
        }

        public double this[int i]
        {
            get { return this.values[i]; }
            set { values[i] = value; }
        }

        private void ComputeAutocovarianceFunction()
        {
            this.sampleAutocovarianceFunction = new double[values.Length];
            for (int h = 0; h < values.Length; h++)
            {
                this.sampleAutocovarianceFunction[h] = values.Take(values.Length - h).Select((x, t) =>
                {
                    return (values[t + h] - sampleMean) * (values[t] - sampleMean);
                }).Sum() / values.Length;
            }

            this.covarianceMatrix = new Matrix(this.sampleAutocovarianceFunction.Length, this.sampleAutocovarianceFunction.Length);
            for (int i = 0; i < this.covarianceMatrix.Values.GetLength(0); i++)
            {
                for (int j = 0; j < this.covarianceMatrix.Values.GetLength(1); j++)
                {
                    this.covarianceMatrix[i, j] = this.sampleAutocovarianceFunction[System.Math.Abs(i - j)];
                }
            }
        }

        private void ComputeSampleAutocorrelationFunction()
        {
            this.sampleAutocorrelationFunction = new double[this.sampleAutocovarianceFunction.Length];
            for (int h = 0; h < this.sampleAutocovarianceFunction.Length; h++)
            {
                this.sampleAutocorrelationFunction[h] = this.sampleAutocovarianceFunction[h] / this.sampleAutocovarianceFunction[0];
            }

            this.autocorrelationMatrix = new Matrix(this.sampleAutocorrelationFunction.Length, this.sampleAutocorrelationFunction.Length);
            for (int i = 0; i < this.autocorrelationMatrix.Values.GetLength(0); i++)
            {
                for (int j = 0; j < this.autocorrelationMatrix.Values.GetLength(1); j++)
                {
                    this.autocorrelationMatrix[i, j] = this.sampleAutocorrelationFunction[System.Math.Abs(i - j)];
                }
            }
        }

        private void ComputeSamplePartialAutocorrelationMatrix()
        {
            this.partialAutocorrelationMatrix = new Matrix(this.values.Length, this.values.Length);
            this.samplePartialAutocorrelationFunction = new double[this.values.Length];
            this.samplePartialAutocorrelationFunction[0] = 1;
            this.partialAutocorrelationMatrix[0, 0] = 1;
            this.samplePartialAutocorrelationFunction[1] = this.sampleAutocorrelationFunction[1];
            this.partialAutocorrelationMatrix[1, 1] = this.sampleAutocorrelationFunction[1];
            Func<int, double> Pi_tau = delegate (int tau)
            {
                double numer = this.sampleAutocorrelationFunction[tau], denom = 1;
                double sum = 0;
                for (int i = 1; i < tau; i++)
                {
                    sum += this.partialAutocorrelationMatrix[tau - 1, i] * this.sampleAutocorrelationFunction[tau - i];
                }
                return (numer - sum) / (denom - sum);
            };
            Func<int, int, double> Pi_tau_j = delegate (int tau, int j)
            {
                return this.partialAutocorrelationMatrix[tau - 1, j]
                    - this.partialAutocorrelationMatrix[tau, tau]
                    * this.partialAutocorrelationMatrix[tau - 1, tau - j];
            };

            for (int i = 2; i < this.samplePartialAutocorrelationFunction.Length; i++)
            {
                this.partialAutocorrelationMatrix[i, i] = Pi_tau(i);
                this.samplePartialAutocorrelationFunction[i] = this.partialAutocorrelationMatrix[i, i];
                for (int j = 1; j < i; j++)
                {
                    this.partialAutocorrelationMatrix[i, j] = Pi_tau_j(i, j);
                }
            }
        }

        private void ComputeQportmanteau()
        {
            throw new NotImplementedException();
        }
    }
}

