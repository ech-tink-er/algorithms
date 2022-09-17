namespace DynamicProgramming
{
    using System.Linq;

    // Minimum Edit Distance
    class MED
    {
        private string from;
        
        private string to;

        private double replaceCost;

        private double deleteCost;

        private double insertCost;

        private double?[,] cache;

        public MED(string from, string to, double replaceCost, double deleteCost, double insertCost)
        {
            this.from = from;
            this.to = to;

            this.replaceCost = replaceCost;
            this.deleteCost = deleteCost;
            this.insertCost = insertCost;

            this.cache = new double?[this.from.Length + 1, this.to.Length + 1];
        }

        /* To find the MED of a suffix of FROM to a suffix of TO we take the cost of each possible operation
         * and add to it the MED of corresponding smaller suffixes of FROM and TO, then we return the smallest cost.
         * In the case that the first chars of the 2 suffixes are equal the MED is the MED of the smaller suffixes.
         */
        public double Recursive(int f = 0, int t = 0)
        {
            if (f >= this.from.Length && t >= this.to.Length)
            {
                return 0;
            }

            if (this.cache[f, t] != null)
            {
                return (double)this.cache[f, t];
            }

            double replaceCost = double.MaxValue;
            if (f < this.from.Length && t < this.to.Length)
            {
                replaceCost = this.Recursive(f + 1, t + 1);

                if (this.from[f] != this.to[t])
                {
                    replaceCost += this.replaceCost;
                }
                else
                {
                    this.cache[f, t] = replaceCost;
                    return replaceCost;
                }
            }

            double deleteCost = double.MaxValue;
            if (f < this.from.Length)
            {
                deleteCost = this.deleteCost + this.Recursive(f + 1, t);
            }

            double insertCost = double.MaxValue;
            if (t < this.to.Length)
            {
                insertCost = this.insertCost + this.Recursive(f, t + 1);
            }

            double result = new double[] { replaceCost, deleteCost, insertCost }.Min();

            this.cache[f, t] = result;
            return result;
        }

        // Calculates the MED of each prefix of FROM to each prefix of TO.
        public double Iterative()
        {
            double[,] results = new double[this.from.Length + 1, this.to.Length + 1];
            results[0, 0] = 0;

            for (int toLen = 1; toLen < results.GetLength(1); toLen++)
            {
                results[0, toLen] = this.insertCost + results[0, toLen - 1];
            }

            for (int fromLen = 1; fromLen < results.GetLength(0); fromLen++)
            {
                results[fromLen, 0] = this.deleteCost + results[fromLen - 1, 0];
            }

            for (int fromLen = 1; fromLen < results.GetLength(0); fromLen++)
            {
                for (int toLen = 1; toLen < results.GetLength(1); toLen++)
                {
                    double replaceCost = results[fromLen - 1, toLen - 1];
                    if (this.from[fromLen - 1] == this.to[toLen - 1])
                    {
                        results[fromLen, toLen] = replaceCost;
                        continue;
                    }
                    else
                    {
                        replaceCost += this.replaceCost;
                    }

                    double deleteCost = this.deleteCost + results[fromLen - 1, toLen];
                    double insertCost = this.insertCost + results[fromLen, toLen - 1];

                    results[fromLen, toLen] = new double[] { replaceCost, deleteCost, insertCost }.Min();
                }
            }

            return results[results.GetLength(0) - 1, results.GetLength(1) - 1];
        }

        /* Calculates the MED of FROM to each prefix of TO. This is build up sequentially from the MEDs of each smaller prefix of FROM to each prefix of TO,
         * starting at the smallest prefix of FROM which is "".
        */
        public double Iterative2()
        {
            double[] toPrefixCosts = new double[this.to.Length + 1];
            toPrefixCosts[0] = 0;

            for (int toLen = 1; toLen < toPrefixCosts.Length; toLen++)
            {
                toPrefixCosts[toLen] = this.insertCost + toPrefixCosts[toLen - 1];
            }

            double[] newToPrefixCosts = new double[toPrefixCosts.Length];

            for (int fromLen = 1; fromLen <= this.from.Length; fromLen++)
            {
                newToPrefixCosts[0] = this.deleteCost * fromLen;

                for (int toLen = 1; toLen < newToPrefixCosts.Length; toLen++)
                {
                    double replaceCost = toPrefixCosts[toLen - 1];
                    if (this.from[fromLen - 1] == this.to[toLen - 1])
                    {
                        newToPrefixCosts[toLen] = replaceCost;
                        continue;
                    }
                    else
                    {
                        replaceCost += this.replaceCost;
                    }

                    double deleteCost = this.deleteCost + toPrefixCosts[toLen];
                    double insertCost = this.insertCost + newToPrefixCosts[toLen - 1];

                    newToPrefixCosts[toLen] = new double[] { replaceCost, deleteCost, insertCost }.Min();
                }

                var hold = toPrefixCosts;
                toPrefixCosts = newToPrefixCosts;
                newToPrefixCosts = hold;
            }

            return toPrefixCosts[toPrefixCosts.Length - 1];
        }
    }
}