// Copyright 2024 - Gregory James Carriveau a.k.a. fractalsnakes840
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files(the �Software�), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and /or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in all copies
// or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

// Let's tickle the CUDAs :)
#include <cuda_runtime.h>
#include <helper_cuda.h>
#include <device_launch_parameters.h>	// not required.. I use it for getting rid of Intellisense squigglies under blockIdx, blockDim, threadIdx in Visual Studio 2022
#include <thrust/complex.h>             // numerics for double precision complex numbers


// Global device constants
__device__ __constant__ double g_e{ 2.718281828459045 }; // euler's number

// Global device variables
//__device__ const int g_colorsInPalette{ 1000 };
__device__ double g_juliaCenterX;
__device__ double g_juliaCenterY;
__device__ int    g_maxIts;
__device__ double g_limit;
__device__ int    g_N;
__device__ int    g_ismove;
typedef thrust::complex<double> (*frmptr)(thrust::complex<double>, thrust::complex<double>);
__device__ frmptr g_alg;

// Fractal default 0: z^2 + p
__device__ thrust::complex<double> frmMandelbrot(thrust::complex<double> z, thrust::complex<double> p)
{
    return z * z + p;
}
// Fractal 1: (sin(z) * z)^2  + p
__device__ thrust::complex<double> frmSinPow2(thrust::complex<double> z, thrust::complex<double> p)
{
    thrust::complex<double>temp{ thrust::sin(z) * z };
    return temp * temp + p;
}
// Fractal 2: (sin(z) * z)^3  + p
__device__ thrust::complex<double> frmSinPow3(thrust::complex<double> z, thrust::complex<double> p)
{
    thrust::complex<double>temp{ thrust::sin(z) * z };
    return temp * temp * temp + p;
}
// Fractal 3: (sin(z) * z)^4  + p
__device__ thrust::complex<double> frmSinPow4(thrust::complex<double> z, thrust::complex<double> p)
{
    thrust::complex<double>temp{ thrust::sin(z) * z };
    return temp * temp * temp * temp + p;
}
// Fractal 4: (sin(z) * z / div)^2 + p
__device__ thrust::complex<double> frmSinPow2Div1(thrust::complex<double> z, thrust::complex<double> p)
{
    thrust::complex<double>temp{ z / thrust::complex<double>{4.0, -1.5} };
    temp *= thrust::sin(z);
    temp *= temp;
    return temp + p;
}
// Fractal 5: (cos(z) * z)^2  + p
__device__ thrust::complex<double> frmCosPow2(thrust::complex<double> z, thrust::complex<double> p)
{
    thrust::complex<double>temp{ thrust::cos(z) * z };
    return temp * temp + p;
}
// Fractal 6: ((z^2 + p) * (cos(z) * z)^2 + p)
__device__ thrust::complex<double> frmCosPow2MandelbrotHybrid(thrust::complex<double> z, thrust::complex<double> p)
{
    thrust::complex<double>temp{ z * z + p};
    thrust::complex<double>temp2{ thrust::cos(z) * z };
    temp2 = temp2 * temp2 + p;
    return temp * temp2;
}
// Fractal 7: Mandelbrot => Burning Ship (+re, -im) => (cos(Burning Ship) * (Burning Ship))^2 + p
__device__ thrust::complex<double> frmCosPow2AbsRIMandelbrotHybrid(thrust::complex<double> z, thrust::complex<double> p)
{
    thrust::complex<double>temp{ thrust::cos(z) * z };
    // this is the conjugate of Mandelbrot z
    temp = thrust::complex<double>{ temp.real(), -temp.imag() };
    // Hybridization of Burning Ship
    temp = thrust::cos(temp) * temp; // cos(+re,-im) * (+re,-im)
    return temp * temp + p;
}
// Fractal 8: (cos(+re,-im) * (+re,-im))^2 + p
__device__ thrust::complex<double> frmCosPow2AbsRI(thrust::complex<double> z, thrust::complex<double> p)
{
    thrust::complex<double> temp{ abs(z.real()), -abs(z.imag()) };
    // this is the conjugate of Mandelbrot z
    temp = thrust::cos(temp) * temp;
    return temp * temp + p;
}
// Fractal 28: SinAbsRI
__device__ thrust::complex<double> frmSinAbsRI(thrust::complex<double> z, thrust::complex<double> p)
{
    thrust::complex<double> temp{ abs(z.real()), -abs(z.imag()) };
    temp = thrust::sin(temp) * temp * temp;
    return temp + p;
}
// Fractal 9: Experiment number 1 ((1 - z^3) / 6) / ((1 - z - z^2) / 2)^2 + p
__device__ thrust::complex<double> frmExperiment1(thrust::complex<double> z, thrust::complex<double> p)
{
    thrust::complex<double> temp{ (1 - z * z * z) / 6 };
    thrust::complex<double> temp2{ (1 - z - z * z) / 2 };
    temp /= temp2 * temp2;
    return temp + p;
}
// Fractal 10: CosPow2AbsRIPow4
__device__ thrust::complex<double> frmCosPow2AbsRIPow4(thrust::complex<double> z, thrust::complex<double> p)
{
    // res = Complex.Pow(new Complex(Math.Abs(res.Real), -Math.Abs(res.Imaginary)), 4);
    // return Complex.Pow(Complex.Multiply(Complex.Cos(res), res), 2.0) + p;
    thrust::complex<double> temp{ thrust::pow(thrust::complex<double>{cuda::std::abs(z.real()), -cuda::std::abs(z.imag())}, 4)};
    temp = thrust::pow(thrust::cos(temp) * temp, 2);
    return temp + p;
}
// Fractal 11: CosPow2SinPow2Hybrid
__device__ thrust::complex<double> frmCosPow2SinPow2Hybrid(thrust::complex<double> z, thrust::complex<double> p)
{
    // res = Complex.Pow(Complex.Multiply(Complex.Sin(res), res), 2.0) + p;
    // return Complex.Pow(Complex.Multiply(Complex.Cos(res), res), 2.0) + p;
    thrust::complex<double> temp{ (thrust::sin(z) * z) };
    temp *= temp;
    temp += p;
    temp = thrust::cos(temp) * temp;
    temp *= temp;
    return temp + p;
}
// Fractal 12: CosPow3
__device__ thrust::complex<double> frmCosPow3(thrust::complex<double> z, thrust::complex<double> p)
{
    // return return Complex.Pow(Complex.Multiply(Complex.Cos(res), res), 3.0) + p;
    thrust::complex<double> temp{ (thrust::cos(z) * z) };
    temp *= temp * temp;
    return temp + p;
}
// Fractal 13: WeirdLim5
__device__ thrust::complex<double> frmWeirdLim5(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Pow(Complex.Multiply(Complex.Cos(res), Complex.Divide(res, Complex.Add(res, -1))), 3.0) + p;
    thrust::complex<double> temp{ thrust::cos(z) * z / (z - 1.0) };
    temp = temp * temp * temp;
    return temp + p;
}
// Fractal 14: WeirdLim9 (same as above, but with a higher escape threshold)
__device__ thrust::complex<double> frmWeirdLim9(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Pow(Complex.Multiply(Complex.Cos(res), Complex.Divide(res, Complex.Add(res, -1))), 3.0) + p;
    thrust::complex<double> temp{ thrust::cos(z) * z / (z - 1.0) };
    temp = temp * temp * temp;
    return temp + p;
}
// Fractal 15: Weird2
__device__ thrust::complex<double> frmWeird2(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Add(Complex.Add(Complex.Sin(res), Complex.Pow(Math.E, res)), p);
    thrust::complex<double> temp{ thrust::sin(z) };
    temp += thrust::pow(g_e, z);
    return temp + p;
}
// Fractal 16: Weird3
__device__ thrust::complex<double> frmWeird3(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Pow(Complex.Multiply(Complex.Pow(Complex.Cos(Z), 3.0), Z), 2.0) + p;
    thrust::complex<double> temp{ thrust::cos(z) };
    temp *= temp;
    temp *= temp;
    temp *= z;
    return temp + p;
}
// Fractal 17: Weird4
__device__ thrust::complex<double> frmWeird4(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Pow(Complex.Multiply(Complex.Pow(Complex.Cos(res), 4.0), res), 2.0) + p;
    thrust::complex<double> temp{ thrust::pow(thrust::cos(z),4) };
    temp *= z;
    temp *= temp;
    return temp + p;
}
// Fractal 18: Weird5
__device__ thrust::complex<double> frmWeird5(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Pow(Complex.Multiply(Complex.Pow(Complex.Sin(res), 4.0), res), 2.0) + p;
    thrust::complex<double> temp{ thrust::pow(thrust::sin(z),4) * z };
    temp *= temp;
    return temp + p;
}
// Fractal 19: Weird6
__device__ thrust::complex<double> frmWeird6(thrust::complex<double> z, thrust::complex<double> p)
{
    // This one is really cool.  This is art.
    // Complex cosReal = new Complex(Math.Cos(res.Real), res.Imaginary);
    // return Complex.Pow(Complex.Multiply(Complex.Pow(cosReal, 4.0), res), 2.0) + p;
    thrust::complex<double> cosREAL{ cos(z.real()), z.imag() };
    thrust::complex<double> temp{ thrust::pow(thrust::pow(cosREAL, 4) * z, 2) };
    return temp + p;
}
// Fractal 20: Weird7
__device__ thrust::complex<double> frmWeird7(thrust::complex<double> z, thrust::complex<double> p)
{
    // This one is really cool.  This is art.
    // Complex cosReal = new Complex(Math.Cos(res.Real), Math.Sin(res.Imaginary));
    // return Complex.Pow(Complex.Multiply(Complex.Pow(Complex.Cos(res), 4.0), cosReal), 2.0) + p;
    thrust::complex<double> cosREALsinIMAG{ cos(z.real()), sin(z.imag()) };
    thrust::complex<double> temp{thrust::cos(z)};
    temp = thrust::pow(thrust::pow(temp,4) * cosREALsinIMAG,2);
    return temp + p;
}
// Fractal 21: Weird8
__device__ thrust::complex<double> frmWeird8(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Add(Complex.Pow(Complex.Divide(res,Complex.Pow(new Complex(1, -0.5),res)), 2.0), p);
    thrust::complex<double> temp{ 1, -0.5 };
    temp = thrust::pow(temp, z);
    temp = thrust::pow(z / temp, 2);
    return temp + p;
}
// Fractal 22: Weird9
__device__ thrust::complex<double> frmWeird9(thrust::complex<double> z, thrust::complex<double> p)
{
    /*
    double oneThird = 1.0 / 3.0;
    return Complex.Subtract(
        Complex.Add(
            Complex.Subtract(
                Complex.Multiply(0.5,Complex.Pow(res,2)),
                Complex.Multiply(oneThird, Complex.Pow(res, 3))
            ),
            res
        ),
    p);
    */
    const double oneThird{ 1.0 / 3.0 };
    thrust::complex<double> temp{ z * z * 0.5 };
    temp -= z * z * z * oneThird;
    temp += z;
    return temp - p;
}
// Fractal 23: Weird10
__device__ thrust::complex<double> frmWeird10(thrust::complex<double> z, thrust::complex<double> p)
{
    /*
    double oneThird = 1.0 / 3.0;
    return Complex.Subtract(
        Complex.Add(
            Complex.Subtract(
                Complex.Multiply(0.5, Complex.Pow(res, 2)),
                Complex.Multiply(oneThird, Complex.Pow(Complex.Cos(res), 3))
            ),
            res
        ),
    p);
    */
    const double oneThird{ 1.0 / 3.0 };
    thrust::complex<double> temp{ z * z * 0.5 };
    temp -= thrust::pow(thrust::cos(z),3) * oneThird;
    temp += z;
    return temp - p;
}
// Fractal 24: Weird11
__device__ thrust::complex<double> frmWeird11(thrust::complex<double> z, thrust::complex<double> p)
{
    /*
    double oneThird = 1.0 / 3.0;
    return Complex.Add(
        Complex.Pow((1 / Complex.Tan(res)),Complex.Multiply(oneThird,res)),
    p);
    */
    const double oneThird{ 1.0 / 3.0 };
    thrust::complex<double> temp{ z * oneThird };
    temp = thrust::pow((1 / thrust::tan(z)), temp);
    return temp + p;
}
// Fractal 25: Mandelbrot4th
__device__ thrust::complex<double> frmMandelbrot4th(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Pow(Complex.Add(Complex.Pow(res, 2.0), p),4.0);
    thrust::complex<double> temp{ z * z };
    temp += p;
    return thrust::pow(temp, 4);
}
// Fractal 26: Mandelbrot8th
__device__ thrust::complex<double> frmMandelbrot8th(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Pow(Complex.Add(Complex.Pow(res, 2.0), p),8.0);
    thrust::complex<double> temp{ z * z };
    temp += p;
    return thrust::pow(temp,8);
}
// Fractal 27: BurningShip
__device__ thrust::complex<double> frmBurningShip(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Add(Complex.Pow(new Complex(Math.Abs(res.Real),-Math.Abs(res.Imaginary)) , 2.0), p);
    thrust::complex<double> temp{ cuda::std::abs(z.real()), -cuda::std::abs(z.imag())};
    temp *= temp;
    //temp *= z;
    return temp + p;
}
// Fracta 29: Experiment2
__device__ thrust::complex<double> frmExperiment2(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Add(Complex.Pow(new Complex(Math.Abs(res.Real),-Math.Abs(res.Imaginary)) , 2.0), p);
    thrust::complex<double> temp{ cuda::std::abs(z.real()), z.imag()};
    temp = temp + thrust::pow(z,2);
    //temp *= z;
    return temp + p;
}
// Fracta 30: Experiment3
__device__ thrust::complex<double> frmExperiment3(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Add(Complex.Pow(new Complex(Math.Abs(res.Real),-Math.Abs(res.Imaginary)) , 2.0), p);
    thrust::complex<double> temp{ cuda::std::abs(z.real()), z.imag() };
    temp = temp + thrust::pow(z, 2) + thrust::pow(z, 3) / 1.8;
    //temp *= z;
    return temp + p;
}
// Fracta 31: Experiment4
__device__ thrust::complex<double> frmExperiment4(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Add(Complex.Pow(new Complex(Math.Abs(res.Real),-Math.Abs(res.Imaginary)) , 2.0), p);
    thrust::complex<double> temp{ thrust::pow(z,1.333333333) };
    //temp *= z;
    return temp + thrust::cos(p);
}
// Fracta 32: Experiment5
__device__ thrust::complex<double> frmExperiment5(thrust::complex<double> z, thrust::complex<double> p)
{
    // return Complex.Add(Complex.Pow(new Complex(Math.Abs(res.Real),-Math.Abs(res.Imaginary)) , 2.0), p);
    thrust::complex<double> temp{ thrust::pow(z,1.666666666) };
    //temp *= z;
    return temp + thrust::sin(z + p);
}
// Fractal 33: Experiment 6
__device__ thrust::complex<double> frmExperiment6(thrust::complex<double> z, thrust::complex<double> p)
{
    return thrust::pow(z,3) - z + p;
}
// Fractal 34: CelticMandelbrot
__device__ thrust::complex<double> frmCelticMandelbrot(thrust::complex<double> z, thrust::complex<double> p)
{
    double zisqr = z.imag() * z.imag();
    double zrsqr = z.real() * z.real();
    double zi = z.real() * z.imag() * 2.0 + p.imag();
    double zr = cuda::std::abs(zrsqr - zisqr) + p.real();
    return thrust::complex<double>{zr, zi};
}
// Fractal 35: CelticMandelbar
__device__ thrust::complex<double> frmCelticMandelbar(thrust::complex<double> z, thrust::complex<double> p)
{
    double zisqr = z.imag() * z.imag();
    double zrsqr = z.real() * z.real();
    double zi = z.real() * z.imag() * -2.0 + p.imag();
    double zr = cuda::std::abs(zrsqr - zisqr) + p.real();
    return thrust::complex<double>{zr, zi};
}
// Fractal 36: CelticPerpendicular
__device__ thrust::complex<double> frmCelticPerpendicular(thrust::complex<double> z, thrust::complex<double> p)
{
    double zisqr = z.imag() * z.imag();
    double zrsqr = z.real() * z.real();
    double zi = cuda::std::abs(z.real()) * z.imag() * -2.0 + p.imag();
    double zr = cuda::std::abs(zrsqr - zisqr) + p.real();
    return thrust::complex<double>{zr, zi};
}
// Fractal 37: CelticHeart
__device__ thrust::complex<double> frmCelticHeart(thrust::complex<double> z, thrust::complex<double> p)
{
    double zisqr = z.imag() * z.imag();
    double zrsqr = z.real() * z.real();
    double zi = cuda::std::abs(z.real()) * z.imag() * 2.0 + p.imag();
    double zr = cuda::std::abs(zrsqr - zisqr) + p.real();
    return thrust::complex<double>{zr, zi};
}
// Fractal 38: Buffalo
__device__ thrust::complex<double> frmBuffalo(thrust::complex<double> z, thrust::complex<double> p)
{
    double zisqr = z.imag() * z.imag();
    double zrsqr = z.real() * z.real();
    double zi = cuda::std::abs(z.real() * z.imag()) * 2.0 + p.imag();
    double zr = cuda::std::abs(zrsqr - zisqr) + p.real();
    return thrust::complex<double>{zr, zi};
}
// Fractal 39: BuffaloPerpendicular
__device__ thrust::complex<double> frmBuffaloPerpendicular(thrust::complex<double> z, thrust::complex<double> p)
{
    double zisqr = z.imag() * z.imag();
    double zrsqr = z.real() * z.real();
    double zi = z.real() * cuda::std::abs(z.imag()) * 2.0 + p.imag();
    double zr = cuda::std::abs(zrsqr - zisqr) + p.real();
    return thrust::complex<double>{zr, zi};
}
// Fractal 40: MandelbrotPerpendicular
__device__ thrust::complex<double> frmMandelbrotPerpendicular(thrust::complex<double> z, thrust::complex<double> p)
{
    double zisqr = z.imag() * z.imag();
    double zrsqr = z.real() * z.real();
    double zi = cuda::std::abs(z.real()) * z.imag() * -2.0 + p.imag();
    double zr = zrsqr - zisqr + p.real();
    return thrust::complex<double>{zr, zi};
}

