namespace Recursion
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public static class Program
    {
        public static int Count = 0;

        public static void Main()
        {
            PermutationsWithRepetiton(2, 3);
            Console.WriteLine("-----------");
            CombinationsWithRepetiton(2, 3);
            Console.WriteLine("-----------");
            CombinationsWithoutRepetiton(2, 4);
            Console.WriteLine("-----------");
            PermutationsWithRepetiton(2, new[] { "hi", "a", "b" });
            Console.WriteLine("-----------");
            CombinationsWithoutRepetiton(2, new[] { "test", "rock", "fun" });
            Console.WriteLine("-----------");

            var stuff = new SortedDictionary<int, int>()
            {
                {1, 1 },
                {2, 1 },
                {3, 1 },
                {4, 1 },
            };

            PrintOrders2(stuff, new List<int>(), stuff.Sum(pair => pair.Value));
            Console.WriteLine($"Count: {Count}");


            Console.WriteLine("-----------");
            Console.WriteLine("Print oRders 3:");
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.WriteLine(numbers.Length);
            int count = PrintOrders3(numbers);
            Console.WriteLine(count);
            //NextPermutation(numbers);

            //Print(numbers);

            //PrintOrders3(new int [] {1 ,2 , 3, 4 });
            Console.WriteLine("-----------");

            Console.WriteLine(IsTherePath(new bool[100, 100], new[] { 0, 0}, new[] { 99, 99 }));
            Console.WriteLine("-----------");

        }

        public static void PermutationsWithRepetiton(int length, int numbers, int[] data = null)
        {
            if (data == null)
            {
                data = new int[length];
            }

            if (length == 0)
            {
                Console.WriteLine(string.Join(" ", data));
                return;
            }

            for (int i = 1; i <= numbers; i++)
            {
                data[data.Length - length] = i;
                PermutationsWithRepetiton(length - 1, numbers, data);
            }
        }

        public static void CombinationsWithRepetiton(int length, int numbers, int[] data = null)
        {
            if (data == null)
            {
                data = new int[length];
            }

            if (length == 0)
            {
                Console.WriteLine(string.Join(" ", data));
                return;
            }

            int start = length == data.Length ? 1 : data[data.Length - length - 1];
            for (int i = start; i <= numbers; i++)
            {
                data[data.Length - length] = i;
                CombinationsWithRepetiton(length - 1, numbers, data);

            }
        }

        public static void CombinationsWithoutRepetiton(int length, int numbers, int[] data = null)
        {
            if (data == null)
            {
                data = new int[length];
            }

            if (length == 0)
            {
                Console.WriteLine(string.Join(" ", data));
                return;
            }

            int start = length == data.Length ? 1 : data[data.Length - length - 1] + 1;
            for (int i = start; i <= numbers; i++)
            {
                data[data.Length - length] = i;
                CombinationsWithoutRepetiton(length - 1, numbers, data);

            }
        }

        public static void PermutationsWithRepetiton<T>(int length, T[] objects, T[] data = null)
        {
            if (data == null)
            {
                data = new T[length];
            }

            if (length == 0)
            {
                Console.WriteLine(string.Join(" ", data));
                return;
            }

            for (int i = 0; i < objects.Length; i++)
            {
                data[data.Length - length] = objects[i];
                PermutationsWithRepetiton(length - 1, objects, data);
            }
        }

        public static void CombinationsWithoutRepetiton<T>(int length, T[] objects, int[] data = null)
        {
            if (data == null)
            {
                data = new int[length];
            }

            if (length == 0)
            {
                Console.WriteLine(string.Join(" ", data.Select(index => objects[index])));
                return;
            }

            int start = length == data.Length ? 0 : data[data.Length - length - 1] + 1;
            for (int i = start; i < objects.Length; i++)
            {
                data[data.Length - length] = i;
                CombinationsWithoutRepetiton(length - 1, objects, data);

            }
        }

        public static void PermutationsWithoutRepetition<T>(int length, T[] objects, int[] data = null)
        {
            if (data == null)
            {
                data = new int[length].Select(n => -1).ToArray();
            }

            if (length == 0)
            {
                Console.WriteLine(string.Join(" ", data.Select(index => objects[index])));
                return;
            }

            for (int i = 0; i < objects.Length; i++)
            {
                if (data.Contains(i))
                {
                    continue;
                }

                data[data.Length - length] = i;
                PermutationsWithoutRepetition(length - 1, objects, data);

                data[data.Length - length] = -1;

            }
        }

        public static bool NextPermutation<T>(T[] objects) where T : IComparable<T>
        {
            int descentStart = objects.Length - 1;
            for (; descentStart >= 1; descentStart--)
            {
                if  (objects[descentStart - 1].CompareTo(objects[descentStart]) < 0)
                {
                    break;
                }
            }

            if (descentStart == 0)
            {
                return false;
            }

            int next = objects.Length - 1;
            for (; next >= descentStart; next--)
            {
                if (objects[descentStart - 1].CompareTo(objects[next]) == -1)
                {
                    break;
                }
            }

            objects.Swap(descentStart - 1, next);

            objects.Reverse(descentStart);

            return true;
        }

        public static int PrintOrders3<T>(T[] objects) where T : IComparable<T>
        {
            int count = 0;

            objects = objects.OrderBy(obj => obj)
                .ToArray();

            do
            {
                Console.WriteLine(string.Join(" ", objects.Select(obj => obj.ToString())));
                count++;
            } while (NextPermutation(objects));

            return count;
        }

        public static void PrintOrders2<T>(IDictionary<T, int> objects, List<T> permutation, int total)
        {
            if (permutation.Count == total)
            {
                Count++;
                Console.WriteLine(string.Join(" ", permutation));
            }

            foreach (var key in objects.Keys.ToArray())
            {
                if (objects[key] == 0)
                {
                    continue;
                }

                permutation.Add(key);
                objects[key]--;

                PrintOrders2(objects, permutation, total);
                objects[key]++;

                permutation.RemoveAt(permutation.Count - 1);
            }
        }

        public static void PrintOrders<T>(T[] objects, int length, ref int fail)
        {
            if (length < 2)
            {
                string str = string.Join(" ", objects);
                Console.WriteLine(str);
                return;
            }

            PrintOrders(objects, length - 1, ref fail);

            for (int i = 0, lookback = 0; i < length - 1; i++)
            {
                if (!objects[length - 1].Equals(objects[length - 2 - lookback]))
                {
                    objects.Swap(length - 1, length - 2 - lookback);
                    PrintOrders(objects, length - 1, ref fail);

                    if (fail > 0)
                    {
                        fail = 0;
                        i += fail;
                    }
                }
                else
                {
                    lookback++;
                }

                if ((length - 2 - lookback) < 0)
                {
                    fail++;
                    return;
                }
            }
        }

        public static void Reverse<T>(this T[] arr, int start = 0, int length = -1)
        {
            if (start < 0 || arr.Length <= start)
            {
                throw new IndexOutOfRangeException("Start must be in array range.");
            }

            int end = start + length - 1;

            if (length < 0 && arr.Length <= end)
            {
                throw new IndexOutOfRangeException("Length is out of array range.");
            }
            else if (length < 0)
            {
                length = arr.Length - start;
            }


            for (int i = 0; i < length / 2; i++)
            {
                int left = start + i;
                int right = (start + length - 1) - i;

                arr.Swap(left, right);
            }
        }

        public static void Swap<T>(this T[] arr, int first, int second)
        {
            T hold = arr[first];
            arr[first] = arr[second];
            arr[second] = hold;
        }

        public static bool IsTherePath(bool[,] matrix, int[] from, int[] to)
        {
            if (!matrix.IsValidPosition(from) || !matrix.IsValidPosition(to) || matrix[from[0], from[1]])
            {
                throw new ArgumentException("From and to must be valid positions.");
            }

            if (from[0] == to[0] && from[1] == to[1])
            {
                return true;
            }

            //Console.WriteLine($"[{from[0]}, {from[1]}]");
            //Console.ReadKey(true);

            matrix[from[0], from[1]] = true;

            var neighbors = new int[][]
            {
                new [] { from[0] + 1, from[1] },
                new [] { from[0] - 1, from[1] },
                new [] { from[0], from[1] + 1 },
                new [] { from[0], from[1] - 1 },
            }.Where(position => matrix.IsValidPosition(position) && !matrix[position[0], position[1]])
            .OrderBy(position => Math.Abs(position[0] - to[0]) + Math.Abs(position[1] - to[1]));

            foreach (var neighbor in neighbors)
            {
                if (IsTherePath(matrix, neighbor, to))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsValidPosition<T>(this T[,] matrix, int[] position)
        {
            return 0 <= position[0] && position[0] < matrix.GetLength(0) &&
                0 <= position[1] && position[1] < matrix.GetLength(1);
        }

        public static void Print<T>(IEnumerable<T> items)
        {
            Console.WriteLine(string.Join(" ", items));
        }
    }
}