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
        public bool AutoTrainStopped { get; private set; } = false;

        public bool AutoTrainClicked { get; private set; }

        private static UInt32 numberOfCorrectGuesses = 0;
        private static UInt32 numberOfWrongGuesses = 0;
        private static UInt32 numberOfIterations = 0;
        public static Perceptron Brain { get; private set; } = new Perceptron(lr:0.5F);
        public static Pen Pen_gs { get; set; } = new Pen(Color.Black, 1.5F);
        public static SolidBrush Brus_gs { get; set; } = new SolidBrush(Color.White);

        public delegate void SafeCallDelegate();
        public delegate void DelegateNumOfCorrectGuesses(string str);
        public delegate void DelegateNumOfIterations(long milliseconds);
        public delegate void DelegateSetBtnResetsText();
        public delegate void DelegateDrawRectangle(Rectangle rectangle);

        public NeuralNetwork NeuralNet { get; private set; }



        //thread for autotrain
        private Thread thread = null;
        public Thread ThreadTrainXOR { get; private set; }
        public Thread ThreadDrawXOR { get; private set; }

        public Graphics G { get; private set; }

        public float F(float x)
        {
            return  x + 80.0F;// + 0.2F;
        }

        public Form1()
        {
            InitializeComponent();
            G = pbCartesianBox.CreateGraphics();
            
            Matrix m = new(2,2);
            m.Randomize(0, 1);

            ActivationFunction activationFunction = ActivationFunctions.SigmoidFunction;
            ActivationFunction activationFunctionDerivative = ActivationFunctions.SigmoidFunctionDerivative;

            NeuralNet = new NeuralNetwork(2, 4, 1, activationFunction, activationFunctionDerivative, 0.1F);

            //var nn = new NeuralNetwork(2, 4, 1, activationFunction, activationFunctionDerivative, 0.1F);
            //Random random = new Random();
            //TrainingSet set1 = new TrainingSet(new float[] { 0, 0 }, new float[] { 0 });
            //TrainingSet set2 = new TrainingSet(new float[] { 0, 1 }, new float[] { 1 });
            //TrainingSet set3 = new TrainingSet(new float[] { 1, 0 }, new float[] { 1 });
            //TrainingSet set4 = new TrainingSet(new float[] { 1, 1 }, new float[] { 0 });

            //TrainingSet[] trainingData = { set1, set2, set3, set4 };

            //for (int i = 0; i < 20_000; i++)
            //{
            //    var index = random.Next(trainingData.Length);
            //    NeuralNet.Train(trainingData[index].Inputs, trainingData[index].Targets);

            //}

            //float[] res1 = NeuralNet.FeedForward(set1.Inputs);
            //float[] res2 = NeuralNet.FeedForward(set2.Inputs);
            //float[] res3 = NeuralNet.FeedForward(set3.Inputs);
            //float[] res4 = NeuralNet.FeedForward(set4.Inputs);


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
            comboxInputs.SelectedIndex = -1;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //Pen_gs = new Pen(Color.Black, 1.5F);
            Pen_gs.Color = Color.Black;
            Pen_gs.Width = 1.5F;
            g.DrawLine(Pen_gs,
                pbCartesianBox.Width / 2, 0, pbCartesianBox.Width / 2, pbCartesianBox.Height);

            g.DrawLine(Pen_gs,
                0, pbCartesianBox.Height / 2, pbCartesianBox.Width, pbCartesianBox.Height / 2);
        }



        private const int LINE_LENGT = 10;
        private const float THICKNESS = 3.0F;

        private void DrawPlus(Pen pen, int posX, int posY)
        {
            //Pen pen = new Pen(Color.Black, thickness);
            G.DrawLine(pen, posX - LINE_LENGT, posY, posX + LINE_LENGT, posY); // draw horizontal line
            G.DrawLine(pen, posX, posY - LINE_LENGT, posX, posY + LINE_LENGT); // draw vertical line
        }

        private void DrawCrossGuess(Pen pen, int posX, int posY)
        {
            //Pen pen = new Pen(Color.Black, thickness);
            G.DrawLine(pen, posX - LINE_LENGT/2, posY - LINE_LENGT/2, posX + LINE_LENGT/2, posY + LINE_LENGT/2); // draw horizontal line
            G.DrawLine(pen, posX - LINE_LENGT / 2, posY + LINE_LENGT/2, posX + LINE_LENGT / 2, posY - LINE_LENGT/2); // draw vertical line
        }


        private Point SampleToPoint(Sample sample)
        {
            int posX = sample.X + pbCartesianBox.Width / 2;
            int posY = pbCartesianBox.Height / 2 - sample.Y;
            return new Point( posX, posY );
        }

        private void DrawRectangle(Sample sample)
        {
            //pen = new Pen(Color.Black, THICKNESS-2.5F);
            Pen_gs.Width = THICKNESS - 2.5F;
            Pen_gs.Color = Color.Black;

            Point point = SampleToPoint(sample); // current sample
            Point topLineLeft = new Point(point.X - LINE_LENGT, point.Y - LINE_LENGT);
            Rectangle rectangle = new Rectangle(topLineLeft.X, topLineLeft.Y, (LINE_LENGT+1) * 2, (LINE_LENGT+1) * 2);

            
            G.DrawRectangle(Pen_gs, rectangle);

            foreach (Sample s in samples)
            {
                Point other = SampleToPoint(s);

                if (point == other) continue;

                else
                {
                    topLineLeft = new Point(other.X - LINE_LENGT, other.Y - LINE_LENGT);
                    rectangle = new Rectangle(topLineLeft.X, topLineLeft.Y, (LINE_LENGT + 1) * 2, (LINE_LENGT + 1) * 2);
                    Pen_gs.Color = Color.White;
                    G.DrawRectangle(Pen_gs, rectangle);
                }
            } 

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            if (G == null) G = pbCartesianBox.CreateGraphics();

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

            //Pen pen;
            lboxSamples.Enabled = true;
            lblInputText.Text = $"x: {e.X}, y: {e.Y}";
            lblInputText.Visible = true;

            switch (selectedClass)
            {
                case 1:
                    //pen = new Pen(Color.Red, THICKNESS);
                    Pen_gs.Color = Color.Red;
                    Pen_gs.Width = THICKNESS;
                    DrawPlus(Pen_gs, e.X, e.Y);
                    
                    AddSampleToList(new Sample(e.X - pbCartesianBox.Width/2, pbCartesianBox.Height/2 - e.Y, CLASSID.CLASS1));
                    break;
                case 2:
                    //pen = new Pen(Color.Green, THICKNESS);
                    Pen_gs.Color = Color.Green;
                    DrawPlus(Pen_gs, e.X, e.Y);
                    
                    AddSampleToList(new Sample(e.X - pbCartesianBox.Width/2, pbCartesianBox.Height/2 - e.Y, CLASSID.CLASS2));

                    break;
                case 3:
                    //pen = new Pen(Color.Blue, THICKNESS);
                    Pen_gs.Color = Color.Blue;
                    DrawPlus(Pen_gs, e.X, e.Y);
                    
                    AddSampleToList(new Sample(e.X - pbCartesianBox.Width/2, pbCartesianBox.Height/2 - e.Y, CLASSID.CLASS3));

                    break;

                case 4:
                    //pen = new Pen(Color.Brown, THICKNESS);
                    Pen_gs.Color = Color.Brown;
                    DrawPlus(Pen_gs, e.X, e.Y);
                    
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
            lboxSamples.Refresh();
            lboxSamples.SelectedIndexChanged += lboxSamples_SelectedIndexChanged;

            lboxSamples.ClearSelected();
            lboxSamples.Enabled = true;
            btn_reset.Enabled = true;
        }


        [DllImport("CudaLib.dll")]
        static extern int callCuda();
        private void btn_addWCuda_Click(object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();
            int status = callCuda();
            sw.Stop();
            MessageBox.Show("Time it took :" + System.Convert.ToString(sw.ElapsedMilliseconds));

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
            comboxInputs.SelectedIndex = -1;
            comboxInputs.Enabled = false;

            comboxNoOfClasses.SelectedIndex = -1;
            comboxNoOfClasses.Enabled = true;

            btn_reset.Enabled = false;
            btnSet.Enabled = true;

            //btn_reset.Text = "RESET";

            if (!AutoTrainStopped) AutoTrainStopped = true;

            numberOfWrongGuesses = 0;
            numberOfCorrectGuesses = 0;
            numberOfIterations = 0;
            pbCartesianBox.Refresh();

            lblNoOfInputs.Text = "";
            lblNoOfInputs.Refresh();

            lblNoOfCorrectGuesses.Text = "";
            lblNoOfCorrectGuesses.Refresh();

            lblNoOfIterations.Text = "";
            lblNoOfIterations.Refresh();

            lblInputText.Text = "";
            lblInputText.Refresh();


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

        Point pointOnPictureBox;
        //Pen pen;
        
        /// <summary>
        /// Called when sample is removed from sample data
        /// </summary>
        private void ReDrawSamples()
        {
            pbCartesianBox.Refresh();
            if (RandomMode || TrainMode) DrawSeperatingLine();

            Pen_gs.Width = THICKNESS;

            foreach (var sample in samples)
            {
                pointOnPictureBox = SampleToPoint(sample);

                switch (sample.sampleID)
                {
                    case CLASSID.CLASS1:
                        Pen_gs.Color = Color.Red;
                        DrawPlus(Pen_gs, pointOnPictureBox.X, pointOnPictureBox.Y);

                        break;
                    case CLASSID.CLASS2:
                        Pen_gs.Color = Color.Green;
                        DrawPlus(Pen_gs, pointOnPictureBox.X, pointOnPictureBox.Y);

                        break;
                    case CLASSID.CLASS3:
                        //pen = new Pen(Color.Blue, THICKNESS);
                        Pen_gs.Color = Color.Blue;
                        DrawPlus(Pen_gs, pointOnPictureBox.X, pointOnPictureBox.Y);

                        break;
                    case CLASSID.CLASS4:
                        Pen_gs.Color = Color.Brown;
                        DrawPlus(Pen_gs, pointOnPictureBox.X, pointOnPictureBox.Y);

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

        float[] inputs;
        private void Train()
        {

            foreach (var sample in samples)
            {
                inputs = new float[] { sample.X,  sample.Y, sample.Bias };//
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
                inputs = new float [] {  sample.X, sample.Y, sample.Bias };//
                int sampleId = (int)sample.sampleID;//

                float target = (sampleId == 3) ? 1 : -1;// map class id to sig function output

                float guess = Brain.Guess(inputs);

                Pen_gs.Width = THICKNESS;

                if (guess == target)
                {
                    //pen = new Pen(Color.Green, THICKNESS);
                    Pen_gs.Color = Color.Green;
                    pointOnPictureBox = SampleToPoint(sample);
                    DrawCrossGuess(Pen_gs, pointOnPictureBox.X, pointOnPictureBox.Y);
                    ++numberOfCorrectGuesses;
                }
                else
                {
                    //pen = new Pen(Color.Red, THICKNESS);
                    Pen_gs.Color = Color.Red;
                    pointOnPictureBox = SampleToPoint(sample);
                    DrawCrossGuess(Pen_gs, pointOnPictureBox.X, pointOnPictureBox.Y);
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
                Pen_gs.Width = THICKNESS;
                //pen = new Pen(Color.Black, THICKNESS);
                Pen_gs.Color = Color.Black;
                
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

                G.DrawLine(Pen_gs, leftXPb, leftYPb, rightXPb, rightYPb);
            }

        }

        
        //Pen penGuessed = new Pen(Color.Orange, THICKNESS-2);
        private void DrawGuessedSeperatingLine(  )
        {
            //pen to draw line

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
            Pen_gs.Width = THICKNESS - 2;
            Pen_gs.Color = Color.Orange;

            if ( (!AutoTrainStopped) || TrainMode )
                G.DrawLine(Pen_gs, leftXPb, leftYPb, rightXPb, rightYPb);
        }

        private void btn_addWcpu_Click(object sender, EventArgs e)
        {
            long elapsedMs = Example.RunExample();
            MessageBox.Show($"Time it took : {elapsedMs}");
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

            if (G == null) G = pbCartesianBox.CreateGraphics();
            ResetList();

            if (AutoTrainClicked) GenerateRandomPointsAndSamples(100);
            
            else GenerateRandomPointsAndSamples(10);

            ReDrawSamples();
            WireUpList();

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
        private void autoTrainToolStripMenuItem_ClickAsync(object sender, EventArgs e)
        {
            
            AutoTrainStopped = false;
            AutoTrainClicked = true;
            trainToolStripMenuItem1_Click(sender, e);
            lblNoOfCorrectGuesses.Text = "";
            lblNoOfIterations.Text = "";
            btn_reset.Text = "STOP";
            //btn_reset.Text = "Stop";

            thread = new Thread(new ThreadStart(AutoTrainAsync));
            thread.Name = "AutoTrain";
            thread.Start();
            //lblNoOfIterations.Refresh();
        }

        private void AutoTrainAsync()
        {
            var sw = Stopwatch.StartNew();

            var ReDrawSamplesSafe = new SafeCallDelegate(ReDrawSamples);
            var DrawGuessedSeperatingLineSafe = new SafeCallDelegate(DrawGuessedSeperatingLine);
            var MakeAGuessSafe = new SafeCallDelegate(MakeAGuess);
            var SetLblNoOfCorrectGuessesTextSafe = new DelegateNumOfCorrectGuesses(SetLblNoOfCorrectGuessesText);
            var SetLblNoOfIterationsTextSafe = new DelegateNumOfIterations(SetLblNoOfIterationsText);
            var SetResetButtonTextAfterAutoTrainSafe = new DelegateSetBtnResetsText(SetResetButtonTextAfterAutoTrain);


            while ((numberOfCorrectGuesses != samples.Count) && (!AutoTrainStopped))
            {
                pbCartesianBox.Invoke(ReDrawSamplesSafe);
                Train();
                pbCartesianBox.Invoke(DrawGuessedSeperatingLineSafe);

                pbCartesianBox.Invoke(MakeAGuessSafe);

                if (numberOfCorrectGuesses > samples.Count * 0.85F)
                {
                    Brain.learningRate = 0.1F;
                    if (LearningRateChanged(Brain.learningRate))
                    {
                        String str = $"Correct Guesses: {numberOfCorrectGuesses}/{samples.Count}, lr:{Brain.learningRate} at {numberOfIterations}th iteration";
                        lblNoOfCorrectGuesses.Invoke(SetLblNoOfCorrectGuessesTextSafe, str);
                        lineRunned++;
                        prevRate = Brain.learningRate;
                    }
                }
                Thread.Sleep(12);
                numberOfIterations++;
            }

            sw.Stop();
            var elapsedMs = sw.ElapsedMilliseconds;
            AutoTrainStopped = true;
            AutoTrainClicked = false;

            btn_reset.Invoke(SetResetButtonTextAfterAutoTrainSafe);
            lblNoOfIterations.Invoke(SetLblNoOfIterationsTextSafe, elapsedMs);

            numberOfCorrectGuesses = 0;
            numberOfWrongGuesses = 0;
            numberOfIterations = 0;



        }

        private void SetResetButtonTextAfterAutoTrain()
        {
            if (AutoTrainStopped) btn_reset.Text = "RESET";
            else btn_reset.Text = "STOP";
        }

        private void SetLblNoOfCorrectGuessesText(String str)
        {
            lblNoOfCorrectGuesses.Text = str;
            lblNoOfCorrectGuesses.Refresh();
        }

        private void SetLblNoOfIterationsText(long elapsedMs)
        {
            if (btn_reset.Text == "STOP")
            { 
                lblNoOfIterations.Text = "";
                return;
            }

            lblNoOfIterations.Text = $"Number of iterations : {numberOfIterations}, time:{elapsedMs}ms";
            lblNoOfIterations.Refresh();
            //lblNoOfIterations.Text = "";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        int resolution = 10;
        Color brushColor;
        private void xORExampleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            AutoTrainStopped = false;
            numberOfIterations = 0;

            ThreadDrawXOR = new Thread(new ThreadStart(GenerateRandomRectangles));
            ThreadDrawXOR.Name = "ThreadXORDrawRect";
            ThreadDrawXOR.Start();

            ThreadTrainXOR = new Thread(new ThreadStart(TrainXor));
            ThreadTrainXOR.Name = "ThreadTrainXOR";
            ThreadTrainXOR.Start();

            btn_reset.Enabled = true;
            comboxNoOfClasses.Enabled = false;
            
        }

        /// <summary>
        /// Runs on different thread.
        /// </summary>
        private void GenerateRandomRectangles()
        {
            float cols = pbCartesianBox.Width / resolution;
            float rows = pbCartesianBox.Height / resolution;

            var DrawRectangleSafe = new DelegateDrawRectangle(DrawRectangle);
            var ReDrawSamplesSafe = new SafeCallDelegate(ReDrawSamples);
            Random random = new();
            Rectangle rectangle = new Rectangle();
            int random_value;

            float x1, x2;
            float[] inputs;

            ThreadTrainXOR.Join();

            TrainingSet set1 = new TrainingSet(new float[] { 0, 0 }, new float[] { 0 });
            TrainingSet set2 = new TrainingSet(new float[] { 0, 1 }, new float[] { 1 });
            TrainingSet set3 = new TrainingSet(new float[] { 1, 0 }, new float[] { 1 });
            TrainingSet set4 = new TrainingSet(new float[] { 1, 1 }, new float[] { 0 });

            TrainingSet[] trainingData = { set1, set2, set3, set4 };

            

            while ( !AutoTrainStopped)
            {

                for (int i = 0; i < 10; i++)
                {
                    var index = random.Next(trainingData.Length);
                    NeuralNet.Train(trainingData[index].Inputs, trainingData[index].Targets);
                    numberOfIterations++;
                }

                if ( numberOfIterations % 10_000 == 0)
                {
                    for (int i = 0; i < cols; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {

                            x1 = i / cols;
                            x2 = j / rows;
                            inputs = new float[] { x1, x2 };
                            var y = NeuralNet.FeedForward(inputs);

                            float res = y[0] * 255;
                            int den = (int)res;
                            random_value = random.Next((int)res);
                            brushColor = Color.FromArgb(random_value, random_value, random_value);
                            Brus_gs.Color = brushColor;

                            rectangle.X = i * resolution;
                            rectangle.Y = j * resolution;
                            rectangle.Width = resolution;
                            rectangle.Height = resolution;
                            //G.FillRectangle(Brus_gs, i * resolution, j * resolution, resolution, resolution);
                            pbCartesianBox.Invoke(DrawRectangleSafe, rectangle);
                        }
                    } 
                }
                
            }

            pbCartesianBox.Invoke(ReDrawSamplesSafe);

        }

        private void DrawRectangle( Rectangle rect)
        {
            G.FillRectangle(Brus_gs, rect);
        }

        private void TrainXor()
        {
            Random random = new Random();

            TrainingSet set1 = new TrainingSet(new float[] { 0, 0 }, new float[] { 0 });
            TrainingSet set2 = new TrainingSet(new float[] { 0, 1 }, new float[] { 1 });
            TrainingSet set3 = new TrainingSet(new float[] { 1, 0 }, new float[] { 1 });
            TrainingSet set4 = new TrainingSet(new float[] { 1, 1 }, new float[] { 0 });

            TrainingSet[] trainingData = { set1, set2, set3, set4 };


            for (int i = 0; i < 10_000; i++)
            {
                var index = random.Next(trainingData.Length);
                NeuralNet.Train(trainingData[index].Inputs, trainingData[index].Targets); 
            }
            
        }


    }
}
