#include <comutil.h>

// Forward declaration of the class that is exported from the dll
class FractalGenerator {
public:
	// Constructor
	FractalGenerator(void);

	// Test function - Adds two integers and returns the result.
	int add(int x, int y);

	// Sets the dimensions of the fractal image - defaults are height = 1080, width = 1920
	// and resizes the m_iterations vector
	void setDimensions(int height, int width);

	int getIterationsAtIndex(size_t index);
private:

	// m_height:		fractal image height in pixels
	int m_height{ 1080 };

	// m_width:			fractal image width in pixels
	int m_width{ 1920 };

	// m_iterations:	container for contiguous rows of iterations per pixel
	std::vector<int> m_iterations{};

	// m_re:			container for real values of points in the fractal plane
	std::vector<double> m_re{};

	// m_im:			container for imaginary values of points in the fractal plane
	std::vector<double> m_im{};
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
// Returns true if object destruction was performed;
extern "C" _declspec(dllexport) bool DestroyFractalGenerator(FractalGenerator* t)
{
	if (!t) return false;
	delete t;
	return true;
}

extern "C" _declspec(dllexport) int Add(FractalGenerator* t, int x, int y)
{
	return t->add(x, y);
}

extern "C" _declspec(dllexport) void SetDimensions(FractalGenerator* t, int height, int width)
{
	t->setDimensions(height, width);
}

extern "C" _declspec(dllexport) int GetIterationsAt(FractalGenerator* t, size_t index)
{
	return t->getIterationsAtIndex(index);
}

// *****************************************************************
// DLL Exports Array Wrappers
// *****************************************************************

// Wrapper class for accessing std::vector<int> m_iterations elements from C#
/*
class IntIterationsWrapper
{
public:
	IntIterationsWrapper(FractalGenerator* t) : m_fractalGenerator(t) { }
	int operator[](size_t index) { return m_fractalGenerator->getIterationsAtIndex(index); }
	~IntIterationsWrapper() { }
private:
	FractalGenerator* m_fractalGenerator;
};

extern "C" _declspec(dllexport) void* GetIterationsWrapper(FractalGenerator* t)
{
	return (void*) new IntIterationsWrapper(t);
}
*/
