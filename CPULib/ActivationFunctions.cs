using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPULib
{
    public static class ActivationFunctions
    {
        /// <summary>
        /// Sigmoid or aka Logistic Function
        /// </summary>
        public static float SigmoidFunction(float x)
        {
            return 1.0F / (1.0F + MathF.Exp(-x));
        }

        //
        public static float SigmoidFunctionDerivative(float x)
        {
            return SigmoidFunction(x) * (1.0F - SigmoidFunction(x));
        }

        public static float SignFunction(float n)
        {
            return (n >= 0.0F) ? 1 : -1;
        }

        //
        public static float TanHFunction(float x) => MathF.Tanh(x);

        //
        public static float TanHFunctionDerivative(float x)
        {
	        return 1.0F - (TanHFunction(x) * TanHFunction(x));
        }
        
        //
        public static float ReluFunction(float x)
        {
	        if (x < 0.0F) return 0.0F;
	        return x;
        }

        public static float ReluFunctionDerivative(float x)
        {
	        if (x < 0.0F) return 0.0F;
	        return 1.0F;
        }


    }
}
