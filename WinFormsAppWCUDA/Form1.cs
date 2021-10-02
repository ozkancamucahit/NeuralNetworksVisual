using CPULib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsAppWCUDA
{
    public partial class Form1 : Form
    {
        private List<Sample> samples = new List<Sample>(); // samples with class ids
        private List<Point> points = new List<Point>(); // points on the picture box
        public bool RandomMode { get; private set; }
        public bool TrainMode { get; private set; }

        private static UInt32 numberOfCorrectGuesses = 0;
        private static UInt32 numberOfWrongGuesses = 0;
        private static UInt32 numberOfIterations = 0;
        public static Perceptron Brain { get; private set; } = new Perceptron(lr:0.5F);

        public Graphics G { get; private set; }

        public float F(float x)
        {
            return  x + 80.0F;// + 0.2F;
        }

        public Form1()
        {
            InitializeComponent();
            //Matrix<float> matrix = new Matrix<float>();
            

        }

        private void comboxNoOfClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string item = (string)comboBox.SelectedItem;
            int maxClassNo = Convert.ToInt32(item);

            comboxInputs.Items.Clear();
            comboxInputs.Enabled = true;
            for (int i = 1; i <= maxClassNo; i++)
                comboxInputs.Items.Add(Convert.ToString(i));
            comboxInputs.SelectedIndex = 0;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(System.Drawing.Pens.Black,
                pbCartesianBox.Width / 2, 0, pbCartesianBox.Width / 2, pbCartesianBox.Height);

            g.DrawLine(System.Drawing.Pens.Black,
                0, pbCartesianBox.Height / 2, pbCartesianBox.Width, pbCartesianBox.Height / 2);


        }



        private const int LINE_LENGT = 10;
        private const float THICKNESS = 3.0F;

        private void DrawPlus(Pen pen, int posX, int posY)
        {
            //Pen pen = new Pen(Color.Black, thickness);
            pbCartesianBox.CreateGraphics().DrawLine(pen, posX - LINE_LENGT, posY, posX + LINE_LENGT, posY); // draw horizontal line
            pbCartesianBox.CreateGraphics().DrawLine(pen, posX, posY - LINE_LENGT, posX, posY + LINE_LENGT); // draw vertical line
        }

        private void DrawCrossGuess(Pen pen, int posX, int posY)
        {
            //Pen pen = new Pen(Color.Black, thickness);
            pbCartesianBox.CreateGraphics().DrawLine(pen, posX - LINE_LENGT/2, posY - LINE_LENGT/2, posX + LINE_LENGT/2, posY + LINE_LENGT/2); // draw horizontal line
            pbCartesianBox.CreateGraphics().DrawLine(pen, posX - LINE_LENGT / 2, posY + LINE_LENGT/2, posX + LINE_LENGT / 2, posY - LINE_LENGT/2); // draw vertical line
        }


        private Point SampleToPoint(Sample sample)
        {
            int posX = sample.X + pbCartesianBox.Width / 2;
            int posY = pbCartesianBox.Height / 2 - sample.Y;
            return new Point( posX, posY );
        }

        private void DrawRectangle(Sample sample)
        {
            Pen pen = new Pen(Color.Black, THICKNESS-2.5F);

            Point point = SampleToPoint(sample); // current sample
            Point topLineLeft = new Point(point.X - LINE_LENGT, point.Y - LINE_LENGT);
            Rectangle rectangle = new Rectangle(topLineLeft.X, topLineLeft.Y, (LINE_LENGT+1) * 2, (LINE_LENGT+1) * 2);

            
            pbCartesianBox.CreateGraphics().DrawRectangle(pen, rectangle);

            foreach (Sample s in samples)
            {
                Point other = SampleToPoint(s);

                if (point == other) continue;

                else
                {
                    topLineLeft = new Point(other.X - LINE_LENGT, other.Y - LINE_LENGT);
                    rectangle = new Rectangle(topLineLeft.X, topLineLeft.Y, (LINE_LENGT + 1) * 2, (LINE_LENGT + 1) * 2);
                    pen = new Pen(Color.White, THICKNESS - 2.5F);
                    pbCartesianBox.CreateGraphics().DrawRectangle(pen, rectangle);
                }
            } 

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            if ( TrainMode == true)
            {
                ReDrawSamples();
                Train();
                MakeAGuess();
                DrawGuessedSeperatingLine();
                lblNoOfCorrectGuesses.Text = $"# of correct guesses : {numberOfCorrectGuesses}";
                numberOfCorrectGuesses = 0;
                numberOfWrongGuesses = 0;
                return;
            }

            if (RandomMode == true) return;

            int selectedClass = 0;
            if (comboxInputs.SelectedItem != null)
                selectedClass = Convert.ToInt32(comboxInputs.SelectedItem.ToString());

            Pen pen;
            lboxSamples.Enabled = true;
            lblInputText.Text = $"x: {e.X}, y: {e.Y}";
            lblInputText.Visible = true;

            switch (selectedClass)
            {
                case 1:
                    pen = new Pen(Color.Red, THICKNESS);
                    DrawPlus(pen, e.X, e.Y);
                    
                    AddSampleToList(new Sample(e.X - pbCartesianBox.Width/2, pbCartesianBox.Height/2 - e.Y, CLASSID.CLASS1));
                    break;
                case 2:
                    pen = new Pen(Color.Green, THICKNESS);
                    DrawPlus(pen, e.X, e.Y);
                    
                    AddSampleToList(new Sample(e.X - pbCartesianBox.Width/2, pbCartesianBox.Height/2 - e.Y, CLASSID.CLASS2));

                    break;
                case 3:
                    pen = new Pen(Color.Blue, THICKNESS);
                    DrawPlus(pen, e.X, e.Y);
                    
                    AddSampleToList(new Sample(e.X - pbCartesianBox.Width/2, pbCartesianBox.Height/2 - e.Y, CLASSID.CLASS3));

                    break;

                case 4:
                    pen = new Pen(Color.Brown, THICKNESS);
                    DrawPlus(pen, e.X, e.Y);
                    
                    AddSampleToList(new Sample(e.X - pbCartesianBox.Width/2, pbCartesianBox.Height/2 - e.Y, CLASSID.CLASS4));

                    break;
                default:
                    MessageBox.Show("Invalid class selection");
                    break;
            }


            WireUpList();

        }


        private void AddSampleToList( Sample sample)
        {
            samples.Add(sample);
            points.Add(SampleToPoint(sample));
        }

        private void WireUpList()
        {
            lboxSamples.SelectedIndexChanged -= lboxSamples_SelectedIndexChanged;
            lboxSamples.DataSource = null;
            lboxSamples.DataSource = samples;
            lboxSamples.SelectedIndex = -1;
            lboxSamples.DisplayMember = "Name";
            lboxSamples.SelectedIndexChanged += lboxSamples_SelectedIndexChanged;

            lboxSamples.ClearSelected();
            lboxSamples.Enabled = true;
            btn_reset.Enabled = true;
        }


        [DllImport("CudaLib.dll")]
        static extern int callCuda();
        private void btn_addWCuda_Click(object sender, EventArgs e)
        {
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            int status = callCuda();
            sw1.Stop();
            MessageBox.Show("Time it took :" + System.Convert.ToString(sw1.ElapsedMilliseconds));

        }

        private void ResetList()
        {
            lboxSamples.DataSource = null;
            samples.Clear();
            points.Clear();
            lboxSamples.Enabled = false;
        }
        private void btn_reset_Click(object sender, EventArgs e)
        {
            comboxInputs.SelectedIndex = 0;
            comboxInputs.Enabled = false;
            pbCartesianBox.Refresh();
            btn_reset.Enabled = false;
            btnSet.Enabled = true;
            lblNoOfInputs.Text = "";
            lblNoOfCorrectGuesses.Text = "";
            lblNoOfIterations.Text = "";
            numberOfWrongGuesses = 0;
            numberOfCorrectGuesses = 0;

            ResetList();

            if (comboxNoOfClasses.SelectedItem != null)
            { 
                comboxInputs.Enabled = true;
                //comboxInputs.SelectedIndex = 0;
            }

            RandomMode = false;
            TrainMode = false;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (samples.Count == 0) { 
                MessageBox.Show("Number of Inputs must be greater than Zero");
                btn_reset.Enabled = false;
                btnSet.Enabled = true;
                return;
            }

            btn_reset.Enabled = true;
            btnSet.Enabled = false;
            

            //else samples = new Sample[numberOfInputs];
        }

        private void lboxSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;
            if ((lb.SelectedItem != null))
            { 
                DrawRectangle((Sample)lb.SelectedItem);
                btnRemoveSelected.Enabled = true;
            }
        }

        /// <summary>
        /// visualization for training. Only Two classes for seperating.
        /// Called From train sample
        /// </summary>
        private void RedrawSamplesCircle()
        {

            SolidBrush blackBrush = new SolidBrush(Color.Black);

            foreach (var sample in samples)
            {
                Point pointOnPictureBox = SampleToPoint(sample);

                switch (sample.sampleID)
                {
                    case CLASSID.CLASS3:
                        Pen pen = new Pen(Color.Black, THICKNESS-2);
                        pbCartesianBox.CreateGraphics().DrawEllipse(pen, pointOnPictureBox.X, pointOnPictureBox.Y, LINE_LENGT, LINE_LENGT);
                        break;
                    case CLASSID.CLASS4:
                        pbCartesianBox.CreateGraphics().FillEllipse(blackBrush, pointOnPictureBox.X, pointOnPictureBox.Y, LINE_LENGT, LINE_LENGT);
                        break;
                    default:
                        break;
                }
            }

            DrawSeperatingLine();

        }

        /// <summary>
        /// Called when sample is removed from sample data
        /// </summary>
        private void ReDrawSamples()
        {
            pbCartesianBox.Refresh();
            if (RandomMode || TrainMode) DrawSeperatingLine();

            foreach (var sample in samples)
            {
                Point pointOnPictureBox = SampleToPoint(sample);
                Pen pen;

                switch (sample.sampleID)
                {
                    case CLASSID.CLASS1:
                        pen = new Pen(Color.Red, THICKNESS);
                        DrawPlus(pen, pointOnPictureBox.X, pointOnPictureBox.Y);

                        break;
                    case CLASSID.CLASS2:
                        pen = new Pen(Color.Green, THICKNESS);
                        DrawPlus(pen, pointOnPictureBox.X, pointOnPictureBox.Y);

                        break;
                    case CLASSID.CLASS3:
                        pen = new Pen(Color.Blue, THICKNESS);
                        DrawPlus(pen, pointOnPictureBox.X, pointOnPictureBox.Y);

                        break;
                    case CLASSID.CLASS4:
                        pen = new Pen(Color.Brown, THICKNESS);
                        DrawPlus(pen, pointOnPictureBox.X, pointOnPictureBox.Y);

                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Generates class3 or class4 samples on cartesian space
        /// </summary>
        /// <param name="size">number of random samples</param>
        private void GenerateRandomPointsAndSamples(int size)
        {
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                Point point = new Point(random.Next(pbCartesianBox.Width), random.Next(pbCartesianBox.Height));
                Sample sample = new Sample();
                sample.X = point.X - pbCartesianBox.Width / 2;
                sample.Y = pbCartesianBox.Height / 2 - point.Y;
                sample.Bias = 1.0F;

                // apply function on cartesian space;
                float lineY = F(sample.X);

                if ((sample.Y >= lineY)) sample.sampleID = CLASSID.CLASS3;
                else sample.sampleID = CLASSID.CLASS4;

                AddSampleToList(sample);
            }
        }

        private void DrawRandomSamples()
        {
            GenerateRandomPointsAndSamples(50);
            WireUpList();
            lboxSamples.Enabled = true;
            ReDrawSamples();
        }

        private void Train()
        {

            foreach (var sample in samples)
            {
                float[] inputs = { sample.X,  sample.Y, sample.Bias };//
                int sampleId = (int)sample.sampleID;//

                int target = (sampleId == 3) ? 1 : -1;// map class id to sig function output

                Brain.Train(inputs, target);

#if false
                int guess = Brain.Guess(inputs);

                if (guess == target)
                {
                    Pen pen = new Pen(Color.Green, THICKNESS);
                    Point point = SampleToPoint(sample);
                    DrawCrossGuess(pen, point.X, point.Y);
                    ++numberOfCorrectGuesses;
                }
                else
                {
                    Pen pen = new Pen(Color.Red, THICKNESS);
                    Point point = SampleToPoint(sample);
                    DrawCrossGuess(pen, point.X, point.Y);
                    ++numberOfWrongGuesses;
                } 
#endif

            }
            
        }

        private void MakeAGuess()
        {
            numberOfCorrectGuesses = 0;
            foreach (var sample in samples)
            {
                float[] inputs = {  sample.X, sample.Y, sample.Bias };//
                int sampleId = (int)sample.sampleID;//

                int target = (sampleId == 3) ? 1 : -1;// map class id to sig function output

                int guess = Brain.Guess(inputs);

                if (guess == target)
                {
                    Pen pen = new Pen(Color.Green, THICKNESS);
                    Point point = SampleToPoint(sample);
                    DrawCrossGuess(pen, point.X, point.Y);
                    ++numberOfCorrectGuesses;
                }
                else
                {
                    Pen pen = new Pen(Color.Red, THICKNESS);
                    Point point = SampleToPoint(sample);
                    DrawCrossGuess(pen, point.X, point.Y);
                    ++numberOfWrongGuesses;
                }
            }
        }

        private void btnRemoveSelected_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        /// <summary>
        /// Draw seperating line on cartesian space.
        /// </summary>
        /// <see cref="GenerateRandomPointsAndSamples(int)"/>
        private void DrawSeperatingLine()
        {
            if (RandomMode || TrainMode)
            {
                Pen pen = new Pen(Color.Black, THICKNESS);
                
                // cartesian points
                float leftX, leftY, rightX, rightY; 
                //picturebox points
                float leftXPb, leftYPb, rightXPb, rightYPb;

                leftX = -pbCartesianBox.Width / 2;
                leftY = F(leftX);

                leftXPb = leftX + pbCartesianBox.Width / 2;
                leftYPb = pbCartesianBox.Height / 2 - leftY;

                rightX = pbCartesianBox.Width / 2;
                rightY = F(rightX);

                rightXPb = rightX + pbCartesianBox.Width / 2;
                rightYPb = pbCartesianBox.Height / 2 - rightY;

                G.DrawLine(pen, leftXPb, leftYPb, rightXPb, rightYPb);
            }

        }

        private void DrawGuessedSeperatingLine(  )
        {
            //pen to draw line
            Pen pen = new Pen(Color.Orange, THICKNESS-2);

            // cartesian points
            float leftX, leftY, rightX, rightY;
            //picturebox points
            float leftXPb, leftYPb, rightXPb, rightYPb;

            leftX = -pbCartesianBox.Width / 2;
            leftY = Brain.GuessY(leftX);

            leftXPb = leftX + pbCartesianBox.Width / 2;
            leftYPb = pbCartesianBox.Height / 2 - leftY;

            rightX = pbCartesianBox.Width / 2;
            rightY = Brain.GuessY(rightX);


            rightXPb = rightX + pbCartesianBox.Width / 2;
            rightYPb = pbCartesianBox.Height / 2 - rightY;


            G.DrawLine(pen, leftXPb, leftYPb, rightXPb, rightYPb);
        }

        private void btn_addWcpu_Click(object sender, EventArgs e)
        {

        }

        private void showToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            btn_reset_Click(sender, e);

            RandomMode = true;
            TrainMode = false;

            //Point topLeft = new Point(0, 0);
            //Point bottomRight = new Point(pbCartesianBox.Width, pbCartesianBox.Height);

            if (G == null) G = pbCartesianBox.CreateGraphics();
            
            DrawSeperatingLine();

            DrawRandomSamples();

            btn_reset.Enabled = true;
            lblNoOfInputs.Text = $"# of inputs:{samples.Count}";
        }

        private void trainToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TrainMode = true;
            RandomMode = false;

            G = pbCartesianBox.CreateGraphics();
            ResetList();
            GenerateRandomPointsAndSamples(10);
            ReDrawSamples();
            WireUpList();
            //Train();

            lblNoOfInputs.Text = $"# of inputs:{samples.Count}";
            lblNoOfCorrectGuesses.Text = "";

        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            if (lboxSamples.SelectedItem != null)
            {
                Sample s = (Sample)lboxSamples.SelectedItem;

                samples.Remove(s);
                points.Remove(SampleToPoint(s));
                WireUpList();
                pbCartesianBox.Refresh();

                if (RandomMode || TrainMode) DrawSeperatingLine();
                
                ReDrawSamples();

                if (samples.Count == 0) btnRemoveSelected.Enabled = false;
            }
            btnRemoveSelected.Enabled = false;
        }

        
        private static float prevRate = Brain.learningRate;
        private static bool LearningRateChanged(float currentLr)
        {
            return (currentLr != prevRate);
        }

        private static UInt32 lineRunned = 0;
        private void autoTrainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trainToolStripMenuItem1_Click(sender, e);
            lblNoOfCorrectGuesses.Text = "";
            var sw = Stopwatch.StartNew();
            
            while ( numberOfCorrectGuesses != samples.Count )
            {
                ReDrawSamples();
                Train();
                DrawGuessedSeperatingLine();
                MakeAGuess();
                if (numberOfCorrectGuesses > samples.Count * 0.85F)
                {
                    Brain.learningRate = 0.1F;
                    if (LearningRateChanged(Brain.learningRate))
                    {
                        String str = $"Correct Guesses: {numberOfCorrectGuesses}/{samples.Count}, lr:{Brain.learningRate} at {numberOfIterations}th iteration";
                        lblNoOfCorrectGuesses.Text = str;
                        lineRunned++;
                        lblNoOfCorrectGuesses.Refresh();
                    }
                    prevRate = Brain.learningRate;
                }
                Thread.Sleep(1);
                ++numberOfIterations;
            }

            sw.Stop();

            var elapsedS = sw.ElapsedMilliseconds / 1000;


            lblNoOfIterations.Text = $"Number of iterations : {numberOfIterations}, time:{elapsedS}s";
            lblNoOfIterations.Refresh();
        }
    }
}
