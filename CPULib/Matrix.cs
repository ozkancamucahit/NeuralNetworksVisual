using System;
using System.Collections.Generic;

namespace CPULib
{
    public delegate float ActivationFunction(float x);

    /// <summary>
    /// Matrix with float data
    /// </summary>
    public class Matrix
    {
        
        public Matrix(int rows, int cols)
        {
            if( rows <=0 || cols <=0)
                {
                    Data = null;
                    Rows = 0;
                    Cols = 0;

                    throw new InvalidOperationException("Rows and columns must be >0");
                }

            Data = new float[rows * cols];
            Rows = rows;
            Cols = cols;
        }

        
        public float[] Data { get; private set; } = null;
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        

        /// <summary>
        /// Randomize each element based on range.
        /// </summary>
        /// <param name="min">Inclusive min range.</param>
        /// <param name="max">Exclusive max range.</param>
        public void Randomize(float min, float max)
        {
            Random random = new Random();
            float range = max - min;
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    Data[ c + r * Cols] = ((float)random.NextDouble() * range + min);
                }
            }
        }

        /// <summary>
        /// Makes the matrix identity matrix.
        /// </summary>
        public void MakeIdentity()
        {
            if( this.Cols != this.Rows)
            {
                this.Data = null;
                throw new InvalidOperationException("Identity matrix must be square");
            }

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    if ( r == c) Data[ c + r * Cols] = 1.0F;
                    else Data[ c + r * Cols] = 0.0F;
                }
            }

        }

        /// <summary>
        /// Element-wise multiplication.(aka Hadamard)
        /// </summary>
        /// <param name="other"></param>
        public void Emul( Matrix other)
        {

            if( (this.Cols != other.Cols) && (this.Rows != other.Rows) )
                {
                    this.Data = null;
                }
            
		    //Matrix mRet = new Matrix( other.Rows, other.Cols);

            //if ( mRet.Data == null) return null;

		    for (int r = 0; r < other.Rows; r++)
			    for (int c = 0; c < other.Cols; c++)
				    this.Data[ c + r * Cols] *=other.Data[ c + r * Cols];
		    
            //return mRet;
        }

        /// <summary>
        /// Multiply each element with x.
        /// </summary>
        /// <param name="x">Scale factor.</param>
        public void Scale(float x)
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    this.Data[ c + r * Cols] *= x;
                }
            }
        }

        //
        public Matrix Transpoze()
        {
            Matrix mret = new Matrix(this.Cols, this.Rows);

            for (int r = 0; r < this.Rows; r++)
            {
                for (int c = 0; c < this.Cols; c++)
                {
                    // mret[c][r] = this[r][c]
                    mret.Data[ r + c * mret.Cols] = Data[ c + r * this.Cols];
                }
            }

            return mret;
        }

        /// <summary>
        /// Takes the transpoze of the matrix.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>new matrix transpozed.</returns>
        public static Matrix Transpoze(Matrix matrix)
        {
            Matrix mRet = new Matrix(matrix.Cols, matrix.Rows);

            for (int r = 0; r < matrix.Rows; r++)
            {
                for (int c = 0; c < matrix.Cols; c++)
                {
                    // mret[c][r] = matrix[r][c]
                    mRet.Data[r + c * mRet.Cols] = matrix.Data[c + r * matrix.Cols];
                }
            }
            return mRet;
        }

        /// <summary>
        /// Matrix multiplication.
        /// </summary>
        /// <param name="matrixL">Left Matrix.</param>
        /// <param name="matrixR">Right Matix.</param>
        /// <returns>Result of Matrix multiplication.</returns>
        /// remarks Columns of the left matrix must be equal to Rows of the right matrix.
        public static Matrix operator * (Matrix matrixL, Matrix matrixR)
        {
            if ( matrixL.Cols != matrixR.Rows) return null;

            Matrix mret = new Matrix(matrixL.Rows, matrixR.Cols);

            for (int r = 0; r < matrixL.Rows; r++)
            {
                for (int c = 0; c < matrixR.Cols; c++)
                {
                    float sum = 0.0F;

                    for (int k = 0; k < matrixL.Cols; k++)
                    {
                        // sum = matrixL[r][k] * matrixR[k][c]
                        sum += matrixL.Data[k + r*matrixL.Cols] * 
                                matrixR.Data[c + k*matrixR.Cols];
                    }
                    mret.Data[c + r * mret.Cols] = sum;
                }
            }
            return mret;
        }

        //
        public static Matrix operator - (Matrix matrixL, Matrix matrixR)
        {
            if((matrixL.Rows != matrixR.Rows) && (matrixL.Cols != matrixR.Cols))
            {
                //throw new InvalidOperationException("Matrix sizes not equal");
                return null;
            }

            Matrix mret = new Matrix(matrixL.Rows, matrixL.Cols);

            for (int r = 0; r < mret.Rows; r++)
            {
                for (int c = 0; c < mret.Cols; c++)
                {
                    float left = matrixL.Data[ r + c * matrixL.Cols];
                    float right = matrixR.Data[ r + c * matrixR.Cols];
                    mret.Data[ r + c * mret.Cols] = left - right;
                }
            }
            return mret;
        }

        /// <summary>
        /// Apply function f on every element.
        /// </summary>
        /// <param name="f">Activation Function</param>
        public void Map( ActivationFunction f)
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    var value = this.Data[ c + r * Cols];
                    var output = f(value);
                    this.Data[ c + r * Cols] = output;
                }
            }
        }

        /// <summary>
        /// Apply activation function on each element of the Matrix.
        /// </summary>
        /// <param name="f">Activation function.</param>
        /// <returns>Resultant matrix.</returns>
        public static Matrix Map( Matrix matrix, ActivationFunction f)
        {
            var mRet = new Matrix(matrix.Rows, matrix.Cols);

            for (int r = 0; r < matrix.Rows; r++)
            {
                for (int c = 0; c < matrix.Cols; c++)
                {
                    var value = matrix.Data[ c + r * matrix.Cols];
                    var output = f(value);
                    mRet.Data[ c + r * mRet.Cols] = output;
                }
            }
            return mRet;
        }

        /// <summary>
        /// Random matrix based on range.
        /// </summary>
        /// <param name="rows">rows</param>
        /// <param name="cols">cols</param>
        /// <param name="min">Inclusive min range.</param>
        /// <param name="max">Exclusive max range.</param>
        /// <returns></returns>
        public static Matrix GenerateRandomMatrix(int rows, int cols, float min, float max)
        {
            Matrix mret = new Matrix(rows, cols);
            mret.Randomize(min, max);
            return mret;
        }

        /// <summary>
        /// Element-wise sum.
        /// </summary>
        /// <param name="other"></param>
        public void Add(Matrix other)
        {
            // matrices must be same size
            //this[r][c] += other[r][c]

            for (int r = 0; r < other.Rows; r++)
                for (int c = 0; c < other.Cols; c++)
                    this.Data[c + r * Cols] += other.Data[c + r * Cols];
        }

        /// <summary>
        /// Adds to each element.
        /// </summary>
        /// <param name="num">Number to add elements.</param>
        public void Add(float num)
        {
            for (int r = 0; r < this.Rows; r++)
                for (int c = 0; c < this.Cols; c++)
                    this.Data[c + r * Cols] += num;
        }
        
        /// <summary>
        /// Element-wise subtraction.
        /// </summary>
        /// <returns>left - right Matrix.</returns>
        /// <seealso cref="operator -(Matrix, Matrix)"/>
        public static Matrix Subtract(Matrix left, Matrix right)
        {
            Matrix mRet = new Matrix(left.Rows, left.Cols);

            //TODO - check if they are the same size

            for (int r = 0; r < left.Rows; r++)
            {
                for (int c = 0; c < left.Cols; c++)
                {
                    // mret[r][c] = left[r][c] - right[r][c]
                    mRet.Data[c + r * mRet.Cols] = left.Data[c + r * left.Cols]
                     - right.Data[c + r * right.Cols];
                }
            }
            return mRet;
        }

        /// <summary>
        /// Converts array to column matrix.
        /// </summary>
        /// <param name="arr">input array</param>
        /// <returns>arr.length rows and 1 column matrix</returns>
        public static Matrix FromArrayToMatrix(float[] arr)
        {
            var mret = new Matrix(arr.Length, 1);

            for (int i = 0; i < arr.Length; i++)
            {
                mret.Data[0 + i * mret.Cols] = arr[i];
            }
            return mret;
        }

        /// <summary>
        /// Convert Matrix to array.
        /// </summary>
        /// <returns></returns>
        public float[] ToArray()
        {
            List<float> list = new List<float>(this.Rows * this.Cols);
            //float[] arrayRet = new float[this.Rows * this.Cols];

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    list.Add(this.Data[c + r * Cols]);
                    //arrayRet[c + r*this.Cols] = this.Data[c + r * Cols];
                }
            }
            return list.ToArray();
        }



    }
}
