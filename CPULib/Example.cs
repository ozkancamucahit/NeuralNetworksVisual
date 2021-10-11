using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPULib
{
    public static class Example
    {
        private static void Add(UInt32 N, float[] x, float[] y)
        {
            for (UInt32 i = 0; i < N; ++i)
                y[i] += x[i];
        }

        /// <summary>
        /// Runs the vector add example on CPU with 1M elements.
        /// </summary>
        public static long RunExample()
        {
            var watch = Stopwatch.StartNew();
            UInt32 N = 1 << 20;

            float[] x = new float[N];
            float[] y = new float[N];

            for (UInt32 i = 0; i < N; ++i)
            {
                x[i] = 8.0F;
                y[i] = 9.0F;
            }

            Add(N, x, y);
            watch.Stop();
            var elapsedMS = watch.ElapsedMilliseconds;
            return elapsedMS;
        }
    }
}
