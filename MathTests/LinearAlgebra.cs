//using NUnit.Framework;
using MathLibrary.LinearAlgebra;

namespace MathTests
{
    public class Tests
    {
        [Test]
        public void Is500by500Matrix()
        {
            int matrixSize = 500;
            Matrix m = new(matrixSize, matrixSize);

            Assert.IsTrue(m.Columns == m.Rows && m.Columns == matrixSize && m.Rows == matrixSize);
        }

        [Test]
        public void Is1001by1001Matrix()
        {
            int matrixSize = 1001;
            Matrix m = new(matrixSize,matrixSize);
            Assert.IsTrue(m.Columns == m.Rows && m.Columns == matrixSize && m.Rows == matrixSize);
        }

        [Test]
        public void IsNot1000by1000Matrix()
        {
            int matrixSize = 1001;
            Matrix m = new Matrix(matrixSize,matrixSize);
            Assert.IsFalse((m.Columns * m.Columns) == (1000 ^ 2));
        }

        [Test]
        public void IsSquare500by500()
        {
            int matrixSize = 500;
            Matrix matrix = new(matrixSize,matrixSize);
            Assert.IsTrue((matrixSize * matrixSize) == (matrix.Columns * matrix.Rows));
        }

        [Test]
        public void OperatorNotationFalse()
        {
            int matrixSize = 17;
            Matrix matrix = new(matrixSize,matrixSize);
            Assert.IsFalse((matrixSize ^ 2) == (matrix.Columns * matrix.Rows));
        }
    }
}