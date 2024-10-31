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

// Alias the JSON serializer/deserializer
using nJson = nlohmann::json; // Thanks, Neil Lohmann (Also MIT) https://github.com/nlohmann/json
using namespace std::string_view_literals;

// CUDA function to calculate main fractal images
int TheCalcmaster(double* host_re, double* host_im, int* host_its, double limit, int fractalID, int maxIts, size_t numElements, int mode = 0, double juliaCenterX = 0.0, double juliaCenterY = 0.0);

// Forward declaration of the class that is exported from the dll
class FractalGenerator {
public:
	// Constructor
	FractalGenerator(void);

	// getLastErrorCode() returns the value of m_lastError to the caller.
	// 0: host execution OK
	// 1: Error in FractalGenerator() constructor locating/loading/opening the fractals.json file.
	// 2: Error in FractalGenerator() constructor parsing the fractals.json file into a valid JSON Document.
	// 3: Error in setDimensions(height, width). Either height or width is a value <= 0.
	// 4: Error in calculateMap() allocating memory to the CUDA compute device.
	// 5: Error in calculateMap() copying host data to device memory.
	// 6: Error in calculateMap() launching the kernel.
	// 7: Error in calculateMap() copying iterations back to m_iterations.
	// 8: Error in calculateMap() freeing device global memory.
	int getLastErrorCode();

	// Test function - Adds two integers and returns the result.
	int add(int x, int y);


	void selectFractalFormula(int fractalFormulaID);

	// Resets the dimensions, in pixels, of the fractal image and resizes the m_iterations, m_re, and m_im vectors.
	// The default dimensions are height = 1080, width = 1920 upon instantiating the FractalGenerator class.
	void setDimensions(int height, int width);

	// Returns the element if m_iterations vector<int> specified by the given index.
	// Note: For the index parameter, the size_t (unsigned long long) data type in C# is UInt64.
	int getIterationsAtIndex(size_t index);

	// Updates m_re and m_im based on current fractal viewbox center and radius, and image height and width
	void generatePoints();

	// returns value of real coordinate at position x of m_re
	double getRealAt(size_t x);
	// returns value of imaginary coordinate at position x of m_im
	double getImaginaryAt(size_t y);

	// Calculates m_iterations vector values for the a selected main fractal type
	int calculateMap();

	// Updates the center to the point that was clicked on (converted to fractal plane)
	// Divides m_radius by 1.5
	// Recalculates m_inc, m_top, m_left
	// Recalculates the fractal plane points
	int zoomInAtPoint(int col, int row);

	// Switch between the "Map" of all julia sets and a Julia mode
	// mode 0 Map, 1 Julia, 2 TheCalcmasterTwist, 3 AirOnAJuliaString (reserved)
	void setMode(int mode, double juliaCenterX = 0.0, double juliaCenterY = 0.0);

	// fractals.json item structure
	struct fracparams
	{
		int id;
		std::string name;
		double centerX;
		double centerY;
		double radius;
		double limit;
		std::string kernel;
	};

private:
	// ***************************************************************
	// Private variables
	// ***************************************************************

	// m_maxIts
	int m_maxIts{ 600 };

	// m_mode 0 Map, 1 Julia, 2 TheCalcmasterTwist, 3 AirOnAJuliaString (reserved)
	int m_mode{ 0 };

	// m_lastError
	int m_lastError{ 0 };

	// m_height:		fractal image height in pixels
	int m_height{ 1080 };

	// m_width:			fractal image width in pixels
	int m_width{ 1920 };

	// Fractal computation variable defaults.
	// These can be changed to a different kind of fractal at runtime using exported SelectFractalFormula(int)
	int			m_id{ 0 };					// unique integer identifier of the fractal
	std::string m_name{ "Mandelbrot" };		// fractal type name
	double		m_centerX{ -0.7 };			// real coordinate of fractal plane center
	double		m_centerY{ 0.0 };			// imaginary coordinate of fractal plane center
	double		m_radius{ 2.0 };			// radius is half of the height of the visible area on the fractal plane
	double		m_limit{ 2.0 };				// iteration escape magnitude
	std::string m_kernel{ "frmMandelbrot" };// the __device__ function that determines the fractal's mathematical form
	double		m_juliaCenterX{ 0.0 };
	double		m_juliaCenterY{ 0.0 };

	// m_inc: space between coordinates on the fractal plane
	double m_inc{ 0.0 };
	double m_top{ 0.0 };
	double m_left{ 0.0 };

	// total elements in the points and iterations vectors
	size_t m_vector_length{ 1080 * 1920 };

	// m_iterations:	container for contiguous rows of iterations per pixel
	std::vector<int> m_iterations{};

	// m_re:			container for contiguous real (column) values of points in the fractal plane
	std::vector<double> m_re{};

	// m_im:			container for coniguous imaginary (row) values of points in the fractal plane
	std::vector<double> m_im{};

	// holds the fractals.json entries as structures
	std::vector<fracparams> m_fractalSettings{};

	// ***************************************************************
	// Private functions
	// ***************************************************************

	// Resizes the m_iterations, m_re, and m_im vectors to (m_height * m_width) elements
	// and sets all elements to 0, or 0,0 according to the element type of the vector.
	// It also calls generatePoints() after initializing the vectors.
	void initializeVectors();

	// fills the m_fractalSettings vector with fractals.json items converted to fracparams structures
	void fillFracParams(nJson fractalsJsonDocument);

	fracparams getFracParams(int id);
};

// *****************************************************************
// DLL Exports (The public interface)
// *****************************************************************

// Returns a pointer to an instantiation of a new FractalGenerator Class
extern "C" _declspec(dllexport) void* InstantiateFractalGenerator()
{
	return (void*) new FractalGenerator();
}

// Destroys the instance of FractalGenerator on the C++ side;
// Returns true if object destruction was performed correctly;
extern "C" _declspec(dllexport) bool DestroyFractalGenerator(FractalGenerator* t)
{
	if (!t) return false;
	delete t;
	return true;
}

extern "C" _declspec(dllexport) int GetLastErrorCode(FractalGenerator* t)
{
	return t->getLastErrorCode();
}

extern "C" _declspec(dllexport) int Add(FractalGenerator* t, int x, int y)
{
	return t->add(x, y);
}

extern "C" _declspec(dllexport) void SelectFractalFormula(FractalGenerator* t, int fractalFormulaID)
{
	t->selectFractalFormula(fractalFormulaID);
}

extern "C" _declspec(dllexport) void SetDimensions(FractalGenerator* t, int height, int width)
{
	t->setDimensions(height, width);
}

extern "C" _declspec(dllexport) int GetIterationsAt(FractalGenerator* t, size_t index)
{
	return t->getIterationsAtIndex(index);
}

extern "C" _declspec(dllexport) int CalculateMap(FractalGenerator* t)
{
	return t->calculateMap();
}

extern "C" _declspec(dllexport) int ZoomInAtPoint(FractalGenerator* t, int col, int row)
{
	return t->zoomInAtPoint(col, row);
}

extern "C" _declspec(dllexport) double GetRealAt(FractalGenerator* t, size_t x)
{
	return t->getRealAt(x);
}

extern "C" _declspec(dllexport) double GetImaginaryAt(FractalGenerator* t, size_t y)
{
	return t->getImaginaryAt(y);
}