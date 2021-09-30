#pragma once

#ifndef MATRIX_H
#define MATRIX_H

#include <random>
#include <chrono>
#include <cmath>
#include <vector>
#include <string.h>

#define min(a,b) ((a) < (b) ? (a) : (b))

#define max(a,b) ((a) > (b) ? (a) : (b))



template <class Type>
class Matrix
{

private:
	// data stored in matrix
	Type ** m_data;
	int m_rows, m_cols;

	// initialize data

public:

	///////////////////////////////////////////////////////////////////////
	//
	//   initialize( int rows, int cols, Type **newData = NULL )
	//   Sets size of matrix to rows x cols and copies data newData
	//   into matrix.
	//
	///////////////////////////////////////////////////////////////////////

	void initialize(int rows, int cols, Type **newdata = NULL)
	{
		empty();

		// parameter checking
		if (rows <= 0 || cols <= 0)
			std::cerr << "Size can't be less than or equal to zero" << std::endl;

		// Allocate new array to store row pointers
		m_data = new Type*[rows];
		if (m_data == NULL)
			std::cerr << "Row allocation failed" << std::endl;

		m_rows = rows;
		m_cols = cols;

		// Allocate each row
		for (int i = 0; i < m_rows; i++)
		{
			m_data[i] = new Type[m_cols];
			if (m_data[i] == NULL)
				std::cerr << "Matrix column allocation failed" << std::endl;
		}

		// Fill array if data given
		if (newdata != NULL)
		{
			for (int i = 0; i < m_rows; i++)
				memcpy(m_data[i], newdata[i], cols * sizeof(Type));
		}

	}




	virtual void print()
	{
		for (int i = 0; i < m_rows; ++i)
		{
			for (int j = 0; j < m_cols; ++j)
			{
				if (fabs(this->m_data[i][j]) < .0000003F)
					this->m_data[i][j] = 0;
				std::cout << this->m_data[i][j] << " ";
			}
			std::cout << '\n';
		}
		std::cout << "\n\n";
	}

	Type ** getData() const { return m_data; }


	 int rows()  { return m_rows; }
	 int cols()  { return m_cols; }

	Matrix(int r_, int c_) : m_rows(r_), m_cols(c_), m_data(NULL)
	{
		initialize(m_rows, m_cols);
	}

	Matrix() : m_data(NULL),
		m_rows(2),
		m_cols(2) {

		initialize(m_rows, m_cols, m_data);

		for (int i = 0; i < m_rows; i++)
		{
			for (int j = 0; j < m_cols; j++)
				this->m_data[i][j] = 0;
		}
	}

	Matrix(int r_, int c_, char p_) : m_rows(r_), m_cols(c_), m_data(NULL)
	{
		if (p_ == 'e' || p_ == 'E')
		{
			if (r_ != c_)
			{
				std::cerr << "Identity matrix must be a square" << std::endl;
				return;
			}
			else {
				initialize(m_rows, m_cols, m_data);
				for (size_t i = 0; i < r_; i++)
				{
					for (size_t j = 0; j < c_; j++)
					{
						if (i == j)
							m_data[i][j] = 1;
						else
							m_data[i][j] = 0;
					}
				}
			}
		}

		else if (p_ == 'r' || p_ == 'R')
		{
			// random seed
			srand(time(NULL));

			initialize(m_rows, m_cols, m_data);
			int data;
			for (size_t i = 0; i < r_; i++)
				for (size_t j = 0; j < c_; j++)
				{
					data = rand() % 256;
					m_data[i][j] = data;
				}

		}
		else {
			std::cerr << "unexpected parameter" << std::endl;
			return;
		}
	}

	Matrix(int r_, int c_, int d_) : m_rows(r_), m_cols(c_), m_data(NULL)
	{
		initialize(m_rows, m_cols, m_data);

		for (int i = 0; i < m_rows; i++)
		{
			for (int j = 0; j < m_cols; j++)
				m_data[i][j] = d_;
		}
	}

	Matrix(const Matrix<Type>& m) :
		m_data(NULL),
		m_rows(0),
		m_cols(0)
	{
		// Construction from existing matrix
		initialize(m.m_rows, m.m_cols, m.m_data);
	}

	bool isEmpty() const { return (m_data == NULL); }

	Matrix <Type>& reSize(int r_, int c_)
	{
		initialize(r_, c_);
		return *this;
	}


