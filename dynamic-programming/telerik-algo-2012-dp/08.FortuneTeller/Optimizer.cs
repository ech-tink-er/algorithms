namespace FortuneTeller
{
    using System;

    class Optimizer
    {
        private string prediction;

        private int totalRight;

        private int totalWrong;

        private int currentRight;

        private int currentWrong;

        public Optimizer(string prediction, int totalRight, int totalWrong)
        {
            this.Prediction = prediction;
            this.TotalRight = totalRight;
            this.TotalWrong = totalWrong;
        }

        public string Prediction
        {
            get
            {
                return this.prediction;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Predicton can't be null!");
                }

                this.prediction = value;
            }
        }

        public int TotalRight
        {
            get
            {
                return this.totalRight;
            }

            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("TotalRight can't be less than 1!");
                }

                this.totalRight = value;
            }
        }

        public int TotalWrong
        {
            get
            {
                return this.totalWrong;
            }

            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("TotalWrong can't be less than 1!");
                }

                this.totalWrong = value;
            }
        }

        private int CurrentRight
        {
            get
            {
                return this.currentRight;
            }

            set
            {
                if (value < 0 || this.TotalRight < value)
                {
                    throw new ArgumentException("CurrentRight can't be less than 0 or more than TotalRight!");
                }

                this.currentRight = value;
            }
        }

        private int CurrentWrong
        {
            get
            {
                return this.currentWrong;
            }

            set
            {
                if (value < 0 || this.TotalWrong < value)
                {
                    throw new ArgumentException("CurrentWrong can't be less than 0 or more than TotalWrong!");
                }

                this.currentWrong = value;
            }
        }

        private int GoodGuesses { get; set; }

        public int GetBestPrediction()
        {
            this.PreSet();

            for (int i = 0; i < this.prediction.Length; i++)
            {
                if (i < this.prediction.Length - 2 &&
                    (this.prediction[i] == this.prediction[i + 1] && this.prediction[i] != this.prediction[i + 2]))
                {
                    if (this.prediction[i] == Program.GoodDay && this.CurrentRight == 1)
                    {
                        this.MakeWrong(i);
                        continue;
                    }

                    if (this.prediction[i] == Program.BadDay && this.CurrentWrong == 1)
                    {
                        this.MakeRight(i);
                        continue;
                    }
                }

                this.MakeBest(i);
            }

            return this.GoodGuesses;
        }

        private void MakeRight(int i)
        {
            if (this.CurrentRight > 0)
            {
                this.CurrentRight--;

                this.CurrentWrong = this.TotalWrong;

                if (this.prediction[i] == Program.GoodDay)
                {
                    this.GoodGuesses++;
                }
            }
            else
            {
                this.MakeWrong(i);
            }
        }

        private void MakeWrong(int i)
        {
            if (this.CurrentWrong > 0)
            {
                this.CurrentWrong--;

                this.CurrentRight = this.TotalRight;

                if (this.prediction[i] == Program.BadDay)
                {
                    this.GoodGuesses++;
                }
            }
            else
            {
                this.MakeRight(i);
            }
        }

        private void MakeBest(int i)
        {
            if (this.prediction[i] == Program.GoodDay)
            {
                this.MakeRight(i);
            }
            else
            {
                this.MakeWrong(i);
            }
        }

        private void PreSet()
        {
            this.CurrentRight = this.TotalRight;
            this.CurrentWrong = this.TotalWrong;
            this.GoodGuesses = 0;
        }
    }
}