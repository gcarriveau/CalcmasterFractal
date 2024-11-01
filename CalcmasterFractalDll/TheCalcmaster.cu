// Copyright 2024 - Gregory James Carriveau a.k.a. fractalsnakes840
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files(the “Software”), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and /or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in all copies
// or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
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

// Global device variables
__device__ const int g_colorsInPalette{ 1024 };
__device__ double g_juliaCenterX;
__device__ double g_juliaCenterY;
__device__ int    g_maxIts;
__device__ double g_limit;
__device__ int    g_N;
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
// Fractal 9: Experiment number 1 ((1 - z^3) / 6) / ((1 - z - z^2) / 2)^2 + p
__device__ thrust::complex<double> frmExperiment1(thrust::complex<double> z, thrust::complex<double> p)
{
    thrust::complex<double> temp{ (1 - z * z * z) / 6 };
    thrust::complex<double> temp2{ (1 - z - z * z) / 2 };
    temp /= temp2 * temp2;
    return temp + p;
}

__global__ void setTheDeviceGlobals(double juliaCenterX, double juliaCenterY, int maxIts, double limit, int fractalFormulaID, int N)
{
    g_juliaCenterX = juliaCenterX;
    g_juliaCenterY = juliaCenterY;
    g_maxIts = maxIts;
    g_limit = limit;
    g_N = N;
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
        //return (Int32)(Complex.Tanh(Complex.Subtract(Math.Min(res.Real, res.Imaginary), p)).Magnitude * its) % numColorsInPalette;
        thrust::complex<double> minReImLessP{ z.real() > z.imag() ? z.imag() - p : z.real() - p };
        double tanHMagnitude{ tanh(thrust::abs(minReImLessP)) };
        iterations[tid] = static_cast<int>(tanHMagnitude) * i % g_colorsInPalette; // for now, g_colorsInPalette is a constant (1024)
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


int TheCalcmaster(double* host_re, double* host_im, int* host_its, double limit, int maxIts, int fractalID, size_t numElements, int mode = 0, double juliaCenterX = 0.0, double juliaCenterY = 0.0)
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
    // Set up the thread blocks
    int threadsPerBlock{ 512 };
    int blocksPerGrid = (static_cast<int>(numElements) + threadsPerBlock - 1) / threadsPerBlock;
    // Global vars
    //setTheDeviceGlobals(double juliaCenterX, double juliaCenterY, int maxIts, double limit, int fractalFormulaID, int N)
    setTheDeviceGlobals<<<1, 1>>>(juliaCenterX, juliaCenterY, maxIts, limit, fractalID, static_cast<int>(numElements));
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
