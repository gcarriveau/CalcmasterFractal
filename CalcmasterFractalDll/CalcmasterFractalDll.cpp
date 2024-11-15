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

// CalcmasterFractalDll.cpp : Defines the exported functions for the DLL.
#include "pch.h"
#include "framework.h"
#include "CalcmasterFractalDll.h"

FractalGenerator::FractalGenerator()
{
    initializeVectors();

    // Read the whole fractals.json file, from the root directory of CalcmasterFractal.exe, into a string.
    std::ifstream file("fractals.json");
    if (!file.is_open())
    {
        m_lastError = 1;
        return;
    }
    std::string fileContent((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
    file.close();

    // m_fractalsDocument
    // nlohmann JSON DOM of fractals.json file loaded in the constructor.
    // fractals.json file is expected in the root folder of the Calcmaster.exe
    // documentation: https://json.nlohmann.me/api/basic_json/parse/#parameters
    nJson fractalsDocument = nJson::parse(fileContent);
    if (fractalsDocument.is_discarded())
    {
        m_lastError = 2;
        return;
    }

    fillFracParams(fractalsDocument);
}

int FractalGenerator::getLastErrorCode()
{
    return m_lastError;
}

int FractalGenerator::add(int x, int y)
{
    return x + y;
}

void FractalGenerator::setDimensions(int height, int width)
{
    // Exit if the current height and width values are the same.
    if (m_height == height && m_width == width) return;

    // Exit if either of the height or width parameters is invalid.
    if (height * width <= 0)
    {
        m_lastError = 3;
        return;
    }
    
    // Otherwise, resize the vectors.
    m_height = height;
    m_width = width;
    m_vector_length = size_t(m_height) * size_t(m_width);
    initializeVectors();
}

void FractalGenerator::setJuliaCenter(double jcX, double jcY)
{
    m_juliaCenterX = jcX;
    m_juliaCenterY = jcY;
}

int FractalGenerator::getIterationsAtIndex(size_t index)
{
    return m_iterations.at(index);
}

int* FractalGenerator::getIterations()
{
    return m_iterations.data();
}


void FractalGenerator::selectFractalFormula(int fractalFormulaID)
{
    fracparams fp = getFracParams(fractalFormulaID);
    m_id = fp.id;
    m_name = fp.name;
    m_centerX = m_mode == 0 ? fp.centerX : 0.0;
    m_centerY = m_mode == 0 ? fp.centerY : 0.0;
    m_radius = fp.radius;
    m_limit = fp.limit;
    m_kernel = fp.kernel;
    // reset the points
    generatePoints();
}

double FractalGenerator::getRealAt(size_t x)
{
    return m_re.at(x);
}

double* FractalGenerator::getReals()
{
    return m_re.data();
}

double FractalGenerator::getImaginaryAt(size_t x)
{
    return m_re.at(x);
}

double* FractalGenerator::getImaginaries()
{
    return m_im.data();
}

void FractalGenerator::setLimit(double limit)
{
    if (limit <= 0.0) return;
    m_limit = limit;
}
double FractalGenerator::getLimit()
{
    return m_limit;
}

int FractalGenerator::calculateMap(int maxIterations, int ismove)
{
    m_maxIts = maxIterations;
    // TheCalcmaster(double* host_re, double* host_im, int* host_its, double limit, int maxIts, int fractalID, size_t numElements, int mode = 0, double juliaCenterX = 0.0, double juliaCenterY = 0.0)
    m_lastError = TheCalcmaster(m_re.data(), m_im.data(), m_iterations.data(), m_limit, m_maxIts, m_id, m_vector_length, m_mode, m_juliaCenterX, m_juliaCenterY, ismove);
    return m_lastError;
}

int FractalGenerator::zoomInAtPoint(int col, int row)
{
    if (m_lastError) return m_lastError;
    // calculate new center and decrease the radius by 1.5x
    m_centerX = m_left + (m_inc * col);
    m_centerY = m_top - (m_inc * row);
    m_radius /= 1.5;
    // update the points
    generatePoints();
    return m_lastError;
}

int FractalGenerator::zoomOut()
{
    if (m_lastError) return m_lastError;
    m_radius *= 1.5;
    // update the points
    generatePoints();
    return m_lastError;
}

int FractalGenerator::move(int d)
{
    if (m_lastError) return m_lastError;
    int numPixels = 10;
    double p2move = m_inc * static_cast<double>(numPixels);
    // Temp vector filled with zeros for shifting the iterations UP DOWN LEFT or RIGHT
    // we move by numPixels (i.e. UP DOWN rows or LEFT RIGHT columns)
    std::vector<int> temp_iterations(m_vector_length);
    switch (d)
    {
    case Direction::UP:
    {
        // shift the iterations values to the right by 10 rows
        size_t offset{ static_cast<size_t>(m_width * numPixels) };
        for (size_t i{ 0 }; i < m_vector_length - offset; ++i)
            temp_iterations[i + offset] = m_iterations[i];
        m_iterations = temp_iterations;
        // regenerate the points
        m_centerY += p2move;
        generatePoints();
        return m_lastError;
    }
    case Direction::DOWN:
    {
        // shift the iterations values to the left by 10 rows
        size_t offset{ static_cast<size_t>(m_width * numPixels) };
        for (size_t i{ offset }; i < m_vector_length; ++i)
            temp_iterations[i - offset] = m_iterations[i];
        m_iterations = temp_iterations;
        // regenerate the points
        m_centerY -= p2move;
        generatePoints();
        return m_lastError;
    }
    case Direction::LEFT:
    {
        // move every row to the right by numPixels
        for (size_t row{ 0 }; row < m_height; ++row)
            for (size_t col{ 0 }; col < static_cast<size_t>(m_width - numPixels); ++col)
                temp_iterations[m_width * row + col + numPixels] = m_iterations[m_width * row + col];
        m_iterations = temp_iterations;
        // regenerate the points
        m_centerX -= p2move;
        generatePoints();
        return m_lastError;
    }
    case Direction::RIGHT:
    {
        // move every row to the left by numPixels
        for (size_t row{ 0 }; row < m_height; ++row)
            for (size_t col{ static_cast<size_t>(numPixels) }; col < m_width; ++col)
                temp_iterations[m_width * row + col - numPixels] = m_iterations[m_width * row + col];
        m_iterations = temp_iterations;
        // regenerate the points
        m_centerX += p2move;
        generatePoints();
        return m_lastError;
    }
    default:
        m_lastError = 9;
        return 9;
    }
}

void FractalGenerator::setMode(int mode, int mouseClickX, int mouseClickY)
{
    // if mode hasn't changed, don't do anything.
    if (mode == m_mode) return;

    if (m_mode == 0)
    {
        // switching from main fractal to a julia flavor mode
        // make a backup of the main fractal state
        m_mapBackup.centerX = m_centerX;
        m_mapBackup.centerY = m_centerY;
        m_mapBackup.inc = m_inc;
        m_mapBackup.left = m_left;
        m_mapBackup.top = m_top;
        m_mapBackup.maxIts = m_maxIts;
        m_mapBackup.radius = m_radius;

        // set the julia set center according to the pixel that was clicked upon
        m_juliaCenterX = m_left + m_inc * mouseClickX;
        m_juliaCenterY = m_top - m_inc * mouseClickY;

        // Update the mode and initialize the julia mode parameters, including coordinate arrays
        // Center of the viewport (m_centerX,m_centerY) is set to (0.0,0.0)
        m_mode = mode;
        selectFractalFormula(m_id);
    }
    else
    {
        // switching from julia flavor mode back to main fractal
        // restore the main fractal state from the backup
        m_centerX = m_mapBackup.centerX;
        m_centerY = m_mapBackup.centerY;
        m_inc = m_mapBackup.inc;
        m_left = m_mapBackup.left;
        m_top = m_mapBackup.top;
        m_maxIts = m_mapBackup.maxIts;
        m_radius = m_mapBackup.radius;

        // set the julia set center back to zeros
        m_juliaCenterX = 0.0;
        m_juliaCenterY = 0.0;

        // update the mode and reset the points
        m_mode = mode;
        generatePoints();
    }
    // update the mode
}

FractalGenerator::FractalState* FractalGenerator::getState()
{
    m_fractalState =
    {
        m_centerX,
        m_centerY,
        m_radius,
        m_limit,
        m_juliaCenterX,
        m_juliaCenterY,
        m_inc,
        m_left,
        m_top,
        m_mode
    };
    return &m_fractalState;
}
// ***************************************************************
// Private function definitions
// ***************************************************************

void FractalGenerator::initializeVectors()
{
    // initialize the iterations and pointer vectors to the default image size
    m_iterations.assign(m_vector_length, 0);    // holds calculated number of iterations for each pixel
    m_re.reserve(m_vector_length);              // real axis values of points on fractal plane
    m_im.reserve(m_vector_length);              // imaginary axis values of points on fractal plane
    generatePoints();
}

void FractalGenerator::generatePoints()
{
    m_inc = 2.0 * m_radius / static_cast<double>(m_height);
    m_left = m_centerX - m_inc * static_cast<double>(m_width) / 2.0;
    m_top = m_centerY + m_inc * static_cast<double>(m_height) / 2.0;
    for (int row{ 0 }; row < m_height; ++row)
    {
        double y = m_top - m_inc * row;
        for (int col{ 0 }; col < m_width; ++col)
        {
            double x = m_left + m_inc * col;
            int pos = m_width * row + col;
            m_re.data()[pos] = x;
            m_im.data()[pos] = y;
        }
    }
}

// Template that converts json object to fracparams object.
// You don't have to worry about the ordering of the params in the json file.
// see https://json.nlohmann.me/api/adl_serializer/from_json/#examples
void from_json(const nJson& j, FractalGenerator::fracparams& p)
{
    j.at("id").get_to(p.id);
    j.at("name").get_to(p.name);
    j.at("centerX").get_to(p.centerX);
    j.at("radius").get_to(p.radius);
    j.at("limit").get_to(p.limit);
    j.at("kernel").get_to(p.kernel);
}

void FractalGenerator::fillFracParams(nJson fractalsDocument)
{
    for (nJson p : fractalsDocument)
    {
        auto fp = p.template get<FractalGenerator::fracparams>();
        m_fractalSettings.push_back(fp);
    }
}

FractalGenerator::fracparams FractalGenerator::getFracParams(int id)
{
    const fracparams notfound{ 0, "Mandelbrot", -0.7, 0.0, 2.0, 2.0, "Mandelbrot.cu" };
    for (fracparams fp : m_fractalSettings)
    {
        if (fp.id == id) return fp;
    }
    return notfound;
}
