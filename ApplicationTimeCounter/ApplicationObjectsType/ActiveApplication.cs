namespace ApplicationTimeCounter
{
    class ActiveApplication
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int ActivityTime { get; set; }
        public string Date { get; set; }
        public string NameActivity { get; set; }
        public IdNameActivityEnum IdNameActivity { get; set; }

        public ActiveApplication()
        {
            ID = 0;
            Title = string.Empty;
            ActivityTime = 0;
            Date = string.Empty;
            NameActivity = string.Empty;
            IdNameActivity = IdNameActivityEnum.NonActive;
        }

        public enum IdNameActivityEnum : int
        {
            NonActive = 0,
            Lack = 1,
            Programming = 2,
            Other = 3
        }

    }
}
