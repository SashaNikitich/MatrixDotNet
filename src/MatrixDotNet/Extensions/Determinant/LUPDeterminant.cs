using MatrixDotNet.Extensions.Decomposition;

namespace MatrixDotNet.Extensions.Determinant
{
    public static partial class Determinant 
    {
        public static double GetLUPDeterminant(this Matrix<double> matrix)
        {
            matrix.GetLUP(out var lower,out var upper,out var p);
            double det = 1;
            for (int i = 0; i < upper.Rows; i++)
            {
                det *= upper[i, i];
            }
            
            return det;
        }
    }
}