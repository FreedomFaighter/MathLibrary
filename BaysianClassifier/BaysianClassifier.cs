using System;
using System.Collections.Generic;
using MathLibrary.LinearAlgebra;

namespace BaysianClassifier
{
    public class Classifier
    {
        private Dictionary<string, Matrix> classes = new Dictionary<string, Matrix>();

        public Dictionary<string, Matrix> Classes
        {
            get { return this.classes; }
            set { this.classes = value; }
        }

        private Dictionary<string, ColumnVector> MeanOfClasses = new Dictionary<string, ColumnVector>();

        private Dictionary<string, Func<ColumnVector, double?>> discriminantsOfEachClass = new Dictionary<string, Func<ColumnVector, double?>>();

        private Dictionary<string, Matrix> sigmas = new Dictionary<string, Matrix>();

        private int totalObservations = 0;

        private Dictionary<string, double> probabilityEachClass = new Dictionary<string, double>();

        private Dictionary<string, Matrix> inverseSigmas = new Dictionary<string, Matrix>();

        public Classifier(Dictionary<string, Matrix> classes)
        {
            this.classes = classes;
            CalculateMeanOfEachClass();
            GetSigmaMatricies();
            CalculateProbabilityOfEachClass();
            GenerateDiscriminantFunctions();
        }

        void CalculateProbabilityOfEachClass()
        {
            foreach (KeyValuePair<string, Matrix> kvp in classes)
            {
                this.probabilityEachClass.Add(kvp.Key, kvp.Value.Values.GetLength(0) / this.totalObservations);
            }
        }

        void CalculateMeanOfEachClass()
        {
            this.MeanOfClasses = new Dictionary<string, ColumnVector>();

            foreach (KeyValuePair<string, Matrix> kvp in classes)
            {
                ColumnVector value = new ColumnVector(kvp.Value.Values.GetLength(1));
                for (int i = 0; i < value.Values.Length; i++)
                {
                    value[i] = 0;
                }
                for (int i = 0; i < kvp.Value.Values.GetLength(0); i++)
                {
                    for (int j = 0; j < value.Values.Length; j++)
                    {
                        value[j] += kvp.Value[i, j] / kvp.Value.Values.GetLength(0);
                    }
                }

                this.MeanOfClasses.Add(kvp.Key, value);
                this.totalObservations += kvp.Value.Values.GetLength(0);
            }
        }

        void GetInverseSigmas()
        {
            foreach (KeyValuePair<string, Matrix> kvp in this.sigmas)
            {
                this.inverseSigmas.Add(kvp.Key, kvp.Value.Inverse());
            }
        }

        void GetSigmaMatricies()
        {
            foreach (KeyValuePair<string, Matrix> kvp in classes)
            {
                this.sigmas.Add(kvp.Key, kvp.Value.Covariance());
            }
            this.GetInverseSigmas();
        }

        void GenerateDiscriminantFunctions()
        {
            //this section isn't good enough, Null value + a real value isn't handled properly
            foreach (KeyValuePair<string, Matrix> kvp in classes)
            {
                this.discriminantsOfEachClass.Add(kvp.Key, (x) =>
                {
                    ColumnVector xMinusMean = x - this.MeanOfClasses[kvp.Key];
                    return kvp.Value.Determinant().HasValue ? -0.5 * (System.Math.Log((double)kvp.Value.Determinant()) + xMinusMean.Transpose()
                                   * this.inverseSigmas[kvp.Key] * xMinusMean)
                        + System.Math.Log(this.probabilityEachClass[kvp.Key]) : new Nullable<double>();
                });
            }

        }

        public string FindClass(ColumnVector x)
        {
            double? max = double.MinValue;
            string maxClass = "\0";
            foreach (KeyValuePair<string, Func<ColumnVector, double?>> kvpFunc in this.discriminantsOfEachClass)
            {
                double? result = kvpFunc.Value(x);
                if (result == max)
                {
                    return "Is in multiple classes";
                }
                else if (result > max)
                {
                    max = result;
                    maxClass = kvpFunc.Key;
                }
                else
                {
                    return "Something went wrong";
                }
            }
            return maxClass;
        }
    }
}
