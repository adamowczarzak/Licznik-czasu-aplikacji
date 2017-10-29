using System;
using System.Windows;
using System.Windows.Forms;

namespace ApplicationTimeCounter
{
    class IconInTaskbar
    {

        private NotifyIcon notifyIcon;
        private MainWindow mainWindow;
        private HomeForm homeForm;


        public IconInTaskbar(ref MainWindow mainWindow, ref HomeForm homeForm)
        {
            this.mainWindow = mainWindow;
            this.homeForm = homeForm;
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon("../../Pictures/icon16.ico");
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_Click);

        }


        private void notifyIcon_Click(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                homeForm.UpdateView();
                homeForm.ShowHomeForm();
                SetMainWindow();
            }
        }

        public void SetMainWindow()
        {
            if (mainWindow.Visibility == Visibility.Hidden)
                mainWindow.Visibility = Visibility.Visible;
            else mainWindow.Visibility = Visibility.Hidden;
        }

        public void DisposeNotifyIcon()
        {
            notifyIcon.Dispose();
        }
    }
}
