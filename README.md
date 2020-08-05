# MatrixDotNet

![](https://github.com/toor1245/MatrixDotNet/blob/master/docs/MatrixDotNet.png)

<h3 align="center">

[![NuGet](https://img.shields.io/nuget/v/MatrixDotNet.svg)](https://www.nuget.org/packages/MatrixDotNet/) 
[![Downloads](https://img.shields.io/nuget/dt/matrixdotnet.svg)](https://www.nuget.org/packages/MatrixDotNet/)
[![Stars](https://img.shields.io/github/stars/toor1245/MatrixDotNet?color=brightgreen)](https://github.com/toor1245/MatrixDotNet/stargazers)
[![Gitter](https://badges.gitter.im/MatrixDotNet/community.svg)](https://gitter.im/MatrixDotNet/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)

</h3>

MatrixDotNet is a lightweight .NET library for calculate matrix. You can install MatrixDotNet via [NuGet package](https://www.nuget.org/packages/MatrixDotNet/).

### Features
* MatrixDotNet is made in priority on speed and accuracy of calculations

##### Example 
[!code-csharp[StrassenSample.cs](https://github.com/toor1245/MatrixDotNet/blob/master/samples/Samples/Samples/StrassenSample.cs)]
```C#
public class StrassenSample
{
    int[,] matrix = new int[512,512];
    int[,] matrix2 = new int[512,512];

    private Matrix<int> matrix3;
    private Matrix<int> matrix4;

    [GlobalSetup]
    public void Setup()
    {
        Random random = new Random();
        Random random2 = new Random();
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = random.Next(1, 10);
            }
        }
        matrix3 = new Matrix<int>(matrix);

        for (int i = 0; i < matrix2.GetLength(0); i++)
        {
            for (int j = 0; j < matrix2.GetLength(1); j++)
            {
                matrix2[i, j] = random2.Next(1, 10);
            }
        }

        matrix4 = new Matrix<int>(matrix2);
    }

    [Benchmark]
    public Matrix<int> Default()
    {
        return matrix3 * matrix4;
    }

    [Benchmark]
    public Matrix<int> Strassen()
    {
        return MatrixExtension.MultiplyStrassen(matrix3, matrix4);
    }
}
```

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.14393.3808 (1607/AnniversaryUpdate/Redstone1)
Intel Core i5-8250U CPU 1.60GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
Frequency=1757816 Hz, Resolution=568.8878 ns, Timer=TSC
  [Host]     : .NET Framework 4.8 (4.8.4180.0), X86 LegacyJIT
  Job-YFITZW : .NET Framework 4.8 (4.8.4180.0), X86 LegacyJIT

IterationCount=5  LaunchCount=1  WarmupCount=5  

```

```ini
|   Method |    Mean |   Error |  StdDev |      Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|--------- |--------:|--------:|--------:|-----------:|----------:|----------:|----------:|
|  Default | 69.88 s | 1.241 s | 0.322 s |          - |         - |         - |   1.01 MB |
| Strassen | 43.23 s | 0.991 s | 0.153 s | 30000.0000 | 5000.0000 | 2000.0000 | 174.32 MB |
```

As you can see algorithm `Strassen` multiply works significant faster(x1.625) than default multiply matrix on big size `MxN`.

* ##### MatrixDotNet bit hacks are present to improve performance

##### Example

```c#

public class MatrixBitMinVsDefaultMin
{
    private int N = 256;
    private int[,] matrix;
    private Matrix<int> matrix3;
    private Random random = new Random();
    private int[] arr;

    [GlobalSetup]
    public void Setup()
    {
        matrix = new int[N,N];
        arr = new int[N];
        // init matrix random data
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                matrix[i, j] = random.Next(-255, 255);
            }
        }
        matrix3 = new Matrix<int>(matrix);


    }

    [Benchmark]
    public void DefaultMin()
    { 
        for (int i = 0; i < N; i++)
        {
            arr[i] = matrix3.Min();   
        }
    }
    
    [Benchmark]
    public void BitMin()
    {
        for (int i = 0; i < N; i++)
        {
            arr[i] = matrix3.BitMin();   
        }
    }
}

```

``` ini
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.14393.3808 (1607/AnniversaryUpdate/Redstone1)
Intel Core i5-8250U CPU 1.60GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
Frequency=1757816 Hz, Resolution=568.8878 ns, Timer=TSC
  [Host]     : .NET Framework 4.8 (4.8.4180.0), X86 LegacyJIT
  DefaultJob : .NET Framework 4.8 (4.8.4180.0), X86 LegacyJIT
```
|     Method |       Mean |    Error |   StdDev | Code Size |
|----------- |-----------:|---------:|---------:|----------:|
| DefaultMin | 1,117.2 ms | 12.93 ms | 11.46 ms |     292 B |
|     BitMin |   646.5 ms |  4.16 ms |  3.25 ms |     165 B |

As you can see BitMin() method works faster(x1.725) than DefaultMin(). Because we eliminate branch prediction.
See more information about Bitwise operations in [article](https://toor1245.github.io/MatrixDotNet/articles/intro.html).

### Sample
Lets see simple operations MatrixDotNet.

```C#
public sealed class Program
{
    static void Main(string[] args)
    {
        // initialize matrix.
        double[,] arr =
        {
            {5,56,7},
            {3,6,3},
            {5,9,15}
        };


        Matrix<float> matrix = new Matrix<float>(arr);

        double[] right = { 1,23,5};

        double[] res = matrix.KramerSolve(right);
        for(var i = 0; i < res.Length; i++)
        {
            Console.Write($"x{i}: {res[i]}\n");
        }
    }
}
 
```
### Result
```
x0: 12,393939393939394
x1: -0,6637806637806638
x2: -3,3997113997114
```

#### See more information [docs](https://toor1245.github.io/MatrixDotNet/) 
