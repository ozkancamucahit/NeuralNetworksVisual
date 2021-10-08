using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPULib
{
    public class TrainingSet
    {
        public float[] Inputs { get; private set; }
        public float[] Targets { get; private set; }

        public void SetInputs(float [] input_array)
        {
            Inputs = input_array;
        }

        public void SetTargets(float[] target_array)
        {
            Targets = target_array;
        }

        public TrainingSet( Sample sample)
        {
            Inputs = new float[] { sample.X, sample.Y};
            Targets = new float[] { (float)sample.sampleID };
        }

        public TrainingSet(float[] input_array, float[] target_array)
        {
            SetInputs(input_array);
            SetTargets(target_array);
        }
    }
}