// ******** Lifesmith.com formulas *************
// https://www.lifesmith.com/formulas.html
// Fractal 41: Lifesmith3
__device__ thrust::complex<double> frmLifesmith3(thrust::complex<double> z, thrust::complex<double> p)
{
    thrust::complex<double> temp{ z * z + p };
    return temp / (z - p);
}
// Fractal 42: Lifesmith4
__device__ thrust::complex<double> frmLifesmith4(thrust::complex<double> z, thrust::complex<double> p)
{
    return z * z - z + p;
}
// Fractal 43: Lifesmith5
__device__ thrust::complex<double> frmLifesmith5(thrust::complex<double> z, thrust::complex<double> p)
{
    return (z * z * z) - (z * z) + z + p;
}
// Fractal 44: Lifesmith6Mod
__device__ thrust::complex<double> frmLifesmith6Mod(thrust::complex<double> z, thrust::complex<double> p)
{
    return (1.0 + p) * z - p * z * z + p;
}
// Fractal 45: Lifesmith7Mod
__device__ thrust::complex<double> frmLifesmith7Mod(thrust::complex<double> z, thrust::complex<double> p)
{
    // added + p on top
    return ((z * z * z) + p) / (1.0 + (z * z * p));
}
// Fractal 46: Lifesmith8
__device__ thrust::complex<double> frmLifesmith8(thrust::complex<double> z, thrust::complex<double> p)
{
    return (z - 1) * (z + 0.5) * (z * z - 1) + p;
}
// Fractal 47: Lifesmith9
__device__ thrust::complex<double> frmLifesmith9(thrust::complex<double> z, thrust::complex<double> p)
{
    return (z * z + 1 + p )/(z * z - 1 - p);
}
// Fractal 48: Lifesmith10
__device__ thrust::complex<double> frmLifesmith10(thrust::complex<double> z, thrust::complex<double> p)
{
    return thrust::pow(z,1.5) + p;
}
// Fractal 49: Lifesmith11
__device__ thrust::complex<double> frmLifesmith11(thrust::complex<double> z, thrust::complex<double> p)
{
    return thrust::exp(z) - p;
}