	Type* operator[](int row)
	{
		if (isEmpty())
			std::cerr << "Matrix has no data\n\n";

		if (row < 0 || row >= m_rows)
			std::cerr << "out of boundary" << std::endl;

		return m_data[row];
	}

	const Type* operator[](int row) const
	{
		if (isEmpty())
			std::cerr << "Matrix has no data\n\n";

		if (row < 0 || row >= m_rows)
			std::cerr << "out of boundary\n\n";

		return m_data[row];
	}

	Matrix<Type> Transpoze()
	{
		Matrix <Type> mRet(this->cols(), this->rows());

		for (int i = 0; i < this->rows(); i++)
			for (int j = 0; j < this->cols(); j++)
				mRet.m_data[j][i] = this->m_data[i][j];
		return mRet;
	}

	Matrix<Type>& operator=(const Matrix<Type>& m)
	{
		// Equal to itself
		if (&m != this)
			initialize(m.m_rows, m.m_cols, m.m_data);

		return *this;
	}

	Matrix<Type> operator+(const Matrix<Type>& m)
	{
		int rows = min(m.rows(), this->rows());
		int cols = min(m.cols(), this->cols());
		if (rows == 0 || cols == 0)
			std::cerr << "Matrix has no data" << std::endl;

		Matrix<Type> mRet(rows, cols);
		for (int i = 0; i < rows; i++)
			for (int j = 0; j < cols; j++)
				mRet.m_data[i][j] = m.m_data[i][j] + this->m_data[i][j];
		return mRet;
	}

	Matrix<Type> operator-(const Matrix<Type>& m)
	{
		int rows = min(m.rows(), this->rows());
		int cols = min(m.cols(), this->cols());
		if (rows == 0 || cols == 0)
			std::cerr << "Matrix has no data" << std::endl;
		Matrix<Type> mRet(rows, cols);
		for (int i = 0; i < rows; i++)
			for (int j = 0; j < cols; j++)
				mRet.m_data[i][j] = this->m_data[i][j] - m.m_data[i][j];

		return mRet;
	}

	Matrix<Type>& operator*(Type scale)
	{
		for (int i = 0; i < rows(); i++)
			for (int j = 0; j < cols(); j++)
				this->m_data[i][j] *= scale;
		return *this;
	}

	Matrix<Type>& operator+(int scale)
	{
		for (int i = 0; i < rows; i++)
			for (int j = 0; j < cols; j++)
				this->m_data[i][j] += scale;
		return *this;
	}

	Matrix<Type>& operator-(int scale)
	{
		for (int i = 0; i < rows; i++)
			for (int j = 0; j < cols; j++)
				this->m_data[i][j] -= scale;
		return *this;
	}

	Matrix<Type>& operator/(int scale)
	{
		for (int i = 0; i < rows; i++)
			for (int j = 0; j < cols; j++)
				this->m_data[i][j] /= scale;
		return *this;
	}

	Matrix<Type>& operator%(int scale)
	{
		for (int i = 0; i < rows; i++)
			for (int j = 0; j < cols; j++)
				this->m_data[i][j] %= scale;
		return *this;
	}

	Matrix<Type>& operator^(int pow_)
	{
		for (int i = 0; i < rows; i++)
			for (int j = 0; j < cols; j++)
				this->m_data[i][j] = pow(this->m_data[i][j], pow_);
		return *this;
	}

