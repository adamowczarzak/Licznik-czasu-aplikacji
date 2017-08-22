using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ApplicationTimeCounter
{
    class SpendingTime
    {
        private string[] namesBarsGraph;
        private MyRectangle[] barsGraph;
        private MyLabel[] mylabels;
        private DailyUseOfApplication_db dailyUseOfApplication_db;


        public SpendingTime(Canvas canvas)
        {
            barsGraph = new MyRectangle[4];
            mylabels = new MyLabel[4];
            namesBarsGraph = new string[4];
            namesBarsGraph[0] = "Wył. komputer";
            namesBarsGraph[1] = "Brak Aktyw.";
            namesBarsGraph[2] = "Programowanie";
            namesBarsGraph[3] = "Inne";

            MyLabel title = new MyLabel(canvas, "Dzienna aktywność", 140, 30, 14, 0, 0);

            CreateSegmentSpendingTime(canvas, "Lavender", 0, 25, 70, 2, 131);
            CreateSegmentSpendingTime(canvas, "DarkSlateBlue", 1, 65, 70, 102, 131);
            CreateSegmentSpendingTime(canvas, "ForestGreen", 2, 105, 70, 2, 146);
            CreateSegmentSpendingTime(canvas, "Goldenrod", 3, 145, 70, 102, 146);

            dailyUseOfApplication_db = new DailyUseOfApplication_db();
            Update();
        }

        public void Update()
        {
            int[] tableTime = new int[4];
            for (int i = 0; i < 4; i++) tableTime[i] = 0;
            tableTime[0] = dailyUseOfApplication_db.GetTimeForTitle("Wyl. komputer");
            tableTime[1] = dailyUseOfApplication_db.GetTimeForTitle("Brak Aktyw.");
            tableTime[2] = dailyUseOfApplication_db.GetTimeForNumberActivity(1);
            tableTime[3] = dailyUseOfApplication_db.GetTimeForNumberActivity(2);

            int sum = 0;
            for (int i = 0; i < 4; i++) sum += tableTime[i];
            for (int i = 0; i < 4; i++)
            {
                if(sum > 0)tableTime[i] = Convert.ToInt32((Convert.ToDouble(tableTime[i]) / Convert.ToDouble(sum)) * 100);
                UpdateSegment(i, tableTime[i]);
            }
           
        }

        private void CreateSegmentSpendingTime(Canvas canvas , string nameColor, int indexNameBarsGraph,
            int xElement, int yElement, int xLabelLegend, int yLabelLegend)
        {
            mylabels[indexNameBarsGraph] = new MyLabel(canvas, "0 %", 40, 25, 10, xElement - 8, yElement + 30);
            MyRectangle colorLegend = new MyRectangle(canvas, 10, 10, nameColor, xLabelLegend, yLabelLegend);
            MyLabel labelLegend = new MyLabel(canvas, namesBarsGraph[indexNameBarsGraph], 80, 25, 10, xLabelLegend + 8, yLabelLegend - 7,
                horizontalAlignment: HorizontalAlignment.Left);
            barsGraph[indexNameBarsGraph] = new MyRectangle(canvas, 0, 20, nameColor, xElement, 170 - yElement);
        }

        private void UpdateSegment(int index, int value)
        {
            mylabels[index].SetContent(value + " %");
            value = Convert.ToInt32((Convert.ToDouble(value) * 0.85));
            barsGraph[index].Resize(value, 20);
            barsGraph[index].Position(y: 120 - value);
            mylabels[index].Position(y: 100 - value);
            
        }
    }
}
