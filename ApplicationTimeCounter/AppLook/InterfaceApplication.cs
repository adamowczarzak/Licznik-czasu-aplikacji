using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ApplicationTimeCounter
{
    class InterfaceApplication
    {

        private bool highLightHideWindow;


        public InterfaceApplication ()
        {
            highLightHideWindow = false;
        }

        public void IfCanHighLightHideWindow(Image hideWindow)
        {
            if (CheckIfCanHighLightHideWindow() == true) HighLightHideWindow(hideWindow);
        }

        public void DisableHighLightHideWindow(Image hideWindow)
        {
            hideWindow.Source = null;
            var uriSource = new Uri("Pictures/hideWindow.png", UriKind.Relative);
            hideWindow.Source = new BitmapImage(uriSource);
            highLightHideWindow = false;
        }

        private bool CheckIfCanHighLightHideWindow()
        {
            if (highLightHideWindow == false) return true;
            else return false;
        }

        private void HighLightHideWindow(Image hideWindow)
        {
            hideWindow.Source = null;
            var uriSource = new Uri("Pictures/hightLighthideWindow.png", UriKind.Relative);
            hideWindow.Source = new BitmapImage(uriSource);
            highLightHideWindow = true;
        }





        
    }
}
