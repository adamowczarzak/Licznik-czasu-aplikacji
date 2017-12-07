using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;
using ApplicationTimeCounter.Controls;

namespace ApplicationTimeCounter
{
    public class LoadingWindow
    {
        public Canvas LoadCanvas;
        private Canvas _canvas;
        public bool notClose;

        public LoadingWindow(ref Canvas _canvas)
        {
            this._canvas = _canvas;
            notClose = true;
            LoadCanvas = CanvasCreator.CreateCanvas(this._canvas, (int)_canvas.Width, (int)_canvas.Height, Color.FromArgb(255, 226, 240, 255), 0, 0);
        }

        public void Load(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            List<MyCircle> loadBar = new List<MyCircle>();
            LoadCanvas.Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    loadBar.Add(new MyCircle(LoadCanvas, 10, 0, (Color.FromArgb(255, 0, 123, 255)), -20, 100, setFill: true));
                }           
            });
            TranslateTransform newPosition;
            double[] positionX = new double[15];
            for (int i = 0; i < 10; i++ )
            {
                positionX[i] = 100 + i * 30;
            }
            while (notClose)
            {
                for (int i = 0; i < 10; i++)
                {
                    positionX[i] += (positionX[i] < 450) ? positionX[i] / 90.0 : (600.0 - positionX[i]) / 60.0;
                    if (positionX[i] > 550) positionX[i] = 120;
                    LoadCanvas.Dispatcher.Invoke(() =>
                    {
                        newPosition = new TranslateTransform(positionX[i], 100);
                        loadBar[i].RenderTransform(newPosition);
                        loadBar[i].Opacity((positionX[i] > 200 && positionX[i] < 450) ? 1 : (positionX[i] > 450) ? (550.0 - (double)positionX[i]) / 150.0 : (double)positionX[i] / 300.0);
                    });
                }
                if (notClose == false)
                {
                    Thread.ResetAbort();
                    break;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }                   
        }

        public void Close(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            for (int i = 0; i < 50; i++)
            {
                LoadCanvas.Dispatcher.Invoke(() => { LoadCanvas.Opacity -= 0.02; });
                Thread.Sleep(20);
            }
            _canvas.Dispatcher.Invoke(() => { _canvas.Children.Remove(LoadCanvas); });
        }
    }
}
