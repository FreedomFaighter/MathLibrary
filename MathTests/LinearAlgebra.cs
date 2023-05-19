using NUnit.Framework;
using MathLibrary.LinearAlgebra;
namespace MathTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Is500by500Matrix()
        {
            int matrixSize = 500;
            Matrix m = new Matrix(matrixSize, matrixSize);

            Assert.IsTrue(m.Columns == m.Rows && m.Columns == matrixSize && m.Rows == matrixSize);
        }
    }
}