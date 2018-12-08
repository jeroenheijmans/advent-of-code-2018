using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public static class Util
    {
        // https://stackoverflow.com/a/16193323/419956 by @AdamHouldsWorth
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : new()
        {
            return dict.GetOrCreate(key, new TValue());
        }

        // https://stackoverflow.com/a/16193323/419956 by @AdamHouldsWorth
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultIfNull)
        {
            TValue val;

            if (!dict.TryGetValue(key, out val))
            {
                val = defaultIfNull;
                dict.Add(key, val);
            }

            return val;
        }

        public static Queue<T> ToQueue<T>(this IEnumerable<T> input) => new Queue<T>(input);
        public static Stack<T> ToStack<T>(this IEnumerable<T> input) => new Stack<T>(input);

        public static IEnumerable<int> AsRangeFromZero(this int input) 
            => Enumerable.Range(0, input);

        public static T[] NewItemsOfType<T>(this int count) 
            where T : new() 
            => count.AsRangeFromZero().Select(_ => new T()).ToArray();

        public static (int minX, int maxX, int minY, int maxY) GetDimensions(this IEnumerable<Point> data)
        {
            return (
                data.Select(p => p.X).Min(),
                data.Select(p => p.X).Max(),
                data.Select(p => p.Y).Min(),
                data.Select(p => p.Y).Max()
            );
        }

        public static int GetManhattanDistance(Point p1, Point p2)
        {
            return GetManhattanDistance(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static int GetManhattanDistance(Point p, int x, int y)
        {
            return GetManhattanDistance(p.X, p.Y, x, y);
        }

        public static int GetManhattanDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }
}
