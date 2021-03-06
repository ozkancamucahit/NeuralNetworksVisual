using System;
using System.Collections.Generic;
using System.Text;

namespace CPULib
{
    public enum CLASSID { NULL, CLASS1, CLASS2, CLASS3, CLASS4}

   

    public struct Sample
    {
        public Sample(int x_, int y_, CLASSID classId_)
        {
            x = x_;
            y = y_;
            classID = classId_;
            Bias = 1.0F;
        }

        int x, y;
        CLASSID classID;

        public float Bias { get; set; }

        public string Name {
            get { return $"( {x},  {y})  -{classID}"; }
            
        }

        public int X { 
            get { return x; }

            set { x = value; } 
        }

        public int Y { 
            get { return y; }

            set { y = value; }
        }

        public CLASSID sampleID { 
            get { return classID; }
            set { classID = value; }
        }

        public static bool operator ==(Sample s1, Sample s2)
        {
            return (( s1.X == s2.X) && (s1.Y == s2.Y));
        }

        public static bool operator !=(Sample s1, Sample s2)
        {
            return !((s1.X == s2.X) && (s1.Y == s2.Y));
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        

    }
    
}