// Fractal 50: Lifesmith12
__device__ thrust::complex<double> frmLifesmith12(thrust::complex<double> z, thrust::complex<double> p)
{
    return z - 1 + thrust::exp(-z) * p;
}
// Fractal 51: Lifesmith13
__device__ thrust::complex<double> frmLifesmith13(thrust::complex<double> z, thrust::complex<double> p)
{
    return z * p - 1 + thrust::exp(-z) * p;
}
// Fractal 52: Lifesmith14Mod
__device__ thrust::complex<double> frmLifesmith14Mod(thrust::complex<double> z, thrust::complex<double> p)
{
    return (thrust::pow(z,5) * 4 + p) / ((5 * thrust::pow(z,4)) + p);
}
// Fractal 53: Lifesmith15Mod
__device__ thrust::complex<double> frmLifesmith15Mod(thrust::complex<double> z, thrust::complex<double> p)
{
    return (thrust::pow(z,7) * 6 + (p / 3)) / (thrust::pow(z + p,6) * 7);
}
// Fractal 54: Lifesmith16
__device__ thrust::complex<double> frmLifesmith16(thrust::complex<double> z, thrust::complex<double> p)
{
    return z * z * thrust::exp(-z) + p;
}
// Fractal 55: Lifesmith17
__device__ thrust::complex<double> frmLifesmith17(thrust::complex<double> z, thrust::complex<double> p)
{
    return z * z * thrust::pow(thrust::exp(z),-2) + p;
}
// Fractal 56: Lifesmith18
__device__ thrust::complex<double> frmLifesmith18(thrust::complex<double> z, thrust::complex<double> p)
{
    return z * thrust::exp(-z) + p;
}
// Fractal 57: Lifesmith19
__device__ thrust::complex<double> frmLifesmith19(thrust::complex<double> z, thrust::complex<double> p)
{
    return p * thrust::exp(-z) + z * z;
}
// Fractal 58: Lifesmith20
__device__ thrust::complex<double> frmLifesmith20(thrust::complex<double> z, thrust::complex<double> p)
{
    return z * z * z + z + p;
}
// Fractal 59: Lifesmith21
__device__ thrust::complex<double> frmLifesmith21(thrust::complex<double> z, thrust::complex<double> p)
{
    return z * z * z * z + z + p;
}
// Fractal 60: Lifesmith22
__device__ thrust::complex<double> frmLifesmith22(thrust::complex<double> z, thrust::complex<double> p)
{
    return z * z * z * z + z * z * p + p;
}
// Fractal 61: Lifesmith23
__device__ thrust::complex<double> frmLifesmith23(thrust::complex<double> z, thrust::complex<double> p)
{
    return z * z * cos(z.real()) + z * p * cos(z.imag()) + p;
}
// Fractal 62: Lifesmith24Mod
__device__ thrust::complex<double> frmLifesmith24Mod(thrust::complex<double> z, thrust::complex<double> p)
{
    return z.real() == 0.0 && z.imag() == 0.0 ? p : thrust::pow(2.0, z) * p * z * z;
}
// Fractal 63: Lifesmith25
__device__ thrust::complex<double> frmLifesmith25(thrust::complex<double> z, thrust::complex<double> p)
{
    return thrust::pow(z,5.0) - thrust::pow(z,3.0) + z + p;
}
// Fractal 64: Lifesmith26
__device__ thrust::complex<double> frmLifesmith26(thrust::complex<double> z, thrust::complex<double> p)
{
    return thrust::pow((z * z + p),2.0) + z + p;
}
// Fractal 65: Lifesmith27
__device__ thrust::complex<double> frmLifesmith27(thrust::complex<double> z, thrust::complex<double> p)
{
    return thrust::pow(z + thrust::sin(z), 2.0) + p;
}
// Fractal 66: Lifesmith28
__device__ thrust::complex<double> frmLifesmith28(thrust::complex<double> z, thrust::complex<double> p)
{
    return p * thrust::exp(z);
}
// Fractal 67: Lifesmith29
__device__ thrust::complex<double> frmLifesmith29(thrust::complex<double> z, thrust::complex<double> p)
{
    return z * z + p * p * p;
}
// Fractal 68: Lifesmith30
__device__ thrust::complex<double> frmLifesmith30(thrust::complex<double> z, thrust::complex<double> p)
{
    return p * thrust::exp(z * p);
}
// Fractal 69: Lifesmith31
__device__ thrust::complex<double> frmLifesmith31(thrust::complex<double> z, thrust::complex<double> p)
{
    return z * z * cos(z.real()) + p * z * sin(z.imag()) + p;
}
// Fractal 70: Lifesmith32
__device__ thrust::complex<double> frmLifesmith32(thrust::complex<double> z, thrust::complex<double> p)
{
    return z.real() == 0.0 && z.imag() == 0.0 ? p : p * z * z + z * p * p;
}
// Fractal 71: Lifesmith33
__device__ thrust::complex<double> frmLifesmith33(thrust::complex<double> z, thrust::complex<double> p)
{
    return thrust::exp(thrust::cos(p * z));
}
// Fractal 72: Lifesmith34 - j0 is the regular Bessel function order 0
__device__ thrust::complex<double> frmLifesmith34(thrust::complex<double> z, thrust::complex<double> p)
{
    return pow(1.0 + j0(z.real()),2.0) + pow(j0(z.imag()) + p, 2.0);
}
// Fractal 73: Lifesmith35
__device__ thrust::complex<double> frmLifesmith35(thrust::complex<double> z, thrust::complex<double> p)
{
    return (thrust::sin(z) + thrust::cos(z)) * p;
}
// Fractal 74: Lifesmith36
__device__ thrust::complex<double> frmLifesmith36(thrust::complex<double> z, thrust::complex<double> p)
{
    return z.real() == 0.0 && z.imag() == 0.0 ? p : thrust::pow(z, -0.5) + p;
}
// Fractal 75: Lifesmith37
__device__ thrust::complex<double> frmLifesmith37(thrust::complex<double> z, thrust::complex<double> p)
{
    return (1 - z) * z * p;
}
// Fractal 76: Lifesmith38
__device__ thrust::complex<double> frmLifesmith38(thrust::complex<double> z, thrust::complex<double> p)
{
    return z.real() == 0.0 && z.imag() == 0.0 ? p : thrust::pow(p, 2.0) * z * (1 - z);
}
// Fractal 77: Lifesmith39
__device__ thrust::complex<double> frmLifesmith39(thrust::complex<double> z, thrust::complex<double> p)
{
    return thrust::pow((z * z + p), 2.0)/(z - p);
}
// Fractal 78: Lifesmith40
__device__ thrust::complex<double> frmLifesmith40(thrust::complex<double> z, thrust::complex<double> p)
{
    return z.real() == 0.0 && z.imag() == 0.0 ? p : thrust::pow((z + thrust::sin(z)),2.0) + thrust::pow(z,-0.5) + p;
}
// Fractal 79: Lifesmith41
__device__ thrust::complex<double> frmLifesmith41(thrust::complex<double> z, thrust::complex<double> p)
{
    return (thrust::sin(z) + thrust::cos(z)) * p * (z * z * z + z + p);
}
// Fractal 80: NovaStar
__device__ thrust::complex<double> frmNovaStar(thrust::complex<double> z, thrust::complex<double> p)
{
    if (z.real() == 0.0 && z.imag() == 0.0) return p * 0.04;
    double temp = thrust::abs(z);
    return (temp * temp + 1.0) / (z * z + p);
}
// Fractal 81: Newton-Raphson Z^3
// https://graphicmaths.com/fractals/escape-time-fractals/newton-fractal/
__device__ thrust::complex<double> frmNewton(thrust::complex<double> z, thrust::complex<double> p)
{
    if (z.real() == 0.0 && z.imag() == 0.0) return thrust::complex<double>{1.0,0.0};
    return z - (thrust::pow(z, 3.0) - 1) / (3.0 * z * z) + p;
}


