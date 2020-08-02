﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MatrixDotNet.Extensions
{
    internal static class MathExtension
    {
        private static readonly Dictionary<(Type type,string op),Delegate> Cache =
            new Dictionary<(Type type, string op), Delegate>();

        #region Arithmetic and Logic Op
        
        internal static T Add<T>(T left, T right) where T: unmanaged
        {
            var t = typeof(T);
            if (Cache.TryGetValue((t, nameof(Add)),out var del))
                return del is Func<T,T,T> specificFunc
                    ? specificFunc(left, right)
                    : throw new InvalidOperationException(nameof(Add));
            
            var leftPar = Expression.Parameter(t, nameof(left));
            var rightPar = Expression.Parameter(t, nameof(right));
            var body = Expression.Add(leftPar, rightPar);
            
            var func = Expression.Lambda<Func<T, T, T>>(body, leftPar, rightPar).Compile();

            Cache[(t, nameof(Add))] = func;

            return func(left, right);
        }
        
        internal static T Sub<T>(T left, T right) where T: unmanaged
        {
            var t = typeof(T);
            if (Cache.TryGetValue((t, nameof(Sub)),out var del))
                return del is Func<T,T,T> specificFunc
                    ? specificFunc(left, right)
                    : throw new InvalidOperationException(nameof(Sub));
            
            var leftPar = Expression.Parameter(t, nameof(left));
            var rightPar = Expression.Parameter(t, nameof(right));
            var body = Expression.Subtract(leftPar, rightPar);
            
            var func = Expression.Lambda<Func<T, T, T>>(body, leftPar, rightPar).Compile();

            Cache[(t, nameof(Sub))] = func;

            return func(left, right);
        }
        
        internal static T Multiply<T>(T left, T right) where T: unmanaged
        {
            var t = typeof(T);
            if (Cache.TryGetValue((t, nameof(Multiply)),out var del))
                return del is Func<T,T,T> specificFunc
                    ? specificFunc(left, right)
                    : throw new InvalidOperationException(nameof(Multiply));
            
            var leftPar = Expression.Parameter(t, nameof(left));
            var rightPar = Expression.Parameter(t, nameof(right));
            var body = Expression.Multiply(leftPar, rightPar);
            
            var func = Expression.Lambda<Func<T, T, T>>(body, leftPar, rightPar).Compile();

            Cache[(t, nameof(Multiply))] = func;

            return func(left, right);
        }
        
        internal static T Divide<T>(T left, T right) where T: unmanaged
        {
            var t = typeof(T);
            if (Cache.TryGetValue((t, nameof(Divide)),out var del))
                return del is Func<T,T,T> specificFunc
                    ? specificFunc(left, right)
                    : throw new InvalidOperationException(nameof(Divide));
            
            var leftPar = Expression.Parameter(t, nameof(left));
            var rightPar = Expression.Parameter(t, nameof(right));
            var body = Expression.Divide(leftPar, rightPar);
            
            var func = Expression.Lambda<Func<T, T, T>>(body, leftPar, rightPar).Compile();

            Cache[(t, nameof(Divide))] = func;

            return func(left, right);
        }
        
        internal static bool GreaterThan<T>(T left, T right) where T: unmanaged
        {
            var t = typeof(T);
            if (Cache.TryGetValue((t, nameof(GreaterThan)),out var del))
                return del is Func<T,T,bool> specificFunc
                    ? specificFunc(left, right)
                    : throw new InvalidOperationException(nameof(GreaterThan));
            
            var leftPar = Expression.Parameter(t, nameof(left));
            var rightPar = Expression.Parameter(t, nameof(right));
            var body = Expression.GreaterThan(leftPar, rightPar);
            
            var func = Expression.Lambda<Func<T, T,bool>>(body, leftPar, rightPar).Compile();

            Cache[(t, nameof(GreaterThan))] = func;

            return func(left, right);
        }
        
        #endregion
    }
}