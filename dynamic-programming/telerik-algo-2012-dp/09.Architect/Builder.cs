namespace Architect
{
    using System.Linq;

    class Builder
    {
        private const int BlockSides = 3;

        private int[][] blocks;

        private bool[] used;

        private int?[,,] cache;

        public Builder(int[][] blocks)
        {
            this.blocks = blocks.Select(RemoveDuplicateSides).ToArray();
            this.used = new bool[this.blocks.Length];

            this.cache = new int?[this.blocks.Length, Builder.BlockSides, Utils.PowOf2(this.blocks.Length)];
        }

        private static int[] RemoveDuplicateSides(int[] block)
        {
            for (int i = 0; i < block.Length; i++)
            {
                int next = (i + 1) % block.Length;
                if (block[i] == block[next])
                {
                    int last = (next + 1) % block.Length;

                    if (block[i] == block[last])
                    {
                        return new int[] { block[i] };
                    }

                    return new int[] { block[last], block[i] };
                }
            }

            return block;
        }

        public int FindTallest()
        {
            int max = 0;
            for (int b = 0; b < this.blocks.Length; b++)
            {
                for (int s = 0; s < this.blocks[b].Length; s++)
                {
                    int used = Utils.SetBit(0, b, true);

                    int height = this.ReadDimension(b, s, 2) + this.FindTallest(b, s, used);

                    if (max < height)
                    {
                        max = height;
                    }
                }
            }

            return max;
        }
            
        public int FindTallest(int lastBlock, int lastBlockSide, int used)
        {
            if (this.cache[lastBlock, lastBlockSide, used] != null)
            {
                return (int)this.cache[lastBlock, lastBlockSide, used];
            }

            int width = this.ReadDimension(lastBlock, lastBlockSide, 0);
            int depth = this.ReadDimension(lastBlock, lastBlockSide, 1);

            int max = 0;
            for (int b = 0; b < this.blocks.Length; b++)
            {
                if (Utils.GetBit(used, b))
                {
                    continue;
                }

                for (int s = 0; s < this.blocks[b].Length; s++)
                {
                    if ((width < this.ReadDimension(b, s, 0) || depth < this.ReadDimension(b, s, 1)) &&
                        (width < this.ReadDimension(b, s, 1) || depth < this.ReadDimension(b, s, 0)))
                    {
                        continue;
                    }

                    used = Utils.SetBit(used, b, true);

                    int height = this.ReadDimension(b, s, 2) + FindTallest(b, s, used);

                    used = Utils.SetBit(used, b, false);

                    if (max < height)
                    {
                        max = height;
                    }
                }
            }

            this.cache[lastBlock, lastBlockSide, used] = max;
            return max;
        }

        private int ReadDimension(int block, int side, int dimension)
        {
            int index = (side + dimension) % Builder.BlockSides;

            index = index < this.blocks[block].Length ? index : this.blocks[block].Length - 1; 

            return this.blocks[block][index];
        }
    }
}