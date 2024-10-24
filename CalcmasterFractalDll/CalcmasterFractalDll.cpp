// CalcmasterFractalDll.cpp : Defines the exported functions for the DLL.

#include "pch.h"
#include "framework.h"
#include "CalcmasterFractalDll.h"


// This is the constructor of a class that has been exported.
FractalGenerator::FractalGenerator()
{
    m_iterations.assign(size_t(m_height) * size_t(m_width), 0);
    m_iterations[0] = 55;
    return;
}

int FractalGenerator::add(int x, int y)
{
    return x + y;
}

void FractalGenerator::setDimensions(int height, int width)
{
    if (m_height == height && m_width == width) return;
    m_height = height;
    m_width = width;
    m_iterations.assign(size_t(m_height) * size_t(m_width), 0);
}

int FractalGenerator::getIterationsAtIndex(size_t index)
{
    return m_iterations.at(index);
}
