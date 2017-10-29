using System;

namespace ApplicationTimeCounter
{
    class Counter
    {

        private DailyUseOfApplication_db dailyUseOfApplication_db;
        private DateTime dateLastRestet;
        
        

        public Counter()
        {
            dateLastRestet = new DateTime();
            dateLastRestet = DateTime.Now;
            dailyUseOfApplication_db = new DailyUseOfApplication_db();
        }

        public void Update()
        {
            dailyUseOfApplication_db.Update();       
        }

        public void Reset()
        {
            dailyUseOfApplication_db.AddTimeToDayDisableComputer();
            dailyUseOfApplication_db.TransferDataToAllDataAndClearTable();
            dateLastRestet = DateTime.Now.Date;
            dailyUseOfApplication_db.AddTimeToNowDisableComputer();
            
        }

        public void UpdateTimeNonActive()
        {
            dailyUseOfApplication_db.UpdateTimeNonActiv();
        }

        public void UpdateTimeDisableCompurte()
        {
            dailyUseOfApplication_db.AddTimeToNowDisableComputer();
        }
    }
}
