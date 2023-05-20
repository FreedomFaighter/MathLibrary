using MathLibrary.LinearAlgebra;

namespace Statistics.LinearRegression
{
    public class LinearRegression
    {
        Matrix BetaHats, Independent, dependent;
        double SSE, SSR, SST;
        public LinearRegression(Matrix Independent, Matrix dependent) 
        {
            this.Independent = Independent;
            this.dependent = dependent;
        }

        public void CalculateBetas()
        {
            if (this.Independent.Rows == this.dependent.Rows)
                this.BetaHats = (Independent.Transpose() * Independent).Inverse() * Independent.Transpose() * dependent;
            else
                throw new Exception($"Dimensionality mismatch {this.Independent.Rows} != {this.dependent.Rows}.");
        }

        public void ComputeSSE()
        {
            Matrix intermediateResult = this.dependent - this.Independent * this.BetaHats;
            this.SSE = (intermediateResult.Transpose() * intermediateResult)[0,0];
        }

        public double evaluateAt(double[] independents)
        {
            if (this.BetaHats == default)
                throw new NullReferenceException("Regression coefficients are not assigned");
            double sum = this.BetaHats[0,0];
            for (int i = 0; i < independents.Length; i++)
                sum += independents[i] * this.BetaHats[i+1,0];
            return sum;
        }

        public void ComputeSST()
        {
            if (this.dependent == default)
                throw new NullReferenceException("Dependent values not assigned");
            double sum = 0.0;
            for (int i = 0; i < this.dependent.Rows; i++)
                sum += this.dependent[i, 0];
            double mean = sum / this.dependent.Rows;
            double sst = 0.0;
            for (int i = 0; i < this.dependent.Rows; i++)
            {
                sst += (this.dependent[i, 0] * this.dependent[i,0] - 2 * mean * this.dependent[i,0] + mean * mean);
            }
            this.SST = sst;
        }

        public void ComputeSSR()
        {
            if (this.SSR == default && this.SST == default)
                this.SSR = this.SST - this.SSE;
            else
                throw new NullReferenceException("Tried to use SSR or SST that is not assigned");
        }

        public double ComputeMSR()
        {
            if (this.SSR == default && this.Independent.Columns > 0)
                throw new NullReferenceException("SSR is not assigned to");
            return this.SSR / this.Independent.Columns;
        }

        public double ComputeMSE()
        {
            if (this.SSE == default)
                throw new NullReferenceException("SSE is not assigned");
            if (this.Independent.Columns < 1)
                throw new NullReferenceException("Number of dimensions on the observation less then 1.");
            if (this.Independent.Rows < 1)
                throw new NullReferenceException("Number of observations is less then 1.");
            if (this.Independent.Rows - (this.Independent.Columns + 1) < 1)
                throw new NullReferenceException("Number of observations and dimensions will be less then 1");
            return this.SSE / (this.Independent.Rows - (this.Independent.Columns + 1));
        }

        public double ComputeFStatistic()
        {
            return this.ComputeMSR() / this.ComputeMSE();
        }
    }
}
