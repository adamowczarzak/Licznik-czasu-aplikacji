using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ApplicationTimeCounter
{
    class MembershipInCategory
    {
        private MyLabel nameElementCategory;
        private MyLabel nameCategory;

        public MembershipInCategory(Canvas canvas)
        {
            MyCircle elementCategory = new MyCircle(canvas, 70, 0, Color.FromArgb(255, 30, 144, 255), 30, 80, setFill: true);
            MyCircle category = new MyCircle(canvas, 100, 0, Color.FromArgb(255, 50, 205, 50), 70, 30, setFill: true);
            MyLabel title = new MyLabel(canvas, "Przynależność", 120, 30, 14, 0, 0, Color.FromArgb(255, 47, 79, 79));
            nameElementCategory = new MyLabel(canvas, "Facebook", 70, 25, 11, 25, 102, Color.FromArgb(255, 47, 79, 79));
            nameCategory = new MyLabel(canvas, "FireFox", 100, 30, 14, 70, 65, Color.FromArgb(255, 47, 79, 79));
        }

        public void Update(string title)
        {

        }
    }
}
