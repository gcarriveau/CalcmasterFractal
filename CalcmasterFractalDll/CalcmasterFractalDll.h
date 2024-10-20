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

	// Gets the m_iterations vector data as an array and returns a pointer to the iterations array
	void getIterations(int** ppInt, int* pCount);

private:

	// m_height:		fractal image height in pixels
	int m_height{ 1080 };

	// m_width:			fractal image width in pixels
	int m_width{ 1920 };

	// m_iterations:	container for contiguous rows of iterations per pixel
	std::vector<int> m_iterations{std::vector<int>(size_t(m_height) * size_t(m_width))};

	// m_re:			container for real values of points in the fractal plane
	std::vector<double> m_re{ std::vector<double>(size_t(m_height) * size_t(m_width)) };

	// m_im:			container for imaginary values of points in the fractal plane
	std::vector<double> m_im{ std::vector<double>(size_t(m_height) * size_t(m_width)) };
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

extern "C" _declspec(dllexport) void GetIterations(FractalGenerator* t, int** ppInt, int* pCount)
{
	t->getIterations(ppInt, pCount);
}
