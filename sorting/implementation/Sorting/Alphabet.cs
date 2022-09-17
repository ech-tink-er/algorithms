namespace Sorting
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using Utilities;

    public class Alphabet
    {
        public static readonly Alphabet Decimal = new Alphabet("0123456789");
        public static readonly Alphabet Uppercase = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        public static readonly Alphabet Lowercase = new Alphabet("abcdefghijklmnopqrstuvwxyz");
        public static readonly Alphabet ASCII = new Alphabet(ASCIICharacters());

        private static string ASCIICharacters()
        {
            var characters = new StringBuilder(128);

            for (char c = (char)0; c < 128; c++)
                characters.Append(c);

            return characters.ToString();
        }

        private readonly Dictionary<char, int> values;

        public Alphabet(string characters)
        {
            this.Characters = characters;

            this.values = new Dictionary<char, int>();
            this.LoadValues();
        }

        public string Characters { get; }

        public int Size
        {
            get
            {
                return this.Characters.Length;
            }
        }

        public string Symbol(int value)
        {
            if (value < 0)
            {
                string symbol = "$";
                if (value < -1)
                    symbol += System.Math.Abs(value);
                return symbol;
            }

            return this.Characters[value].ToString();
        }

        public int Value(char c)
        {
            return this.values[c];
        }

        public string String(IList<int> values)
        {
            return string.Join("", values.Select(v => this.Symbol(v)));
        }

        public int[] Values(string str)
        {
            return str.Select((c, i) => this.Value(c))
                .ToArray();
        }

        public bool Contains(char c)
        {
            return this.values.ContainsKey(c);
        }

        public int Compare(string first, string second, int f = 0, int s = 0, int length = -1)
        {
            return Misc.Compare(this.Values(first), this.Values(second), f, s, length);
        }

        public int Compare(string first, string second, out int matched, int f = 0, int s = 0, int length = -1)
        {
            return Misc.Compare(this.Values(first), this.Values(second), out matched, f, s, length);
        }

        private void LoadValues()
        {
            for (int i = 0; i < this.Characters.Length; i++)
                this.values[this.Characters[i]] = i;
        }
    }
}