	Matrix<Type> operator*( Matrix<Type>& m)
	{
		int rows = this->rows();
		int cols = m.cols();
		if (rows == 0 || cols == 0)
			std::cerr << "Matrix has no data" << std::endl;
		if (this->cols() != m.rows())
			std::cerr << "Row size does not match column size" << std::endl;

		Matrix<Type> mRet( rows, cols );
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				Type tsum = 0;
				for (int k = 0; k < this->cols(); k++)
					tsum += this->m_data[i][k] * m.m_data[k][j];
				mRet.m_data[i][j] = tsum;
			}
		}
		return mRet;
	}

	 void Randomize()
	{
		std::mt19937_64 rng;
		// initialize the random number generator with time-dependent seed
		uint64_t timeSeed = std::chrono::high_resolution_clock::now().time_since_epoch().count();
		std::seed_seq ss{ uint32_t(timeSeed & 0xffffffff), uint32_t(timeSeed >> 32) };
		rng.seed(ss);
		// initialize a uniform distribution between 0 and 0.2
		std::uniform_real_distribution<double> unif(0,0.2);
		// ready to generate random numbers
		

		for (size_t i = 0; i < rows() ; ++i)
			for (size_t j = 0; j < cols(); ++j)
				this.m_data[i][j] = unif(rng);
			


	}

	static int Randomize04()
	{
		std::mt19937_64 rng;
		// initialize the random number generator with time-dependent seed
		uint64_t timeSeed = std::chrono::high_resolution_clock::now().time_since_epoch().count();
		std::seed_seq ss{ uint32_t(timeSeed & 0xffffffff), uint32_t(timeSeed >> 32) };
		rng.seed(ss);
		// initialize a uniform distribution between 0 and 4
		std::uniform_real_distribution<double> unif(0, 4);
		// ready to generate random numbers


		return unif(rng);

	}

	static Matrix<Type> fromVector(std::vector<Type> arr )
	{

		Matrix<Type> m(arr.size(), 1); //m; //= new Matrix<Type>(arr->size(), 1);

		for (size_t i = 0; i < arr.size(); ++i)
		{
			m.m_data[i][0] = arr.at(i);
		}
		return m;

	}

	std::vector<Type> toVector()
	{
		std::vector<Type> arr;
		for (size_t i = 0; i < this->rows(); ++i) {
			for (size_t j = 0; j < this->cols(); ++j) {
				//arr.at( i* this->cols() + j ) = this->m_data[i][j];
				arr.push_back( this->m_data[i][j] );
			}
		}
		return arr;

	}

	static Matrix<Type> subtract(Matrix<Type>& a, Matrix<Type>& b) {
		// Return a new Matrix a-b
		Matrix<Type> result(a.rows(), a.cols()); // = new Matrix(a.rows(), a.cols());
		for (size_t i = 0; i < result.rows(); ++i) {
			for (size_t j = 0; j < result.cols(); ++j) {
				result.m_data[i][j] = a.m_data[i][j] - b.m_data[i][j];
			}
		}
		return result;
	}

	 void map( float (*func)(float) )
	{
		for (size_t i = 0; i < rows() ; ++i)
		{
			for (size_t j = 0; j < cols() ; ++j)
			{
				float val = this->m_data[i][j];
				this->m_data[i][j] = func(val);
			}
		}

	}


	  static Matrix<Type> map(Matrix<Type>& M, float(*func)(float))
	{
		Matrix<Type>  result (M.rows(), M.cols()); // = new Matrix<Type>(M.rows(), M.cols());

		for (size_t i = 0; i < M.rows(); ++i)
			for (size_t j = 0; j < M.cols(); ++j)
			{
				Type val = M.m_data[i][j];
				result.m_data[i][j] = func(val);
			}

		return result;
	}

	void add (Matrix<Type>& m)
	{
		if ((m.rows() != this->rows()) && (m.cols() != this->rows()))
		{
			std::cout<<"matrix error";
			exit(-17);
		}
		else{
			for (size_t i = 0; i < this->rows(); ++i) 
				for (size_t j = 0; j < this->cols(); ++j)
				{
					//std::cout << this->m_data[i][j] << " + " << m.m_data[i][j];
					this->m_data[i][j] += m.m_data[i][j];
				}

		}
	}

	void add( Type m)
	{
		for (size_t i = 0; i < this->rows(); ++i)
			for (size_t j = 0; j < this->cols(); ++j)
				this->m_data[i][j] += m;

	}


	void swapRows(int r1, int r2)
	{
		if (isEmpty())
			std::cerr << " Empty matrix" << std::endl;

		if (r1 < 0 || r1 >= m_rows || r2 < 0 || r2 >= m_rows)
			std::cerr << " Boundary exception" << std::endl;

		Type *pt = m_data[r1];
		m_data[r1] = m_data[r2];
		m_data[r2] = pt;
	}

	///////////////////////////////////////////////////////////////////////////
	//
	// Matrix<Type> invert( void )
	//   Invert a square matrix.
	//  Returns:
	//   Resultant matrix ( mI )
	//
	///////////////////////////////////////////////////////////////////////////

	Matrix<double> invert()
	{
		if (this->isEmpty())
			std::cerr << " Empty matrix" << std::endl;

		if (this->cols() != this->rows())
			std::cerr << " inverse of non-square matrix" << std::endl;

		int n = this->cols();
		Matrix<double> mI(n, n);
		conv(mI, *this);

		// Allocate index arrays and set to zero
		int *pivotFlag = new int[n];
		int *swapRow = new int[n];
		int *swapCol = new int[n];
		if (pivotFlag == NULL || swapRow == NULL || swapCol == NULL)
			std::cerr << " Couldnt allocate memory" << std::endl;

		for (int i = 0; i < n; i++)
		{
			pivotFlag[i] = 0;
			swapRow[i] = 0;
			swapCol[i] = 0;
		}

		// Pivoting n iterations
		int row, irow, col, icol;
		for (int i = 0; i < n; i++)
		{
			// Find the biggest pivot element
			double big = 0.0;
			for (row = 0; row < n; row++)
			{
				if (pivotFlag[row] == 0)
				{
					// Only unused pivots
					for (col = 0; col < n; col++)
					{
						if (pivotFlag[col] == 0)
						{
							double abs_element = fabs(mI.m_data[row][col]);
							if (abs_element >= big)
							{
								big = abs_element;
								irow = row;
								icol = col;
							}
						}
					}
				}
			}

			// Mark this pivot as used
			pivotFlag[icol]++;

			// Swap rows to make this diagonal the biggest absolute pivot
			if (irow != icol)
			{
				for (col = 0; col < n; col++)
				{
					double temp = mI.m_data[irow][col];
					mI.m_data[irow][col] = mI.m_data[icol][col];
					mI.m_data[icol][col] = temp;
				}
			}

			// Store what we swaped
			swapRow[i] = irow;
			swapCol[i] = icol;

			// Bad news if the pivot is zero
			if (mI.m_data[icol][icol] == 0.0)
				std::cerr << "Invert of singular matrix" << std::endl;

			// Divide the row by the pivot
			double pivotInverse = 1.0 / mI.m_data[icol][icol];

			// Pivot = 1 to avoid round off
			mI.m_data[icol][icol] = 1.0;
			for (col = 0; col < n; col++)
				mI.m_data[icol][col] *= pivotInverse;

			// Fix the other rows by subtracting
			for (row = 0; row < n; row++)
			{
				if (row != icol)
				{
					double temp = mI.m_data[row][icol];
					mI.m_data[row][icol] = 0.0;
					for (col = 0; col < n; col++)
						mI.m_data[row][col] -= mI.m_data[icol][col] * temp;
				}
			}
		}

		// Fix the effect of all the swaps for final answer
		for (int swap = n - 1; swap >= 0; swap--)
		{
			if (swapRow[swap] != swapCol[swap])
			{
				for (row = 0; row < n; row++)
				{
					double temp = mI.m_data[row][swapRow[swap]];
					mI.m_data[row][swapRow[swap]] = mI.m_data[row][swapCol[swap]];
					mI.m_data[row][swapCol[swap]] = temp;
				}
			}
		}

		// Free memory
		delete[] pivotFlag;
		delete[] swapRow;
		delete[] swapCol;

		// Convert back to Type
		return mI;


	}

	///////////////////////////////////////////////////////////////////////////
	//
	// double det( void )
	//   Calculate determinant of a matrix.
	//  Returns:
	//   Determinant as double.
	//
	///////////////////////////////////////////////////////////////////////////

	double det()
	{
		if (this->isEmpty())
			std::cerr << "Matrix has no data" << std::endl;
		if (this->cols() != this->rows())
			std::cerr << "Det of non square matrix" << std::endl;
		// Allocate space for the determinant calculation matrix
		Matrix<double> mDet(this->cols(), this->cols());
		conv(mDet, *this);

		int n = this->cols();

		// Initialize the answer
		double det = 1.0;
		for (int pivot = 0; pivot < n - 1; pivot++)
		{
			// Find the biggest absolute pivot
			double big = fabs(mDet[pivot][pivot]);

			// Initialize for no swap
			int swapRow = 0;
			for (int row = pivot + 1; row < n; row++)
			{
				double abs_element = fabs(mDet[row][pivot]);
				if (abs_element > big)
				{
					swapRow = row;
					big = abs_element;
				}
			}

			// Unless swapRow is still zero we must swap two rows
			if (swapRow != 0)
			{
				// Swap two rows
				mDet.swapRows(pivot, swapRow);

				// Change the sign of determinant because of swap
				det *= -mDet[pivot][pivot];
			}
			else
			{
				// Calculate the determinant by the product of the pivots
				det *= mDet[pivot][pivot];
			}

			// If almost singular matrix, give up now
			if (fabs(det) < .0000003F)
				return det;

			double pivotInverse = 1.0 / mDet[pivot][pivot];
			for (int col = pivot + 1; col < n; col++)
				mDet[pivot][col] = mDet[pivot][col] * pivotInverse;

			for (int row = pivot + 1; row < n; row++)
			{
				double temp = mDet[row][pivot];
				for (int col = pivot + 1; col < n; col++)
				{
					mDet[row][col] = mDet[row][col] - mDet[pivot][col] * temp;
				}
			}
		}

		// Last pivot, no reduction required
		det *= mDet[n - 1][n - 1];
		return det;
	}

	///////////////////////////////////////////////////////////////////////////
	//
	// Matrix<Type> emul( const Matrix<Type>& m1, const Matrix<Type>& m )
	//   Multiply elements of two matrices.
	//  Returns:
	//   Resultant matrix ( m1 * m2 )
	//
	///////////////////////////////////////////////////////////////////////////

	Matrix<Type>& emul( Matrix<Type>& m)
	{
		int rows = min(m.rows(), this->rows());
		int cols = min(m.cols(), this->cols());
		if (rows == 0 || cols == 0)
			std::cerr << "Matrix has no data" << std::endl;
		Matrix<Type> mRet(rows, cols);
		for (int i = 0; i < rows; i++)
			for (int j = 0; j < cols; j++)
				this->m_data[i][j] *= m.m_data[i][j];
		return *this;
	}

	// Set all elements of matrix equal to constant
	Matrix<Type>& operator=(const Type t)
	{
		for (int i = 0; i < m_rows; i++)
		{
			Type *pt = m_data[i];
			for (int j = 0; j < m_cols; j++)
				*pt++ = t;
		}
		return *this;
	}


	// Destructor
	~Matrix() { empty(); }

	// clear data
	void empty()
	{
		if (m_data != NULL)
		{
			for (int i = 0; i < m_rows; i++)
				delete[] m_data[i];

			delete[] m_data;
			m_data = NULL;
			m_rows = 0;
			m_cols = 0;
		}
	}


};

