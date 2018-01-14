using System.Windows.Controls;
using System.Windows.Media;
using ApplicationTimeCounter.Controls;
using ApplicationTimeCounter.Other;
using System;

namespace ApplicationTimeCounter
{
    class MembershipInCategory
    {
        private MyLabel nameGroup;
        private MyLabel nameActivity;

        public MembershipInCategory(Canvas canvas)
        {
            MyCircle elementCategory = new MyCircle(canvas, 70, 0, Color.FromArgb(255, 30, 144, 255), 30, 80, setFill: true);
            MyCircle category = new MyCircle(canvas, 100, 0, Color.FromArgb(255, 50, 205, 50), 70, 30, setFill: true);
            MyLabel title = new MyLabel(canvas, "Przynależność", 120, 30, 14, 0, 0, Color.FromArgb(255, 47, 79, 79), Color.FromArgb(0, 0, 0, 0));
            nameGroup = new MyLabel(canvas, "-", 90, 25, 10, 20, 102, Color.FromArgb(255, 47, 79, 79), Color.FromArgb(0, 0, 0, 0));
            nameActivity = new MyLabel(canvas, "-", 120, 30, 12, 60, 65, Color.FromArgb(255, 47, 79, 79), Color.FromArgb(0, 0, 0, 0));
        }

        public void Update(string title)
        {
            if (!string.IsNullOrEmpty(title) && !string.Equals(title, "-"))
            {
                int idTitle = Convert.ToInt32(ActiveApplication_db.GetIdActivityByName(SqlValidator.Validate(title)));
                nameGroup.SetContent(ActiveApplication_db.GetNameGroupByIdTitle(idTitle));
                nameActivity.SetContent(ActiveApplication_db.GetNameActivityByIdTitle(idTitle));
            }
        }
    }
}
