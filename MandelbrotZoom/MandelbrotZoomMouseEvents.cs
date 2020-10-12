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
    partial class MandelbrotZoom
    {
        int MouseDownX, MouseDownY;
        int MouseUpX, MouseUpY;

        bool MouseDrawing;
        bool BoxMoving;
        bool BoxCreated;

        int BoxOffsetX =0, BoxOffsetY = 0;

        private void MouseDownCanvasEvent(Object sender, MouseEventArgs e)
        {
            if (Started && e.Button == MouseButtons.Left && (BoxCreated == false || !InsideBox(e)))
            {
                MouseDrawing = true;
                MouseDownX = e.X;
                MouseDownY = e.Y;
            }
            else if(e.Button == MouseButtons.Left && BoxCreated && InsideBox(e))
            {
                BoxMoving = true;
                BoxOffsetX = Box.Item1 - e.X; //- Outline.Location.X;
                BoxOffsetY = Box.Item2 - e.Y; //- Outline.Location.Y;
            }
            else if(PreviousZooms.Count > 0)
            {
                BoxCreated= false;

                Tuple<double, double, double, double> Zoom = PreviousZooms.Pop();
                StartX = Zoom.Item1;
                StartY = Zoom.Item2;

                RangeX = Zoom.Item3;
                RangeY = Zoom.Item4;

                dx = RangeX / PicCanvas.ClientSize.Width;
                dy = RangeY / PicCanvas.ClientSize.Height;
                Start("Zooming Out");
            }
        }

        private void MouseUpCanvasEvent(Object sender, MouseEventArgs e)
        {
            if (MouseDrawing)
            {
                MouseDrawing = false;
                MouseUpX = e.X;
                MouseUpY = e.Y;

                CreateZoomBox();
            }
            if (BoxMoving)
            {
                BoxMoving = false;
            }
        }

        private void MouseMoveCanvasEvent(Object sender, MouseEventArgs e)
        {
            if (MouseDrawing)
            {
                MouseUpX = e.X;
                MouseUpY = e.Y;
                CreateZoomBox();
                BoxCreated = true;
                
            }
            if (BoxMoving)
            {
                DrawZoomBox(e.X + BoxOffsetX, e.Y + BoxOffsetY, Box.Item3, Box.Item4, Box.Item5);
                Box = new Tuple<int, int, int, int, int>(e.X + BoxOffsetX, e.Y + BoxOffsetY, Box.Item3, Box.Item4, Box.Item5);
            }

        }


        private void MouseDownForm(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right && PreviousZooms.Count >0)
            {
                BoxCreated = false;

                Tuple<double, double, double, double> Zoom = PreviousZooms.Pop();
                StartX = Zoom.Item1;
                StartY = Zoom.Item2;

                RangeX = Zoom.Item3;
                RangeY = Zoom.Item4;

                dx = RangeX / PicCanvas.ClientSize.Width;
                dy = RangeY / PicCanvas.ClientSize.Height;

                Start("Zooming Out");
            }
        }
    }
}
