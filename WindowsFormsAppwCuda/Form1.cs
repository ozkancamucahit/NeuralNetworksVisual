using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsAppwCuda
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cb_useCuda.Checked = false;
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

        private void btn_addWcpu_Click(object sender, EventArgs e)
        {
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            addWCPU();
            sw2.Stop();

            MessageBox.Show("Time it took :" + System.Convert.ToString(sw2.ElapsedMilliseconds));

        }

        public void addWCPU()
        {
            UInt32 N = 1 << 20;

            float[] x = new float[N];
            float[] y = new float[N];

            for (UInt32 i = 0; i < N; ++i)
            {
                x[i] = 8.0F;
                y[i] = 9.0F;
            }

            for (UInt32 i = 0; i < N; ++i)
                y[i] += x[i];

        }

        private void cb_useCuda_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cuda = sender as CheckBox;
            if (cuda.Checked)
                MessageBox.Show("Cuda enabled");
            else                                
                MessageBox.Show("Cuda disabled");
        }

        private void pbCartesianBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics  g = e.Graphics;
            g.DrawLine(System.Drawing.Pens.Black,
                pbCartesianBox.Width / 2, 0, pbCartesianBox.Width / 2, pbCartesianBox.Height);

            g.DrawLine(System.Drawing.Pens.Black,
                0, pbCartesianBox.Height / 2, pbCartesianBox.Width, pbCartesianBox.Height / 2);
        }

        private const int LINE_LENGT = 5;
        private void DrawPlus(Pen pen, int posX, int posY, float thickness)
        {
            //Pen pen = new Pen(Color.Black, thickness);
            pbCartesianBox.CreateGraphics().DrawLine(pen, posX - LINE_LENGT, posY, posX + LINE_LENGT, posY); // draw horizontal line
            pbCartesianBox.CreateGraphics().DrawLine(pen, posX, posY - LINE_LENGT, posX, posY + LINE_LENGT); // draw vertical line
            
        }

        private const float THICKNESS = 3.0F;
        private void pbCartesianBox_MouseClick(object sender, MouseEventArgs e)
        {
            int selectedClass=0;
            if ( comboxInputs.SelectedItem != null)
                selectedClass =  Convert.ToInt32( comboxInputs.SelectedItem.ToString() );
            
            Pen pen;

            switch (selectedClass)
            {
                case 1:
                    pen = new Pen(Color.Red, THICKNESS);
                    DrawPlus(pen, e.X, e.Y, THICKNESS);

                    break;
                case 2:
                    pen = new Pen(Color.Green, THICKNESS);
                    DrawPlus(pen, e.X, e.Y, THICKNESS);

                    break;
                case 3:
                    pen = new Pen(Color.Blue, THICKNESS);
                    DrawPlus(pen, e.X, e.Y, THICKNESS);

                    break;

                case 4:
                    pen = new Pen(Color.Brown, THICKNESS);
                    DrawPlus(pen, e.X, e.Y, THICKNESS);

                    break;
                default:
                    MessageBox.Show("Invalid class selection");
                    break;
            }

        }

        private void comboxNoOfClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string item = (string)comboBox.SelectedItem;
            int maxClassNo = Convert.ToInt32(item);

            comboxInputs.Items.Clear();
            comboxInputs.Enabled = true;
            for (int i = 1; i <= maxClassNo; i++)
                comboxInputs.Items.Add( Convert.ToString(i) );
            comboxInputs.SelectedIndex = 0;

        }
    }
}
