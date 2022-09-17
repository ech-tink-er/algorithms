namespace SecretLanguage
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    class Decoder
    {
        private static readonly int? Empty = -1; 

        private string message;

        private Dictionary<string, List<string>> words;

        private int[] lengths;

        private int?[] cache;

        public Decoder(string message, string[] words)
        {
            this.message = message;
            this.Load(words);

            this.cache = new int?[this.message.Length]
                .Select(v => Decoder.Empty)
                .ToArray();
        }       

        private static int GetCost(string original, string permutation)
        {
            if (original.Length != permutation.Length)
            {
                throw new ArgumentException("Original and permutations must be of equal length!");
            }

            int cost = 0;
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] != permutation[i])
                {
                    cost++;
                }
            }

            return cost;
        }

        private static string GetKey(string word)
        {
            return new string(word.OrderBy(c => c).ToArray());
        }

        public int? Decode(int index = 0)
        {
            if (this.cache[index] != Decoder.Empty)
            {
                return this.cache[index];
            }

            int lengthLeft = this.message.Length - index;

            int? min = null;
            foreach (var length in this.lengths.Where(length => length <= lengthLeft))
            {
                string partial = this.message.Substring(index, length);
                string key = new string(partial.OrderBy(c => c).ToArray());

                int? cost = null;
                if (this.words.ContainsKey(key))
                {
                    if (lengthLeft - length == 0)
                    {
                        cost = 0;
                    }
                    else
                    {
                        cost = this.Decode(index + length);
                    }

                    if (cost != null)
                    {
                        cost += this.words[key].Select(word => GetCost(partial, word)).Min();
                    }
                }

                if (cost != null && (min == null || cost < min))
                {
                    min = cost;
                }
            }

            this.cache[index] = min;
            return min;
        }

        private void Load(string[] words)
        {
            var viable = words.Where(word => word.Length <= this.message.Length);

            this.lengths = new SortedSet<int>(viable.Select(word => word.Length))
                .ToArray();

            this.words = new Dictionary<string, List<string>>();

            foreach (var word in words)
            {
                string key = GetKey(word);

                if (!this.words.ContainsKey(key))
                {
                    this.words[key] = new List<string>();
                }

                this.words[key].Add(word);
            }
        }
    }
}