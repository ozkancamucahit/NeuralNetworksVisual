#pragma once
#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include <math.h>
#include <chrono>

#define DLL_EXPORT_API extern "C" __declspec(dllexport)
#define DLL_IMPORT_API extern "C" __declspec(dllimport)


#ifdef DLL_EXPORT 
DLL_EXPORT_API int addWithCuda(int* c, const int* a, const int* b, unsigned int size);
DLL_EXPORT_API __global__ void addKernel(int* c, const int* a, const int* b);
DLL_EXPORT_API  int callCuda();
#else
DLL_IMPORT_API int addWithCuda(int* c, const int* a, const int* b, unsigned int size);
DLL_IMPORT_API __global__ void addKernel(int* c, const int* a, const int* b);
DLL_IMPORT_API  int callCuda();
#endif
