using MatrixDotNet.Extensions.Decomposition;

namespace MatrixDotNet.Extensions.Determinant
{
    public static partial class Determinant 
    {
        /// <summary>
        /// Gets LUP determinant.
        /// </summary>
        /// <param name="matrix">the matrix.</param>
        /// <returns></returns>
        public static double GetLowerUpperPermutationDeterminant(this Matrix<double> matrix)
        {
            matrix.GetLowerUpperPermutation(out var lower,out var upper,out var p);
            double det = 1;
            for (int i = 0; i < upper.Rows; i++)
            {
                det *= upper[i, i];
            }
            
            return det;
        }
    }
}