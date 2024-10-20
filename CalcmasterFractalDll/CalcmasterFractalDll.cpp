// CalcmasterFractalDll.cpp : Defines the exported functions for the DLL.

#include "pch.h"
#include "framework.h"
#include "CalcmasterFractalDll.h"


// This is the constructor of a class that has been exported.
FractalGenerator::FractalGenerator()
{
    return;
}

int FractalGenerator::add(int x, int y)
{
    return x + y;
}

void FractalGenerator::setDimensions(int height, int width)
{
    this->m_height = height;
    this->m_width = width;
    this->m_iterations.resize(size_t(height) * size_t(width));
}

void FractalGenerator::getIterations(int** ppInt, int* pCount)
{
    // arr points to the first element of the continguous int data of m_iterations
    int* arr = this->m_iterations.data();
    ppInt = &arr;
    // number of elements in the vector
    int arrLen{ this->m_height * this->m_width };
    pCount = &arrLen;
}
