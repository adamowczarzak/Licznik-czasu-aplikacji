using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ApplicationTimeCounter.Controls;

namespace ApplicationTimeCounter
{
    class BiggestResultsOfDay
    {
        private CircleBar circleBar;
        private MyLabel[] labels;
        private MyLabel[] labelLegend;
        private Color[] nameColor;
        private Canvas canvas;
        private DailyUseOfApplication_db dailyUseOfApplication_db;

        public BiggestResultsOfDay(Canvas canvas)
        {
            this.canvas = canvas;
            labels = new MyLabel[4];
            labelLegend = new MyLabel[4];
            nameColor = new Color[4];
            MyLabel title = new MyLabel(canvas, "Największe użycie", 140, 30, 14, 0, 0, Color.FromArgb(255, 47, 79, 79), Color.FromArgb(0, 0, 0, 0));
            circleBar = new CircleBar(canvas, 0.04, Color.FromArgb(255, 255, 255, 255), 10, -8, -8, 40, false);

            nameColor[0] = Color.FromArgb(255, 46, 139, 87);
            nameColor[1] = Color.FromArgb(255, 255, 69, 0);
            nameColor[2] = Color.FromArgb(255, 255, 165, 0);
            nameColor[3] = Color.FromArgb(255, 75, 0, 130);

            CreateSegmentRecordDay(2, 131, 0);
            CreateSegmentRecordDay(102, 131, 1);
            CreateSegmentRecordDay(2, 146, 2);
            CreateSegmentRecordDay(102, 146, 3);

            dailyUseOfApplication_db = new DailyUseOfApplication_db();
        }

        public void Update()
        {
            string[,] biggestResults = dailyUseOfApplication_db.GetBiggestResults();
            double[] partOfResults = GetDoubleBiggestResults(biggestResults);

            double sumpartOfResults = 0;
            for (int i = 0; i < 4; i++)
            {
                if (i > 0) sumpartOfResults += partOfResults[i - 1];
                if (partOfResults[i] > 0)
                    UpdateSegment(partOfResults[i], sumpartOfResults, i, biggestResults[i, 0]);
                else labels[i].Opacity(0);
            }
        }

        public void Reset()
        {
            circleBar.SetColorsOnParts(0, 1, Color.FromArgb(255, 255, 255, 255));
            for (int i = 0; i < 4; i++)
            {
                labels[i].Opacity(0);
            }
        }

        private double[] GetDoubleBiggestResults(string[,] stringbiggestResults)
        {
            double[] doubleBiggestResults = new double[4];
            double sumResults = 0;
            for (int i = 0; i < 4; i++)
                sumResults += Convert.ToDouble(stringbiggestResults[i, 1]);
            for (int i = 0; i < 4; i++)
                doubleBiggestResults[i] =
                    Math.Round(((Convert.ToDouble(stringbiggestResults[i, 1])) / sumResults), 5);
            return doubleBiggestResults;
        }

        private void CreateSegmentRecordDay(int xLabelLegend, int yLabelLegend, int index)
        {
            MyRectangle colorLegend = new MyRectangle(canvas, 10, 10, nameColor[index], xLabelLegend, yLabelLegend);
            labelLegend[index] = new MyLabel(canvas, " - ", 65, 25, 10, xLabelLegend + 8, yLabelLegend - 7, Color.FromArgb(255, 47, 79, 79),Color.FromArgb(0, 0, 0, 0),
                horizontalAlignment: HorizontalAlignment.Left);
            labels[index] = new MyLabel(canvas, "", 40, 25, 10, 0, 0, Color.FromArgb(255, 47, 79, 79), Color.FromArgb(0, 0, 0, 0), horizontalAlignment: HorizontalAlignment.Center);
        }

        private void UpdateSegment(double valueUsingAplication, double startPositionInRadian, int index,
            string nameApllication)
        {
            circleBar.SetColorsOnParts(startPositionInRadian, valueUsingAplication, nameColor[index]);
            Point l1 = GetPositionLabelUsingApllication((((2 * startPositionInRadian) + valueUsingAplication) / 2) * 6.28, -8, -8);
            labels[index].Position((int)(l1.X), (int)(l1.Y));
            labels[index].SetContent(Math.Round((valueUsingAplication * 100), 0) + " %");
            labels[index].Opacity(1);
            labelLegend[index].SetContent(nameApllication);
        }

        private Point GetPositionLabelUsingApllication(double angle, int xCenter, int yCenter)
        {
            Point returnPoint = new Point(0, 0);
            returnPoint.X = Math.Cos(angle - 1.55) * 55 + canvas.Width / 2 + xCenter - 10;
            returnPoint.Y = Math.Sin(angle - 1.55) * 55 + canvas.Height / 2 + yCenter - 4;
            return returnPoint;
        }
    }
}
