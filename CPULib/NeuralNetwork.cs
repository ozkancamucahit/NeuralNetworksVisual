using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPULib
{
    public class NeuralNetwork
    {
        public int InputNodes    { get; private set; }
        public int HiddenNodes   { get; private set; }
        public int OutputNodes   { get; private set; }
        public Matrix Weights_ih { get; private set; }
        public Matrix Weights_ho { get; private set; }
        public Matrix Bias_h     { get; private set; }
        public Matrix Bias_o     { get; private set; }

        public ActivationFunction activationFunction { get; private set; }




        public NeuralNetwork(int iNodes, int hNodes, int oNodes, ActivationFunction af)
        {
            InputNodes  = iNodes;
            HiddenNodes = hNodes;
            OutputNodes = oNodes;
            Weights_ih = Matrix.GenerateRandomMatrix(hNodes, iNodes, -1, 1);
            Weights_ho = Matrix.GenerateRandomMatrix(oNodes, hNodes, -1, 1);
            Bias_h     = Matrix.GenerateRandomMatrix(hNodes, 1, -1, 1);
            Bias_o     = Matrix.GenerateRandomMatrix(oNodes, 1, -1, 1);
            activationFunction = af;

        }

        public float[] FeedForward(Matrix inputs, ActivationFunction f)
        {
            var hidden = this.Weights_ih * inputs;
            hidden.Add(this.Bias_h);
            hidden.Map(f);

            var output = this.Weights_ho * hidden;
            output.Add(Bias_o);
            output.Map(f);

            return output.ToArray();
        }


        public void Train(float[] inputs, float[] targetArr)
        {
            //This works for single hidden layer.
            //TODO add multi layer support.

            // Get the array.
            var outputArr = this.FeedForward(Matrix.FromArrayToMatrix(inputs), activationFunction);

            // Convert it to matrix.
            var output = Matrix.FromArrayToMatrix(outputArr);
            var targets = Matrix.FromArrayToMatrix(targetArr);

            // error = targets - outputs.
            var output_errors = Matrix.Subtract(targets, output);

            // Make the Weights from hidden to output transpozed.
            var weights_ho_t = Matrix.Transpoze(this.Weights_ho);

            // Calculate hidden layer errors.
            var hidden_errors = weights_ho_t * output_errors;
        }

    }
}
