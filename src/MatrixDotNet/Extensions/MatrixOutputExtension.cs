﻿using System;
using System.Text;
using MatrixDotNet.Extensions.BitMatrix;

namespace MatrixDotNet.Extensions
{
    public static partial class MatrixExtension
    {
        /// <summary>
        /// Pretty output.
        /// </summary>
        /// <param name="matrix"></param>
        /// <typeparam name="T"></typeparam>
        public static void Pretty<T>(this Matrix<T> matrix) where T : unmanaged
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            SetColorMessage(matrix);
            Console.ResetColor();
        }

        private static void SetColorMessage<T>(Matrix<T> matrix) where  T : unmanaged
        {
            if (matrix is null)
                throw new NullReferenceException();
            
            T[] arr = matrix.MaxColumns();
            T[] arr2 = matrix.MinColumns();
            int[] output = new int[arr.Length];
            
            for (int i = 0; i < output.Length; i++)
            {
                var x = string.Format($"{arr[i].ToString():f2}").Length;
                var y = string.Format($"{arr2[i].ToString():f2}").Length;

                if (x > y)
                {
                    output[i] = string.Format($"{arr[i].ToString():f2}").Length;
                }
                else
                {
                    output[i] = string.Format($"{arr2[i].ToString():f2}").Length;
                }
            }
            SetColorMessageSmallSize(matrix, output);

        }
        
        
        private static void SetColorMessageSmallSize<T>(Matrix<T> matrix,int[] output) where T : unmanaged 
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine();
            
            builder.AppendLine($"Number of rows: {matrix.Rows}");
            builder.AppendLine($"Number of columns: {matrix.Columns}\n");
            
            for (int i = 0; i < matrix.Rows; i++)
            {

                for (int j = 0; j < matrix.Columns; j++)
                {
                    var n = output[j];
                    int length = $"{matrix[i, j].ToString():f2}".Length;
                    string format = $"{matrix[i, j].ToString():f2}";
                    if (length >= n)
                    {
                        builder.Append(" ".PadLeft(2) + format + "".PadRight(length - n) + "  |");    
                    }
                    else
                    {
                        builder.Append(" ".PadLeft(2) + format + "".PadRight(n - length) + "  |");
                    }
                }

                builder.AppendLine();
            }

            Console.WriteLine(builder.ToString());
        }
        
        internal static string OutputPretty<T>(Matrix<T> matrix) where  T : unmanaged
        {
            if (matrix is null)
                throw new NullReferenceException();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine();

            int n = 9;
            
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    int length = $"{matrix[i, j]:f2}".Length;
                    string format = $"{matrix[i, j]:f2}";
                    
                    if (length > n)
                    {
                        builder.Append(" ".PadLeft(2) + format + "".PadRight(length - n) +   "|");    
                    }
                    else
                    {
                        builder.Append(" ".PadLeft(2) + format + "".PadRight(n + (n - length)) +   "|");
                    }
                }

                builder.AppendLine();
            }
            
            return builder.ToString();
        }
    }
}