__global__ void setTheDeviceGlobals(double juliaCenterX, double juliaCenterY, int maxIts, double limit, int fractalFormulaID, int N, int ismove)
{
    g_juliaCenterX = juliaCenterX;
    g_juliaCenterY = juliaCenterY;
    g_maxIts = maxIts;
    g_limit = limit;
    g_N = N;
    g_ismove = ismove;
    switch (fractalFormulaID)
    {
    case 1:
        g_alg = frmSinPow2;
        break;
    case 2:
        g_alg = frmSinPow3;
        break;
    case 3:
        g_alg = frmSinPow4;
        break;
    case 4:
        g_alg = frmSinPow2Div1;
        break;
    case 5:
        g_alg = frmCosPow2;
        break;
    case 6:
        g_alg = frmCosPow2MandelbrotHybrid;
        break;
    case 7:
        g_alg = frmCosPow2AbsRIMandelbrotHybrid;
        break;
    case 8:
        g_alg = frmCosPow2AbsRI;
        break;
    case 9:
        g_alg = frmExperiment1;
        break;
    case 10:
        g_alg = frmCosPow2AbsRIPow4;
        break;
    case 11:
        g_alg = frmCosPow2SinPow2Hybrid;
        break;
    case 12:
        g_alg = frmCosPow3;
        break;
    case 13:
        g_alg = frmWeirdLim5;
        break;
    case 14:
        g_alg = frmWeirdLim9;
        break;
    case 15:
        g_alg = frmWeird2;
        break;
    case 16:
        g_alg = frmWeird3;
        break;

    case 17:
        g_alg = frmWeird4;
        break;
    case 18:
        g_alg = frmWeird5;
        break;
    case 19:
        g_alg = frmWeird6;
        break;
    case 20:
        g_alg = frmWeird7;
        break;
    case 21:
        g_alg = frmWeird8;
        break;
    case 22:
        g_alg = frmWeird9;
        break;
    case 23:
        g_alg = frmWeird10;
        break;
    case 24:
        g_alg = frmWeird11;
        break;
    case 25:
        g_alg = frmMandelbrot4th;
        break;
    case 26:
        g_alg = frmMandelbrot8th;
        break;
    case 27:
        g_alg = frmBurningShip;
        break;
    case 28:
        g_alg = frmSinAbsRI;
        break;
    case 29:
        g_alg = frmExperiment2;
        break;
    case 30:
        g_alg = frmExperiment3;
        break;
    case 31:
        g_alg = frmExperiment4;
        break;
    case 32:
        g_alg = frmExperiment5;
        break;
    case 33:
        g_alg = frmExperiment6;
        break;
    case 34:
        g_alg = frmCelticMandelbrot;
        break;
    case 35:
        g_alg = frmCelticMandelbar;
        break;
    case 36:
        g_alg = frmCelticPerpendicular;
        break;
    case 37:
        g_alg = frmCelticHeart;
        break;
    case 38:
        g_alg = frmBuffalo;
        break;
    case 39:
        g_alg = frmBuffaloPerpendicular;
        break;
    case 40:
        g_alg = frmMandelbrotPerpendicular;
        break;
    case 41:
        g_alg = frmLifesmith3;
        break;
    case 42:
        g_alg = frmLifesmith4;
        break;
    case 43:
        g_alg = frmLifesmith5;
        break;
    case 44:
        g_alg = frmLifesmith6Mod;
        break;
    case 45:
        g_alg = frmLifesmith7Mod;
        break;
    case 46:
        g_alg = frmLifesmith8;
        break;
    case 47:
        g_alg = frmLifesmith9;
        break;
    case 48:
        g_alg = frmLifesmith10;
        break;
    case 49:
        g_alg = frmLifesmith11;
        break;
    case 50:
        g_alg = frmLifesmith12;
        break;
    case 51:
        g_alg = frmLifesmith13;
        break;
    case 52:
        g_alg = frmLifesmith14Mod;
        break;
    case 53:
        g_alg = frmLifesmith15Mod;
        break;
    case 54:
        g_alg = frmLifesmith16;
        break;
    case 55:
        g_alg = frmLifesmith17;
        break;
    case 56:
        g_alg = frmLifesmith18;
        break;
    case 57:
        g_alg = frmLifesmith19;
        break;
    case 58:
        g_alg = frmLifesmith20;
        break;
    case 59:
        g_alg = frmLifesmith21;
        break;
    case 60:
        g_alg = frmLifesmith22;
        break;
    case 61:
        g_alg = frmLifesmith23;
        break;
    case 62:
        g_alg = frmLifesmith24Mod;
        break;
    case 63:
        g_alg = frmLifesmith25;
        break;
    case 64:
        g_alg = frmLifesmith26;
        break;
    case 65:
        g_alg = frmLifesmith27;
        break;
    case 66:
        g_alg = frmLifesmith28;
        break;
    case 67:
        g_alg = frmLifesmith29;
        break;
    case 68:
        g_alg = frmLifesmith30;
        break;
    case 69:
        g_alg = frmLifesmith31;
        break;
    case 70:
        g_alg = frmLifesmith32;
        break;
    case 71:
        g_alg = frmLifesmith33;
        break;
    case 72:
        g_alg = frmLifesmith34; // Bessel j0
        break;
    case 73:
        g_alg = frmLifesmith35;
        break;
    case 74:
        g_alg = frmLifesmith36;
        break;
    case 75:
        g_alg = frmLifesmith37;
        break;
    case 76:
        g_alg = frmLifesmith38;
        break;
    case 77:
        g_alg = frmLifesmith39;
        break;
    case 78:
        g_alg = frmLifesmith40;
        break;
    case 79:
        g_alg = frmLifesmith41;
        break;
    case 80:
        g_alg = frmNovaStar;
        break;
    case 81:
        g_alg = frmNewton;
        break;
    default:
        g_alg = frmMandelbrot;
        break;
    }
}

