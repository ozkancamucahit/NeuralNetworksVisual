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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsAppWCUDA
{
    public partial class Form1 : Form
    {
        private List<Sample> samples = new List<Sample>(); // samples with class ids
        private List<Point> points = new List<Point>(); // points on the picture box
        public Form1()
        {
            InitializeComponent();
            Matrix matrix = new Matrix();
            
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

        private const int LINE_LENGT = 8;
        private const float THICKNESS = 3.0F;

        private void DrawPlus(Pen pen, int posX, int posY, float thickness)
        {
            //Pen pen = new Pen(Color.Black, thickness);
            pbCartesianBox.CreateGraphics().DrawLine(pen, posX - LINE_LENGT, posY, posX + LINE_LENGT, posY); // draw horizontal line
            pbCartesianBox.CreateGraphics().DrawLine(pen, posX, posY - LINE_LENGT, posX, posY + LINE_LENGT); // draw vertical line
            

        }

        private Point SampleToPoint(Sample sample)
        {
            int posX = sample.X + pbCartesianBox.Width / 2;
            int posY = pbCartesianBox.Height / 2 - sample.Y;
            return new Point( posX, posY );
        }

        private void DrawRectangle(Sample sample)
        {
            Pen pen = new Pen(Color.Black, THICKNESS-1);

            Point point = SampleToPoint(sample); // current sample
            Point topLineLeft = new Point(point.X - LINE_LENGT, point.Y - LINE_LENGT);
            Rectangle rectangle = new Rectangle(topLineLeft.X, topLineLeft.Y, LINE_LENGT * 2, LINE_LENGT * 2);

            pbCartesianBox.CreateGraphics().DrawRectangle(pen, rectangle);

            
            foreach (Sample s in samples)
            {
                Point other = SampleToPoint(s);

                if (point == other) continue;

                else 
                {
                    topLineLeft = new Point(other.X - LINE_LENGT, other.Y - LINE_LENGT);
                    rectangle = new Rectangle(topLineLeft.X, topLineLeft.Y, LINE_LENGT * 2, LINE_LENGT * 2);
                    pen = new Pen(Color.White, THICKNESS-1);
                    pbCartesianBox.CreateGraphics().DrawRectangle(pen, rectangle); 
                }
            }

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
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
                    DrawPlus(pen, e.X, e.Y, THICKNESS);
                    
                    AddSampleToList(new Sample(e.X - pbCartesianBox.Width/2, pbCartesianBox.Height/2 - e.Y, CLASSID.CLASS1));
                    break;
                case 2:
                    pen = new Pen(Color.Green, THICKNESS);
                    DrawPlus(pen, e.X, e.Y, THICKNESS);
                    
                    AddSampleToList(new Sample(e.X - pbCartesianBox.Width/2, pbCartesianBox.Height/2 - e.Y, CLASSID.CLASS2));

                    break;
                case 3:
                    pen = new Pen(Color.Blue, THICKNESS);
                    DrawPlus(pen, e.X, e.Y, THICKNESS);
                    
                    AddSampleToList(new Sample(e.X - pbCartesianBox.Width/2, pbCartesianBox.Height/2 - e.Y, CLASSID.CLASS3));

                    break;

                case 4:
                    pen = new Pen(Color.Brown, THICKNESS);
                    DrawPlus(pen, e.X, e.Y, THICKNESS);
                    
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
            lboxSamples.Enabled = false;
        }
        private void btn_reset_Click(object sender, EventArgs e)
        {
            comboxInputs.SelectedIndex = 0;
            comboxInputs.Enabled = false;
            pbCartesianBox.Refresh();
            btn_reset.Enabled = false;
            btnSet.Enabled = true;

            ResetList();

            if (comboxNoOfClasses.SelectedItem != null)
            { 
                comboxInputs.Enabled = true;
                //comboxInputs.SelectedIndex = 0;
            }

            //comboxNoOfClasses.SelectedIndex = 0;
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
        /// Called when sample is removed from sample data
        /// </summary>
        private void ReDrawSamples()
        {
            foreach (var sample in samples)
            {
                Point pointOnPictureBox = SampleToPoint(sample);
                Pen pen;

                switch (sample.sampleID)
                {
                    case CLASSID.CLASS1:
                        pen = new Pen(Color.Red, THICKNESS);
                        DrawPlus(pen, pointOnPictureBox.X, pointOnPictureBox.Y, THICKNESS);

                        break;
                    case CLASSID.CLASS2:
                        pen = new Pen(Color.Green, THICKNESS);
                        DrawPlus(pen, pointOnPictureBox.X, pointOnPictureBox.Y, THICKNESS);

                        break;
                    case CLASSID.CLASS3:
                        pen = new Pen(Color.Blue, THICKNESS);
                        DrawPlus(pen, pointOnPictureBox.X, pointOnPictureBox.Y, THICKNESS);

                        break;
                    case CLASSID.CLASS4:
                        pen = new Pen(Color.Brown, THICKNESS);
                        DrawPlus(pen, pointOnPictureBox.X, pointOnPictureBox.Y, THICKNESS);

                        break;
                    default:
                        break;
                }
            }
        }

        private void btnRemoveSelected_MouseClick(object sender, MouseEventArgs e)
        {
            if (lboxSamples.SelectedItem != null)
            {
                Sample s = (Sample)lboxSamples.SelectedItem;
                
                samples.Remove(s);
                points.Remove(SampleToPoint(s));
                WireUpList();
                pbCartesianBox.Refresh();
                ReDrawSamples();

                if (samples.Count == 0) btnRemoveSelected.Enabled = false;
            }
        }
    }
}
