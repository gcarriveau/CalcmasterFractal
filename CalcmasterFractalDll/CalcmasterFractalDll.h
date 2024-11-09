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
	// 9: Error in move(). Invalid direction parameter value.
	int getLastErrorCode();

	// Test function - Adds two integers and returns the result.
	int add(int x, int y);

	// Direction parameter for the Move(int direction) method
	enum Direction { UP, DOWN, LEFT, RIGHT };

	void selectFractalFormula(int fractalFormulaID);

	// Resets the dimensions, in pixels, of the fractal image and resizes the m_iterations, m_re, and m_im vectors.
	// The default dimensions are height = 1080, width = 1920 upon instantiating the FractalGenerator class.
	void setDimensions(int height, int width);

	// Returns the element if m_iterations vector<int> specified by the given index.
	// Note: For the index parameter, the size_t (unsigned long long) data type in C# is UInt64.
	int getIterationsAtIndex(size_t index);

	// Returns the address of the iterations int array data, m_iterations.data()
	int* getIterations();

	// Updates m_re and m_im based on current fractal viewbox center and radius, and image height and width
	void generatePoints();

	// returns value of real coordinate at position x of m_re
	double getRealAt(size_t x);

	// returns the address of the fractal plane's real coordinates double array data, m_re.data()
	double* getReals();

	// returns value of imaginary coordinate at position x of m_im
	double getImaginaryAt(size_t y);

	// returns the address of the fractal plane's imaginary coordinates double array data, m_im.data()
	double* getImaginaries();

	// Calculates m_iterations vector values for the a selected main fractal type
	int calculateMap();

	void setJuliaCenter(double jcX, double jcY);

	// Updates the center to the point that was clicked on (converted to fractal plane)
	// Divides m_radius by 1.5
	// Recalculates m_inc, m_top, m_left
	// Recalculates the fractal plane points
	int zoomInAtPoint(int col, int row);

	int zoomOut();

	int move(int direction);

	// Switch between the "Map" of all julia sets and a Julia mode
	// mode 0 Map, 1 Julia, 2 TheCalcmasterTwist, 3 AirOnAJuliaString (reserved)
	void setMode(int mode, int mouseClickX, int mouseClickY);

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

	// used to save/retrieve the main (map) fractal params when switching
	// back and forth between mode 0 and a julia set mode.
	struct mapBackup
	{
		int maxIts;
		double centerX;
		double centerY;
		double radius;
		double inc;
		double top;
		double left;
	};

	struct FractalState
	{
		double centerX;
		double centerY;
		double radius;
		double limit;
		double juliaCenterX;
		double juliaCenterY;
		double inc;
		double left;
		double top;
		int mode;
	};

	FractalState* getState();

private:
	// ***************************************************************
	// Private variables
	// ***************************************************************

	// m_backup: stores a backup of the main fractal's parameters
	// when switching from m_mode == 0 to m_mode != 0
	mapBackup m_mapBackup{}; // initialize with defaults

	// m_maxIts
	int m_maxIts{ 1000 };

	// m_mode 0 Map, 1 Julia, 2 TheCalcmasterTwist, 3 AirOnAJuliaString (reserved)
	int m_mode{ 0 };

	// m_lastError
	int m_lastError{ 0 };

	// m_height:		fractal image height in pixels
	int m_height{ 1080 };

	// m_width:			fractal image width in pixels
	int m_width{ 1920 };

	FractalState m_fractalState{};

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

extern "C" _declspec(dllexport) void SetJuliaCenter(FractalGenerator* t, double jcX, double jcY)
{
	t->setJuliaCenter(jcX, jcY);
}

extern "C" _declspec(dllexport) void SetMode(FractalGenerator* t, int mode, int mouseClickX = 0, int mouseClickY = 0)
{
	t->setMode(mode, mouseClickX, mouseClickY);
}

extern "C" _declspec(dllexport) FractalGenerator::FractalState* GetState(FractalGenerator* t)
{
	FractalGenerator::FractalState* ptr = t->getState();
	return ptr;
}

extern "C" _declspec(dllexport) int GetIterationsAt(FractalGenerator* t, size_t index)
{
	return t->getIterationsAtIndex(index);
}

extern "C" _declspec(dllexport) int* GetIterations(FractalGenerator* t)
{
	return t->getIterations();
}

extern "C" _declspec(dllexport) int CalculateMap(FractalGenerator* t)
{
	return t->calculateMap();
}

extern "C" _declspec(dllexport) int ZoomInAtPoint(FractalGenerator* t, int col, int row)
{
	return t->zoomInAtPoint(col, row);
}

extern "C" _declspec(dllexport) int ZoomOut(FractalGenerator* t)
{
	return t->zoomOut();
}

extern "C" _declspec(dllexport) int Move(FractalGenerator* t, int direction)
{
	return t->move(direction);
}

extern "C" _declspec(dllexport) double GetRealAt(FractalGenerator* t, size_t x)
{
	return t->getRealAt(x);
}

extern "C" _declspec(dllexport) double* GetReals(FractalGenerator* t)
{
	return t->getReals();
}

extern "C" _declspec(dllexport) double GetImaginaryAt(FractalGenerator* t, size_t y)
{
	return t->getImaginaryAt(y);
}

extern "C" _declspec(dllexport) double* GetImaginaries(FractalGenerator* t)
{
	return t->getImaginaries();
}
