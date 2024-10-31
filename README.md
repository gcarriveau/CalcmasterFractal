# CalcmasterFractal
2024-10-31


![fractalsnakes](fractal_mouse.jpg)

CalcmasterFractal is a CUDA GPU-accelerated fractal and julia set generator.

The Windows user interface project, located in the the CalcmasterFractal folder, is written in C# using .NET 8 LTS.

The other project in the CalcmasterFractalDll folder is the engine which calculates the iterations array.  It's written in C++20 and requires installation of CUDA Toolkit 12.6.  It compiles to a dll file which is loaded by the C# Windows UI at runtime.

My original project which was used to generate a bitmap file series was written completely in C# and parallel processing was performed using CPU cores.

I've posted several [videos](https://www.youtube.com/@fractalsnakes840) on YouTube that are good examples of what this project will be capable of when I finish adding the full functionality of the original C# project to this one.

