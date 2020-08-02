﻿using MatrixDotNet.Exceptions;

namespace MatrixDotNet.Extensions
{
    public static partial class MatrixExtension
    {
        public static void GetLowerUpperPermutation<T>(this Matrix<T> matrix,out Matrix<T> lower,out Matrix<T> upper) where T : unmanaged
        {
            if (!matrix.IsSquare)
                throw new MatrixDotNetException(
                    $"matrix is not square\n Rows: {matrix.Rows}\n Columns: {matrix.Columns}");

            int n = matrix.Columns;
            
            lower = new Matrix<T>(n,n);
            upper = new Matrix<T>(n, n)
            {
                [0, State.Row] = matrix[0, State.Row]
            };
            
            for (int i = 0; i < n; i++)
            {
                lower[0, i, State.Column] = MathExtension.Divide(matrix[0, State.Column][i], upper[0, 0]);
            }
            
            for (int i = 1; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    T sumL = default;
                    T sumU = default;
                    for (int k = 0; k < i; k++)
                    {
                        sumU = MathExtension.Add(sumU, MathExtension.Multiply(lower[i, k], upper[k, j]));
                        sumL = MathExtension.Add(sumL, MathExtension.Multiply(lower[j, k], upper[k, i]));
                    }
                    
                    upper[i, j] = MathExtension.Sub(matrix[i, j],sumU);
                    lower[j, i] = MathExtension.Divide(MathExtension.Sub(matrix[j, i],sumL),upper[i,i]);
                }
            }
        }
        
        public static Matrix<T> GetLowerMatrix<T>(this Matrix<T> matrix) where T : unmanaged
        {
            if (!matrix.IsSquare)
                throw new MatrixDotNetException("matrix is not square");
            
            Matrix<T> lower = new Matrix<T>(matrix.Rows,matrix.Columns);
            
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    lower[i, j] = matrix[i,j];
                }
            }
            
            return lower;
        }
        
        public static Matrix<T> GetUpperMatrix<T>(this Matrix<T> matrix) where T : unmanaged
        {
            if (!matrix.IsSquare)
                throw new MatrixDotNetException("matrix is not square");
            Matrix<T> upper = new Matrix<T>(matrix.Rows,matrix.Columns);
            
            for (int i = 0; i < matrix.Columns; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    upper[j, i] = matrix[i,j];
                }
            }
            
            return upper;
        }
    }
}