using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter
{
    class ActiveApplication
    {

        public ActiveApplication()
        {
            ID = 0;
            Title = string.Empty;
            ActivityTime = 0;
            Date = string.Empty;
            IdNameActivity = -3;
        }
        public int ID { get; set; }
        public string Title { get; set; }
        public int ActivityTime { get; set; }
        public string Date { get; set; }
        public int IdNameActivity { get; set; }
    }
}