__global__ void algMap(const double* __restrict__ realCoords, const double* __restrict__ imagCoords, int* __restrict__ iterations)
{
    // Calculate global thread ID
    int tid = blockIdx.x * blockDim.x + threadIdx.x;

    // Boundary check
    if (tid < g_N)
    {
        // if we're doing a move UP DOWN LEFT or RIGHT, skip the non-zero iterations
        if (g_ismove == 1 && iterations[tid] != 0) return;
        thrust::complex<double> z{ 0.0,0.0 };
        const thrust::complex<double> p{ realCoords[tid], imagCoords[tid] };
        int i{ 0 };
        while (i < g_maxIts)
        {
            thrust::complex<double> temp{ g_alg(z, p)};
            if (thrust::abs(temp) > g_limit * g_limit) break;   // magnitude escapes the limit?
            z = temp;
            ++i;
        }
        iterations[tid] = i;
    }
}

__global__ void algJulia(const double* __restrict__ realCoords, const double* __restrict__ imagCoords, int* __restrict__ iterations)
{
    // Calculate global thread ID
    int tid = blockIdx.x * blockDim.x + threadIdx.x;

    // Boundary check
    if (tid < g_N)
    {
        // if we're doing a move UP DOWN LEFT or RIGHT, skip the non-zero iterations
        if (g_ismove == 1 && iterations[tid] != 0) return;
        thrust::complex<double> z{ realCoords[tid], imagCoords[tid] };
        const thrust::complex<double> p{ g_juliaCenterX, g_juliaCenterY };
        int i{ 0 };
        while (i < g_maxIts)
        {
            thrust::complex<double> temp{ g_alg(z, p) };
            if (thrust::abs(temp) > g_limit * g_limit) break;   // magnitude escapes the limit?
            z = temp;
            ++i;
        }
        iterations[tid] = i;
    }
}

