using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Media;

namespace ApplicationTimeCounter
{
    public partial class MyButton : System.Windows.Forms.UserControl
    {
        MyRectangle bodyButton;
        MyLabel labelButton;

        public MyButton(Canvas _canvas, int width, int height, double x, double y,
            Color backColor, Color borderColor, Color labelColor, string contentLabel, int borderThickness = 0,
            double opacity = 1) 
        {
            bodyButton = new MyRectangle(_canvas, width + borderThickness * 2, height + borderThickness * 2,
                borderColor, (borderThickness * 2 > 0) ? x - borderThickness : x, (borderThickness * 2 > 0) ? y - borderThickness : y, 1);
        }
    }
}
