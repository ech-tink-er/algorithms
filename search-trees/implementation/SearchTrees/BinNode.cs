namespace SearchTrees
{
    using System;

    public class BinNode<T>
        where T : IComparable<T>
    {
        private BinNode<T> left;

        private BinNode<T> right;

        public BinNode(T value = default(T), int data = 0)
        {
            this.Value = value;
            this.Location = 0;

            this.Data = data;
        }

        public T Value { get; set; }

        public BinNode<T> Parent { get; private set; }

        public BinNode<T> Left
        {
            get
            {
                return this.left;
            }

            set
            {
                if (this.left != null)
                {
                    this.left.Parent = null;
                }

                this.left = value;

                if (this.left != null)
                {
                    this.left.Disconnect();

                    this.left.Parent = this;

                    this.left.Location = -1;
                }
            }
        }

        public BinNode<T> Right
        {
            get
            {
                return this.right;
            }

            set
            {
                if (this.right != null)
                {
                    this.right.Parent = null;
                }

                this.right = value;

                if (this.right != null)
                {
                    this.right.Disconnect();

                    this.right.Parent = this;

                    this.right.Location = 1;
                }
            }
        }

        public int Location { get; private set; }

        public int Data { get; set; }

        public BinNode<T> GetChild(int location)
        {
            if (location < 0)
            {
                return this.Left;
            }
            else if (0 < location)
            {
                return this.Right;
            }
            else
            {
                return this;
            }
        }

        public BinNode<T> SetChild(int location, BinNode<T> node)
        {
            BinNode<T> old = this;
            if (location < 0)
            {
                old = this.Left;
                this.Left = node;
            }
            else if (0 < location)
            {
                old = this.Right;
                this.Right = node;
            }

            return old;
        }

        public int Disconnect()
        {
            if (this.Parent == null)
            {
                return 0;
            }

            this.Location = 0;
            if (this.Parent.Left == this)
            {
                this.Parent.Left = null;
                return -1;
            }
            else
            {
                this.Parent.Right = null;
                return 1;
            }
        }

        public override string ToString()
        {
            return this.ToString(verbose: false);
        }

        public string ToString(bool verbose, Func<int, string> formatData = null)
        {
            string value = this.Value.ToString();

            if (!verbose)
            {
                return value;
            }

            string data;
            if (formatData == null)
            {
                data = this.Data.ToString();
            }
            else
            {
                data = formatData(this.Data);
            }

            return value + "|" + data;
        }
    }
}