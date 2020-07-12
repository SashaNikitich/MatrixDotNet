﻿using System;
using System.Linq;
using System.Text;
using MatrixDotNet.Exceptions;
using MatrixDotNet.Extensions;

namespace MatrixDotNet
{
    /// <summary>
    /// Represents math matrix.
    /// </summary>
    /// <typeparam name="T">integral type.</typeparam>
    public class Matrix<T> : ICloneable
        where T : unmanaged
    {
        #region Properties

        /// <summary>
        /// Gets matrix.
        /// </summary>
        internal T[,] _Matrix { get; private set; }
        
        
        /// <summary>
        /// Gets length matrix.
        /// </summary>
        public int Length => _Matrix.Length;
        
        /// <summary>
        /// Gets length row of matrix.
        /// </summary>
        public int Rows => _Matrix.GetLength(0);

        /// <summary>
        /// Gets length columns of matrix.
        /// </summary>
        public int Columns => _Matrix.GetLength(1);

        /// <summary>
        /// Gets rank of matrix.
        /// </summary>
        public double Rank => GetRank();
        
        /// <summary>
        /// Checks square matrix.
        /// </summary>
        public bool IsSquare => Rows == Columns;
        
        #endregion

        #region Indexators
        
        /// <summary>
        /// Gets element matrix.
        /// </summary>
        /// <param name="i">the index by rows.</param>
        /// <param name="j">the index by columns.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Throws if index out of range
        /// </exception>
        public T this[int i, int j]
        {
            get
            {
                if (!IsRange(i, j))
                    throw new IndexOutOfRangeException();

                return _Matrix[i, j];
            }
            set
            {
                if (!IsRange(i, j))
                    throw new IndexOutOfRangeException();

                _Matrix[i, j] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets array by row.
        /// </summary>
        /// <param name="i">the row</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public T[] this[int i]
        {
            get => this.GetRow(i);
            set
            {
                if (!IsRange(i))
                    throw new IndexOutOfRangeException();
                
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] = value[j];
                }
            }
        }

        /// <summary>
        /// Gets or sets array by rows or columns.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="dimension"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public T[] this[int i,State dimension]
        {
            get
            {
                return dimension switch
                {
                    State.Row => this.GetRow(i),
                    State.Column => this.GetColumn(i),
                    _ => throw new ArgumentException("state error")
                };
            }

            set
            {
                if (!IsRange(i))
                    throw new IndexOutOfRangeException();
                
                if (dimension == State.Row)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        this[i, j] = value[j];
                    }
                }

                if (dimension == State.Column)
                {
                    for (int j = 0; j < Rows; j++)
                    {
                        this[j, i] = value[j];
                    }
                }
            }
        }


        #endregion
        
        #region Ctor
        
        /// <summary>
        /// Initialize matrix.
        /// </summary>
        /// <param name="matrix">the matrix.</param>
        public Matrix(T[,] matrix)
        {
            _Matrix = new T[matrix.GetLength(0),matrix.GetLength(1)];
            
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    _Matrix[i,j] = matrix[i,j];
                }
            }
        }

        /// <summary>
        /// Creates matrix.
        /// </summary>
        /// <param name="row">row</param>
        /// <param name="col">col</param>
        public Matrix(int row,int col)
        {
            _Matrix = new T[row,col];
        }
        
        #endregion

        #region OverLoad operator

        /// <summary>
        /// Add operation of two matrix.
        /// </summary>
        /// <param name="left">left matrix.</param>
        /// <param name="right">right matrix.</param>
        /// <returns><see cref="Matrix{T}"/></returns>
        /// <exception cref="MatrixDotNetException">
        /// Length of two matrix not equal.
        /// </exception>
        public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)
        {
            if (left.Length != right.Length)
            {
                throw new MatrixDotNetException(
                    $"matrix {nameof(left)} length: {left.Length} != matrix {nameof(right)}  length: {right.Length}");
            }
                
            Matrix<T> matrix = new Matrix<T>(left.Rows,right.Columns);

            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < left.Columns; j++)
                {
                    matrix[i, j] = MathExtension.Add(left[i,j],right[i,j]);
                }
            }

            return matrix;
        } 
        
        /// <summary>
        /// Subtract operation of two matrix.
        /// </summary>
        /// <param name="left">left matrix.</param>
        /// <param name="right">right matrix.</param>
        /// <returns><see cref="Matrix{T}"/>.</returns>
        /// <exception cref="MatrixDotNetException">
        /// Length of two matrix not equal.
        /// </exception>
        public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right)
        {
            if (left.Length != right.Length)
            {
                throw new MatrixDotNetException(
                    $"matrix {nameof(left)} length: {left.Length} != matrix {nameof(right)}  length: {right.Length}");
            }

            Matrix<T> matrix = new Matrix<T>(left.Rows,right.Columns);

            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < left.Columns; j++)
                {
                    matrix[i, j] = MathExtension.Sub(left[i,j],right[i,j]);
                }
            }

            return matrix;
        } 
        
        /// <summary>
        /// Multiply operation of two matrix.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        /// <exception cref="MatrixDotNetException"></exception>
        public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right)
        {
            if (left.Columns != right.Rows)
            {
                throw new MatrixDotNetException(
                    $"matrix {nameof(left)} columns length must be equal matrix {nameof(right)} rows length");
            }

            Matrix<T> matrix = new Matrix<T>(left.Rows,right.Columns);

            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < right.Columns; j++)
                {
                    for (int k = 0; k < left.Columns; k++)
                    {
                        // matrix[i,j] += left[i,k] * right[k,j]; 
                        matrix[i, j] = MathExtension
                            .Add(matrix[i,j],MathExtension.Multiply(left[i,k],right[k,j]));
                    }
                }
            }

            return matrix;
        } 
        
        /// <summary>
        /// Multiply operation matrix on digit right side.
        /// </summary>
        /// <param name="matrix">matrix.</param>
        /// <param name="digit">digit.</param>
        /// <returns><see cref="Matrix{T}"/></returns>
        public static Matrix<T> operator *(Matrix<T> matrix, T digit)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    // matrix1[i,j] = matrix[i,j] * matrix
                    matrix[i, j] = MathExtension.Multiply(matrix[i,j],digit);
                }
            }
            
            return matrix;
        }
       
        /// <summary>
        /// Multiply operation matrix on digit left side.
        /// </summary>
        /// <param name="digit">digit</param>
        /// <param name="matrix">matrix</param>
        /// <returns><see cref="Matrix{T}"/></returns>
        public static Matrix<T> operator *(T digit, Matrix<T> matrix)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    // matrix1[i,j] = matrix[i,j] * matrix
                    matrix[i, j] = MathExtension.Multiply(matrix[i,j],digit);
                }
            }
            
            return matrix;
        }
        
        /// <summary>
        /// Divide operation matrix on digit right side.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static Matrix<T> operator /(Matrix<T> matrix, T digit)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    // matrix1[i,j] = matrix[i,j] * matrix
                    matrix[i, j] = MathExtension.Divide(matrix[i,j],digit);
                }
            }
            
            return matrix;
        }
        
        /// <summary>
        /// Divide operation matrix on digit left side.
        /// </summary>
        /// <param name="digit"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix<T> operator /(T digit,Matrix<T> matrix)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    // matrix1[i,j] = matrix[i,j] * matrix
                    matrix[i, j] = MathExtension.Divide(matrix[i,j],digit);
                }
            }
            return matrix;
        }
        
        #endregion
        
        // Checks matrix on range by rows - i, columns - j.
        private bool IsRange(int i,int j)
        {
            return i < _Matrix.GetLength(0) || j < _Matrix.GetLength(1);
        }

        // Checks matrix on range by rows.
        private bool IsRange(int i)
        {
            return i < _Matrix.GetLength(0);
        }

        /// <summary>
        /// <inheritdoc cref="object.ToString"/>
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    builder.Append(this[i,j] + " ");
                }

                builder.Append("\n");
            }
            
            return builder.ToString();
        }

        /// <summary>
        /// Clones matrix.
        /// </summary>
        /// <returns>object.</returns>
        public object Clone()
        {
            Matrix<T> matrix = new Matrix<T>(Rows,Columns);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    matrix[i, j] = this[i, j];
                }
            }

            return matrix;
        }
        
        /// <summary>
        /// Checks on equals two matrix by rows - i ,columns - j
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public override bool Equals(object obj)
        {
            if(!(obj is Matrix<T>))
                throw new ArgumentException();
            
            var t = (Matrix<T>) obj;
            var count = 0;
            
            for (int i = 0; i < t.Rows; i++)
            {
                for (int j = 0; j < t.Columns; j++)
                {
                    if (this[i, j].Equals(t[i, j])) count++;
                }
            }
            
            return count == Length;
        }

        /// <summary>
        /// Gets hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        /// <summary>
        /// Gets rank matrix.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MatrixDotNetException"></exception>
        private double GetRank()
        {
            var matrix = (Matrix<T>)Clone();
            if (matrix is null)
                throw new MatrixDotNetException("matrix is null", nameof(_Matrix));
            
            for (int i = 0; i < Columns; i++)
            {
                for (int j = i + 1; j < Rows; j++)
                { 
                    matrix[j, i] = default;
                }
            }
            
            return this.GetDeterminate();
        }

        /// <summary>
        /// Gets l-norm of matrix.
        /// </summary>
        /// <returns></returns>
        public double LNorm()
        {
            T max = default;
            var temp = new T[Rows]; 
            
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    max = MathExtension.Add(max,this[j, i]);
                }
                temp[i] = max;
                max = default;
            }

            return default;
        }

        /// <summary>
        /// Gets m-norm of matrix.
        /// </summary>
        public T MNorm => this.MaxRows().Max();
    }

    /// <summary>
    /// State column or row
    /// </summary>
    public enum State
    {
        Row,
        Column
    }
}