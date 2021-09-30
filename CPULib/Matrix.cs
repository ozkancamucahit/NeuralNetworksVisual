using System;

namespace CPULib
{
    public class Matrix
    {

        public Matrix(int rows, int cols)
        {
            Data = new float[rows * cols];
            Rows = rows;
            Cols = cols;
        }

        public Matrix()
        {

        }
        public float[] Data { get; set; }

        public int Rows { get; private set; }
        public int Cols { get; private set; }


    }
}
