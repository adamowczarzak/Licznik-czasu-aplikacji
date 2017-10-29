using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ApplicationTimeCounter.Controls;

namespace ApplicationTimeCounter
{
    class HomeForm
    {
        private Canvas MainCanvasHome;
        private Canvas[] canvases;
        private Canvas contentPage;      
        private ViewContent viewContent;
        private BarUsingApplication barUsingApplication;
        private TimeUsingApplication timeUsingApplication;
        private MembershipInCategory membershipInCategory;
        private BiggestResultsOfDay biggestResultsOfDay;
        private SpendingTime spendingTime;
        private AnimatedClock animatedClock;
        private MyLabel titleAplication;


        public HomeForm(Canvas contentPage, ref ViewContent viewContent)
        {            
            this.viewContent = viewContent;
            this.contentPage = contentPage;
            canvases = new Canvas[6];

            CreatePanelCanvas();
            barUsingApplication = new BarUsingApplication(canvases[0]);
            timeUsingApplication = new TimeUsingApplication(canvases[1]);
            membershipInCategory = new MembershipInCategory(canvases[2]);
            biggestResultsOfDay = new BiggestResultsOfDay(canvases[3]);
            spendingTime = new SpendingTime(canvases[4]);
            animatedClock = new AnimatedClock(canvases[5]);

            ShowHomeForm();

        }

        public void ShowHomeForm()
        {
            viewContent.ChangeContent(MainCanvasHome);
        }

        public void UpdateView()
        {         
            barUsingApplication.Update(titleAplication.GetContent());
            timeUsingApplication.Update(titleAplication.GetContent());
            membershipInCategory.Update(titleAplication.GetContent());
            biggestResultsOfDay.Update();
            spendingTime.Update();
            animatedClock.Update();
        }

        public void UpdateTitleApllication()
        {
            ActiveWindow activeWindow = new ActiveWindow();
            string title = activeWindow.GetNameActiveWindow();
            if (title != "Brak aktywnego okna") titleAplication.SetContent(title);
        }

        private void CreatePanelCanvas()
        {
            int startX = 15;
            int startY = 65;

            MainCanvasHome = CanvasCreator.CreateCanvas(contentPage, 620, 410, Color.FromArgb(255, 30, 39, 93), 0, 0);
            Canvas upperBar = CanvasCreator.CreateCanvas(MainCanvasHome, 590, 40, Color.FromArgb(200, 255, 255, 255), 15, 15);
            titleAplication = new MyLabel(upperBar, "-",
                590, 40, 18, 0, 2, Color.FromArgb(255, 47, 79, 79) , Color.FromArgb(0, 0, 0, 0), horizontalAlignment: HorizontalAlignment.Left);
                

            for(int i = 0; i < 6; i++)
            {
                canvases[i] = CanvasCreator.CreateCanvas(MainCanvasHome, 190, 160, Color.FromArgb(200, 255, 255, 255), startX, startY);
                startX += Convert.ToInt32(canvases[i].Width) + 10;
                if (i == 2)
                {
                    startY += Convert.ToInt32(canvases[i].Height) + 10;
                    startX = 15;
                }
            }
        }
    }
}
