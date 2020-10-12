using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MandelbrotZoom
{
    public partial class MandelbrotZoom : Form
    {
        int NumberIterations;
        Stack<Tuple<double,double, double, double>> PreviousZooms;
        Tuple<int,int,int,int,int> Box;

        Color[] cols;

        Bitmap bm;
        PictureBox PicCanvas;

        double MagnitudeThreshold;
        double RangeX, RangeY;
        double StartX, StartY;
        double dx, dy;

        bool IterativeColoring;
        bool Started;

        public MandelbrotZoom()
        {
            InitializeComponent();

            NumberIterations = 200;
            MagnitudeThreshold = 100;
            IterativeColoring = false;
            BoxCreated = false;
            Started = false;
           
            this.SetupPictureBox();
            this.SetupColors();

            this.MouseDown += new MouseEventHandler(MouseDownForm);

            PreviousZooms = new Stack<Tuple<double, double, double, double>>();
            Box = new Tuple<int, int, int, int, int>(-1, -1, -1, -1, 1);


            RangeX = RangeY = 4.0f;
            StartX = StartY = -2.0f;
            dx = RangeX / PicCanvas.ClientSize.Width;
            dy = RangeY / PicCanvas.ClientSize.Height;
        }

        private void SetupPictureBox()
        {
            //PictureBox initialization
            this.PicCanvas = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PicCanvas)).BeginInit();
            this.PicCanvas.Name = "PicCanvas";
            this.PicCanvas.Location = new System.Drawing.Point(180, 10);
            this.PicCanvas.Size = new System.Drawing.Size(1000, 1000);
            this.Controls.Add(this.PicCanvas);
            ((System.ComponentModel.ISupportInitialize)(this.PicCanvas)).EndInit();

            this.PicCanvas.MouseUp += new MouseEventHandler(MouseUpCanvasEvent);
            this.PicCanvas.MouseDown += new MouseEventHandler(MouseDownCanvasEvent);
            this.PicCanvas.MouseMove += new MouseEventHandler(MouseMoveCanvasEvent);

            bm = new Bitmap(PicCanvas.ClientSize.Width, PicCanvas.ClientSize.Height);

            DrawBox(0, 0, PicCanvas.ClientSize.Width, PicCanvas.ClientSize.Height, PicCanvas.ClientSize.Width, Color.Black);
            PaintCanvas(null, null);
        }

        private void SetupColors()
        {
            cols = new Color[NumberIterations];
            int Red = 50, Green = 5, Blue = 0, alpha = 255;
            for (int i = 0; i < NumberIterations; i++)
            {
                cols[i] = Color.FromArgb(alpha, Red, Green, Blue);
                Red += 18;
                if (Red > 255)
                {
                    Red = 0;
                    Green += 15;
                }
                if (Green > 255)
                {
                    Green = 0;
                    Blue += 1;
                }
                if (Blue > 255)
                {
                    Red = 255;
                    Green = 255;
                    Blue = 255;
                }
            }
        }

        private void Draw()
        {
            int px = 0, py = 0;

            using (Graphics g = Graphics.FromImage(bm))
            {
                g.Clear(Color.Black);


                while (px < PicCanvas.ClientSize.Width)
                {

                    while (py < PicCanvas.ClientSize.Height)
                    {
                        TestPixel(px, py);

                        py += 1;
                    }
                    //PaintCanvas(null, null);
                    py = 0;
                    px += 1;
                }
            }
            PaintCanvas(null, null);
        }

        private void Start(string loadMessage = "Loading")
        {
            ZoomInButton.Text = loadMessage;
            ZoomInButton.ForeColor = Color.Red;
            ZoomInButton.Refresh();
            Draw();
            Started = true;
            ZoomInButton.Text = "Zoom In";
            ZoomInButton.ForeColor = Color.Black;
        }
        private void CreateZoomBox()
        {
            int mouseDownX = MouseDownX, mouseDownY = MouseDownY;
            if(MouseUpX < mouseDownX)
            {
                int temp = mouseDownX;
                mouseDownX = MouseUpX;
                MouseUpX = temp;
            }

            if (MouseUpY < mouseDownY)
            {
                int temp = mouseDownY;
                mouseDownY = MouseUpY;
                MouseUpY = temp;
            }

            DrawZoomBox(mouseDownX, mouseDownY, MouseUpX - mouseDownX, MouseUpY - mouseDownY, Box.Item5);
        }

        private void DrawZoomBox(int x, int y, int sizeX, int sizeY, int width)
        {
            if (BoxCreated == true)
            {
                ReDrawPixels(Box);
            }
            DrawBox(x, y, sizeX, sizeY, width, Color.White);
            PaintCanvas(null, null);
        }

        private void ZoomIn(object sender, EventArgs e)
        {
            if (!Started)
            {
                Start();
            }
            if (!BoxCreated)
            {
                return;
            }

            BoxCreated = false;

            PreviousZooms.Push(new Tuple<double, double, double, double>(StartX,StartY,RangeX,RangeY));

            StartX = StartX + Box.Item1 * dx;
            StartY = StartY + Box.Item2 * dy;

            RangeX = Box.Item3 * dx;
            RangeY = Box.Item4 * dy;

            dx = RangeX / PicCanvas.ClientSize.Width;
            dy = RangeY / PicCanvas.ClientSize.Height;
            Start();   
        }

        private void DrawBox(int bx, int by, int sizeX, int sizeY, int width, Color col)
        {
            using(Graphics g = Graphics.FromImage(bm))
            {
                for(int i = by; i<by + width; i++)
                {                    
                    for(int j = bx; j<bx+sizeX; j++)
                    {
                        if (j >= 0 && j < PicCanvas.ClientSize.Width)
                        {
                            if (i >= 0 && i < PicCanvas.ClientSize.Height)
                            {
                                bm.SetPixel(j, i, col);
                            }
                            if (i + sizeY >= 0 && i + sizeY < PicCanvas.ClientSize.Height)
                            {
                                bm.SetPixel(j, i + sizeY,col);
                            }
                        }
                    }
                }

                for (int i = bx; i < bx + width; i++)
                {
                    for (int j = by; j < by + sizeY; j++)
                    {
                        if (j >= 0 && j < PicCanvas.ClientSize.Height)
                        {
                            if(i >=0 && i< PicCanvas.ClientSize.Width)
                            {
                                bm.SetPixel(i, j, col);
                            }
                            if(i+sizeX >=0 && i+sizeX < PicCanvas.ClientSize.Width)
                            {
                                bm.SetPixel(i + sizeX, j, col);
                            }
                        }
                    }
                }
            }

            Box = new Tuple<int, int, int, int, int>(bx, by, sizeX, sizeY, width);
        }

        private void ReDrawPixels(Tuple<int, int, int, int, int> box)
        {
            int bx = box.Item1, by = box.Item2, sizeX = box.Item3, sizeY = box.Item4, width = box.Item5;


            using (Graphics g = Graphics.FromImage(bm))
            {
                for (int i = by; i < by + width; i++)
                {
                    for (int j = bx; j < bx + sizeX; j++)
                    {
                        if (j >= 0 && j < PicCanvas.ClientSize.Width)
                        {
                            if (i >= 0 && i < PicCanvas.ClientSize.Height)
                            {
                                TestPixel(j, i);
                            }
                            if (i + sizeY >= 0 && i + sizeY < PicCanvas.ClientSize.Height)
                            {
                                TestPixel(j, i + sizeY);
                            }
                        }
                    }
                }

                for (int i = bx; i < bx + width; i++)
                {
                    for (int j = by; j < by + sizeY; j++)
                    {
                        if (j >= 0 && j < PicCanvas.ClientSize.Height)
                        {
                            if (i >= 0 && i < PicCanvas.ClientSize.Width)
                            {
                                TestPixel(i, j);
                            }
                            if (i + sizeX >= 0 && i + sizeX < PicCanvas.ClientSize.Width)
                            {
                                TestPixel(i + sizeX, j);
                            }
                        }
                    }
                }
            }
        }

        private void IterativeColoringButton_CheckedChanged(object sender, EventArgs e)
        {
            if(IterativeColoringButton.Checked == false)
            {
                IterativeColoring = false;
            }
            else
            {
                IterativeColoring = true;
            }
            Draw();
            if (BoxCreated == true)
            {
                DrawZoomBox(Box.Item1, Box.Item2, Box.Item3, Box.Item4, Box.Item5);
            }
        }

        private void TestPixel(int px, int py)
        {
            double cx = StartX + px * dx;
            double cy = StartY + py * dy;
            double x = 0, y = 0;
            double magnitude = 0;

            int i;
            for (i = 0; i < NumberIterations; i++)
            {

                magnitude = x * x + y * y;
                if (magnitude > MagnitudeThreshold)
                {
                    break;
                }

                double nextX = ((x * x) - (y * y)) + cx;
                double nextY = (2 * x * y) + cy;
                x = nextX;
                y = nextY;
            }
            if (magnitude > MagnitudeThreshold)
            {
                if (IterativeColoring == true)//.Checked == true)
                {
                    bm.SetPixel(px, py, cols[i]);
                }
                else
                {
                    bm.SetPixel(px, py, Color.Red);
                }
            }
            else
            {
                if (IterativeColoring == true)//.Checked == true)
                {
                    bm.SetPixel(px, py, cols[i - 1]);
                }
                else
                {
                    bm.SetPixel(px, py, Color.Blue);
                }
            }
        }

        private bool InsideBox(MouseEventArgs e)
        {
            return (BoxCreated && e.X >= Box.Item1 && e.X <= Box.Item1 + Box.Item3 && e.Y >= Box.Item2 && e.Y <= Box.Item2 + Box.Item4);
        }

        private void PaintCanvas(object sender, PaintEventArgs e)
        {
            PicCanvas.Image = bm;
            PicCanvas.Refresh();
        }
    }
}