// Pretty leaves and scary hairy eyeball videos in areas near black holes
__global__ void algTheCalcmasterTwist(const double* __restrict__ realCoords, const double* __restrict__ imagCoords, int* __restrict__ iterations)
{
    // Calculate global thread ID
    int tid = blockIdx.x * blockDim.x + threadIdx.x;

    // Boundary check
    if (tid < g_N)
    {
        // if we're doing a move UP DOWN LEFT or RIGHT, skip the non-zero iterations
        if (g_ismove == 1 && iterations[tid] != 0) return;
        thrust::complex<double> z{ realCoords[tid], imagCoords[tid] };
        const thrust::complex<double> p{ g_juliaCenterX, g_juliaCenterY };
        thrust::complex<double> temp{ 0.0, 0.0 };
        int i{ 0 };
        while (i < g_maxIts)
        {
            temp = { g_alg(z, p) };
            if (thrust::abs(temp) > g_limit * g_limit) break;   // magnitude escapes the limit?
            z = temp;
            ++i;
        }
        // iterations[tid] = i;
        
        // C#
        // return (Int32)
        // (
        //   Complex.Tanh(
        //     Complex.Subtract(
        //       Math.Min(res.Real, res.Imaginary), p
        //     )
        //   )
        //   .Magnitude * its
        // ) % numColorsInPalette;
        if (temp.real() > temp.imag())
            temp = thrust::complex<double>{ temp.real() - p.real(), p.imag() * -1 };
        else
            temp = thrust::complex<double>{ temp.imag() - p.real(), p.imag() * -1 };
        
        //temp = thrust::tanh(temp);
        thrust::complex<double> sinhTemp{ thrust::sinh(temp) };
        thrust::complex<double> coshTemp{ thrust::cosh(temp) };
        temp = sinhTemp / coshTemp;
        double tanHMagnitude{ thrust::abs(temp) };
        //if (tanHMagnitude > 5000.0) tanHMagnitude = 200.0;
        if (tanHMagnitude < 1) tanHMagnitude *= 100;
        int tanHMagnitudeNarrow{__double2int_rz(tanHMagnitude) * i};
        if (tanHMagnitudeNarrow < 0) tanHMagnitudeNarrow *= -1;
        //if (tanHMagnitude < 2147483647.0 * i) tanHMagnitudeNarrow = int(tanHMagnitude);
        //tanHMagnitudeNarrow /= 2;
        iterations[tid] = tanHMagnitudeNarrow % 5000;//tanHMagnitudeNarrow % 200; // for now, g_colorsInPalette is a constant (1024)
    }
}

