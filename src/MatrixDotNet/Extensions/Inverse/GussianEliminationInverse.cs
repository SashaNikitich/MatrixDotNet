using System;
using MatrixDotNet.Exceptions;
using MatrixDotNet.Extensions.Builder;
using MatrixDotNet.Extensions.Conversion;

namespace MatrixDotNet.Extensions.Inverse
{
    public static partial class MatrixExtension
    {
        public static Matrix<double> GaussianEliminationInverse(this Matrix<double> matrix)
        {
            if (!matrix.IsSquare)
                throw new MatrixDotNetException("Matrix must be square");

            int size = matrix.Rows;
            Matrix<double> a = matrix.Clone() as Matrix<double>;
            Matrix<double> b = matrix.CreateIdentityMatrix();
            
            if(a is null || b is null)
                throw new NullReferenceException();
            
            // Forward substitution
            for (int i = 0; i < size - 1; i++) 
            {

                if (Math.Abs(a[i, i]) < 0.00001) 
                {
                    for (int j = i + 1; j < size; j++)
                    {
                        if (Math.Abs(a[j, i]) < 0.00001) 
                        {
                            if (j == size - 1) 
                            {
                                throw new MatrixDotNetException("Matrix is singular");
                            }
                        }
                        else 
                        {
                            a.SwapRows(i, j);
                            b.SwapRows(i, j);
                            break;
                        }
                    }
                }

                for (int k = i + 1; k < size; k++)
                {
                    if(Math.Abs(a[k,i]) < 0.00001) continue;
                    double div = a[k,i] / a[i, i];
                    for (int j = 0; j < size; j++)
                    {
                        a[k, j] = a[k, j] - a[i, j] * div;
                        b[k, j] = b[k, j] - b[i, j] * div;
                    }
                }
            }

            // Back substitution
            for (int i = size - 1; i > 0; i--) 
            {
                if (Math.Abs(a[i, i]) < 0.00001) 
                {
                    for (int j = i + 1; j < size; j++) 
                    {
                        if (Math.Abs(a[j, i]) < 0.00001)
                        {
                            if (j == size - 1) 
                            {
                                throw new MatrixDotNetException("Matrix is singular");
                            }
                        }
                        else 
                        {
                            a.SwapRows(i, j);
                            b.SwapColumns(i,j);
                            break;
                        }
                    }
                }

                for (int k = i - 1; k >= 0; k--) 
                {
                    if(Math.Abs(a[k,i]) < 0.00001) continue;
                    double div = a[k,i] / a[i, i];
                    
                    for (int j = size - 1; j >= 0; j--) 
                    {
                        a[k, j] =  a[k,j] - a[i,j] * div;
                        b[k, j] =  b[k,j] - b[i,j] * div;
                    }
                }
            }

            // Correction
            for (int i = 0; i < size; i++) 
            {
                double d = a[i, i];
                if (Math.Abs(d - 1) < 0.00001) continue;
                
                for (int j = 0; j < size; j++) 
                {
                    b[i, j] =  b[i, j] / d;
                }
            }

            for (int i = size - 2; i >= 0; i--)
            {
                b.SwapColumns(size - 1,i);
            }

            return b;
        }
    }
}