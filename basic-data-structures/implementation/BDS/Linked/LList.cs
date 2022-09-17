namespace BDS
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class LList<T> : IEnumerable<T>
    {
        public LList()
        {
            this.Count = 0;
        }

        public LList(IEnumerable<T> items)
            : this()
        {
            foreach (var item in items)
            {
                this.AddLast(item);
            }
        }

        public ListNode<T> First { get; private set; }

        public ListNode<T> Last { get; private set; }

        public int Count { get; private set; }

        public ListNode<T> AddLast(T value)
        {
            var node = this.MakeFirst(value);
            if (node != null)
            {
                return node;
            }

            return this.AddAfter(value, this.Last);
        }

        public ListNode<T> AddFirst(T value)
        {
            var node = this.MakeFirst(value);
            if (node != null)
            {
                return node;
            }

            return this.AddBefore(value, this.First);
        }

        public ListNode<T> AddAfter(T value, ListNode<T> node)
        {
            this.ValidateNode(node);

            var @new =  new ListNode<T>(value, list: this, next: node.Next, previous: node);
            this.Count++;

            if (node == this.Last)
            {
                this.Last = @new;
            }

            return @new;
        }

        public ListNode<T> AddBefore(T value, ListNode<T> node)
        {
            this.ValidateNode(node);

            var @new = new ListNode<T>(value, list: this, next: node, previous: node.Previous);
            this.Count++;

            if (node == this.First)
            {
                this.First = @new;
            }

            return @new;
        }

        public void Remove(ListNode<T> node)
        {
            this.ValidateNode(node);

            node.List = null;
            this.Count--;

            if (this.Count == 0)
            {
                this.First = null;
                this.Last = null;

                return;
            }

            if (node == this.Last)
            {
                this.Last = node.Previous;
                this.Last.Next = null;

                return;
            }

            if (node == this.First)
            {
                this.First = node.Next;
                this.First.Previous = null;

                return;
            }

            node.Previous.Next = node.Next;
        }

        public bool Remove(T value)
        {
            var node = this.Find(value);
            if (node == null)
            {
                return false;
            }

            this.Remove(node);

            return true;
        }

        public bool RemoveAll(T value)
        {
            var nodes = this.FindAll(value);
            if (!nodes.Any())
            {
                return false;
            }

            foreach (var node in nodes)
            {
                this.Remove(node);
            }

            return true;
        }

        public void Clear()
        {
            while (this.Last != null)
            {
                this.Remove(this.Last);
            }
        }

        public ListNode<T> Find(T value, ListNode<T> start = null, bool findLast = false)
        {
            if (start != null)
            {
                this.ValidateNode(start);
            }
            else
            {
                start = findLast ? this.Last : this.First;
            }

            for (var current = start; current != null; current = findLast ? current.Previous : current.Next)
            {
                if (current.Value.Equals(value))
                {
                    return current;
                }
            }

            return null;
        }

        public List<ListNode<T>> FindAll(T value)
        {
            var nodes = new List<ListNode<T>>();

            var start = this.First;

            while (start != null)
            {
                var node = this.Find(value, start);
                if (node == null)
                {
                    return nodes;
                }


                nodes.Add(node);

                start = node.Next;
            }

            return nodes;
        }

        public bool Contains(T value)
        {
            return this.Find(value) != null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = this.First;
            while (node != null)
            {
                yield return node.Value;

                node = node.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private ListNode<T> MakeFirst(T value)
        {
            if (this.First == null && this.Last == null)
            {
                var node = new ListNode<T>(value, this);

                this.First = node;
                this.Last = node;

                this.Count = 1;

                return node;
            }

            return null;
        }

        private void ValidateNode(ListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("Node can't be null!");
            }

            if (node.List != this)
            {
                throw new ArgumentException("Node must be part of this list!");
            }
        }
    }
}