// --- reserved name (for now it's just a regular julia set with no spice)
__global__ void algAirOnAJuliaString(const double* __restrict__ realCoords, const double* __restrict__ imagCoords, int* __restrict__ iterations)
{
    // Calculate global thread ID
    int tid = blockIdx.x * blockDim.x + threadIdx.x;

    // Boundary check
    if (tid < g_N)
    {
        thrust::complex<double> z{ realCoords[tid], imagCoords[tid] };
        const thrust::complex<double> p{ g_juliaCenterX, g_juliaCenterY };
        int i{ 0 };
        while (i < g_maxIts)
        {
            thrust::complex<double> temp{ g_alg(z, p) };
            if (thrust::abs(temp) > g_limit * g_limit) break;   // magnitude escapes the limit?
            z = temp;
            ++i;
        }
        iterations[tid] = i;
    }
}


int TheCalcmaster(double* host_re, double* host_im, int* host_its, double limit, int maxIts, int fractalID, size_t numElements, int mode = 0, double juliaCenterX = 0.0, double juliaCenterY = 0.0, int ismove = 0)
{
    // Error code to check return values for CUDA calls
    cudaError_t err = cudaSuccess;
    // Device memory allocation
    size_t doubleVectorSize = numElements * sizeof(double);
    size_t intVectorSize = numElements * sizeof(int);
    // Real coords input vector
    double* device_re = NULL;
    err = cudaMalloc((void**)&device_re, doubleVectorSize);
    if (err != cudaSuccess) return 4;
    // Imaginary coords input vector
    double* device_im = NULL;
    err = cudaMalloc((void**)&device_im, doubleVectorSize);
    if (err != cudaSuccess) return 4;
    // Iterations output vector
    int* device_its = NULL;
    err = cudaMalloc((void**)&device_its, intVectorSize);
    if (err != cudaSuccess) return 4;
    // Copy real coords to device
    err = cudaMemcpy(device_re, host_re, doubleVectorSize, cudaMemcpyHostToDevice);
    if (err != cudaSuccess) return 5;
    // Copy imaginary coords to device
    err = cudaMemcpy(device_im, host_im, doubleVectorSize, cudaMemcpyHostToDevice);
    if (err != cudaSuccess) return 5;
    // Copy the iterations to the device if we're doing a move
    if (ismove == 1)
    {
        err = cudaMemcpy(device_its, host_its, intVectorSize, cudaMemcpyHostToDevice);
        if (err != cudaSuccess) return 5;
    }
    // Set up the thread blocks
    int threadsPerBlock{ 512 };
    int blocksPerGrid = (static_cast<int>(numElements) + threadsPerBlock - 1) / threadsPerBlock;
    // Global vars
    //setTheDeviceGlobals(double juliaCenterX, double juliaCenterY, int maxIts, double limit, int fractalFormulaID, int N)
    setTheDeviceGlobals<<<1, 1>>>(juliaCenterX, juliaCenterY, maxIts, limit, fractalID, static_cast<int>(numElements), ismove);
    //RUN THE KERNEL (const double* __restrict__ realCoords, const double* __restrict__ imagCoords, int* __restrict__ iterations)
    switch (mode)
    {
    case 1:
        algJulia<<<blocksPerGrid, threadsPerBlock>>>(device_re, device_im, device_its);
        break;
    case 2:
        algTheCalcmasterTwist<<<blocksPerGrid, threadsPerBlock>>>(device_re, device_im, device_its);
        break;
    case 3:
        algAirOnAJuliaString<<<blocksPerGrid, threadsPerBlock>>>(device_re, device_im, device_its);
        break;
    default:
        algMap<<<blocksPerGrid, threadsPerBlock>>>(device_re, device_im, device_its);
        break;
    }
    err = cudaGetLastError();
    if (err != cudaSuccess) return 6;
    // Copy device iterations back to m_iterations
    err = cudaMemcpy(host_its, device_its, intVectorSize, cudaMemcpyDeviceToHost);
    if (err != cudaSuccess) return 7;
    // Step in, the janitor.
    err = cudaFree(device_re);
    if (err != cudaSuccess) return 8;
    err = cudaFree(device_im);
    if (err != cudaSuccess) return 8;
    err = cudaFree(device_its);
    if (err != cudaSuccess) return 8;
    return 0;
}
