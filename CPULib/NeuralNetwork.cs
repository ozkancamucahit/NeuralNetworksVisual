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
        public float LearningRate { get; private set; }
        public Matrix Weights_ih { get; private set; }
        public Matrix Weights_ho { get; private set; }
        public Matrix Bias_h     { get; private set; }
        public Matrix Bias_o     { get; private set; }

        public ActivationFunction activationFunction { get; private set; }
        public ActivationFunction activationFunctionDerivative { get; private set; }




        public NeuralNetwork(int iNodes, int hNodes, int oNodes, 
        ActivationFunction af, ActivationFunction afDerivative=null, float lRate=0.1F)
        {
            InputNodes  = iNodes;
            HiddenNodes = hNodes;
            OutputNodes = oNodes;
            LearningRate = lRate;
            Weights_ih = Matrix.GenerateRandomMatrix(hNodes, iNodes, -1, 1);
            Weights_ho = Matrix.GenerateRandomMatrix(oNodes, hNodes, -1, 1);
            Bias_h     = Matrix.GenerateRandomMatrix(hNodes, 1, -1, 1);
            Bias_o     = Matrix.GenerateRandomMatrix(oNodes, 1, -1, 1);
            activationFunction = af;
            activationFunctionDerivative = afDerivative;

        }

        public float[] FeedForward(float[] input_array)
        {
            var inputs = Matrix.FromArrayToMatrix(input_array);
            var hidden = this.Weights_ih * inputs;
            hidden.Add(this.Bias_h);
            hidden.Map(this.activationFunction);

            var output = this.Weights_ho * hidden;
            output.Add(Bias_o);
            output.Map(this.activationFunction);

            return output.ToArray();
        }


        static int count = 0;
        public void Train(float[] input_array, float[] target_array)
        {
            //This works for single hidden layer.
            //TODO add multi layer support.
            
            var inputs = Matrix.FromArrayToMatrix(input_array);
            var hidden = this.Weights_ih * inputs;
            hidden.Add(this.Bias_h);

            hidden.Map(this.activationFunction);

            var outputs = this.Weights_ho*hidden;
            outputs.Add(this.Bias_o);
            outputs.Map(activationFunction);

            var targets = Matrix.FromArrayToMatrix(target_array);
            
            var output_errors = Matrix.Subtract(targets, outputs);

            var gradients = Matrix.Map(outputs, this.activationFunctionDerivative);
            gradients.Emul(output_errors);
            gradients.Scale(this.LearningRate);

            var hidden_T = Matrix.Transpoze(hidden);
            var weight_ho_deltas = gradients * hidden_T;

            // SUS
            this.Weights_ho.Add(weight_ho_deltas);
            this.Bias_o.Add(gradients);

            var Weights_ho_T = Matrix.Transpoze(this.Weights_ho);
            var hidden_errors = Weights_ho_T * output_errors; 

            var hidden_gradient = Matrix.Map(hidden, activationFunctionDerivative);
            hidden_gradient.Emul(hidden_errors);
            hidden_gradient.Scale(this.LearningRate);

            var inputs_T = Matrix.Transpoze(inputs);
            var Weights_ih_deltas = hidden_gradient * inputs_T;

            if(count % 5_000 == 0)
            {
                Matrix m = Matrix.GenerateRandomMatrix(1, 1, 1, 2);
            }

            this.Weights_ih.Add(Weights_ih_deltas);
            this.Bias_h.Add(hidden_gradient);

            count++;
        }

    }
}