///////////////////////////////////////////////////////////////////////////
//
// Matrix<Type1>& conv( Matrix<TypeTo>& mTo, const Matrix<TypeFrom>& mFrom )
//   Convert a matrix from one type to another.
//  Returns:
//   Resultant matrix ( mTo = (mTo's type)mFrom )
//
///////////////////////////////////////////////////////////////////////////

template <class TypeTo, class TypeFrom>
Matrix<TypeTo>& conv(Matrix<TypeTo>& mTo, const Matrix<TypeFrom>& mFrom)
{
	if (mFrom.isEmpty())
		std::cerr << "Empty matrix\n\n";

	int rows = mFrom.rows();
	int cols = mFrom.cols();
	mTo.reSize(rows, cols);

	// Convert data element-by-element
	for (int i = 0; i < rows; i++)
		for (int j = 0; j < cols; j++)
			mTo[i][j] = (TypeTo)mFrom[i][j];
	return mTo;
}


///////////////////////////////////////////////////////////////////////////
//
// Matrix<Type> mult( const Matrix<Type>& m1, const Matrix<Type>& m2 )
//   Cross product of two matrices.
//
//  Returns:
//   Resultant matrix ( m1 cross m2 )
//
///////////////////////////////////////////////////////////////////////////
template <class Type>
Matrix<Type> mult(const Matrix<Type>& m1, const Matrix<Type>& m2)
{
	int rows = m1.rows();
	int cols = m2.cols();
	if (rows == 0 || cols == 0)
		std::cerr << "Empty matrix\n\n";
	if (m1.cols() != m2.rows())
		std::cerr << "Row size does not match column size\n\n";

	Matrix<Type> mRet(rows, cols);
	Matrix<Type> mRet = new Matrix<Type>(rows,cols);
	for (int i = 0; i < rows; i++)
	{
		for (int j = 0; j < cols; j++)
		{
			Type tsum = 0;
			for (int k = 0; k < m1.cols(); k++)
				tsum += m1[i][k] * m2[k][j];
			mRet[i][j] = tsum;
		}
	}
	return mRet;
}

template <class Type>
void print(std::vector<Type> const &input)
{
	for (int i = 0; i < input.size(); i++) {
		std::cout << input.at(i) << std::endl;
	}
}

struct object
{
	std::vector<float> inputs;
	std::vector<float> targets;
};


#endif // !MATRIX_H