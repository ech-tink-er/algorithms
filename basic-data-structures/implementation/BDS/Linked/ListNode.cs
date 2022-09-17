namespace BDS
{
    using System;

    public sealed class ListNode<T>
    {
        private ListNode<T> next;

        private ListNode<T> previous;

        public ListNode(T value, LList<T> list)
        {
            this.Value = value;
            this.List = list;
        }

        public ListNode(T value, LList<T> list, ListNode<T> next)
            : this(value, list)
        {
            this.Next = next;
        }

        public ListNode(T value, LList<T> list, ListNode<T> next, ListNode<T> previous)
            : this(value, list, next)
        {
            this.Previous = previous;
        }

        public T Value { get; set; }

        public ListNode<T> Next
        {
            get
            {
                return this.next;
            }

            internal set
            {
                if (value == null)
                {
                    this.next = null;
                    return;
                }

                if (value.List != this.List)
                {
                    throw new ArgumentException("Both nodes must be of the same list!");
                }

                this.next = value;

                this.next.previous = this;
            }
        }

        public ListNode<T> Previous 
        {
            get
            {
                return this.previous;
            }

            internal set
            {
                if (value == null)
                {
                    this.previous = null;
                    return;
                }

                if (value.List != this.List)
                {
                    throw new ArgumentException("Both nodes must be of the same list!");
                }

                this.previous = value;

                this.previous.next = this;
            }
        }

        public LList<T> List { get; internal set; }
    }
}