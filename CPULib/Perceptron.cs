﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPULib
{
    /// <summary>
    /// </summary>
    public class Perceptron
    {
        public float[] Weights { get; private set; }
        public float learningRate { get; private set; }

        public Perceptron(int size, float lr = 0.1F)
        {
            Weights = new float[size];
            learningRate = lr;
            Random random = new Random();

            //Init weights randomly
            for (int i = 0; i < size; i++)
                Weights[i] = random.Next(-1, 1);
            
        }

        public static int Sign( float n)
        {
            return (n >= 0.0F) ? 1 : -1;
        }

        //Feedforward
        public int Guess(float[] inputs)
        {
            float sum = 0.0F;

            for (int i = 0; i < Weights.Length; i++)
                sum += inputs[i] * Weights[i];

            int output = Sign(sum);
            return output;
        }

        public void Train(float [] inputs, int target)
        {
            int guess = Guess(inputs);
            int error = target - guess;

            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] += error * inputs[i]*learningRate;
            }


        }

    }
}