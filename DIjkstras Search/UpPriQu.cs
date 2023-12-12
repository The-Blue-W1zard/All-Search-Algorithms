using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIjkstras_Search
{
    //Updateable Priority Queue
    public class UpPriQu
    {
        private Dictionary<Point,int> sortedDict;
        private int numItems;
        private int capacity;
        private Point None = new Point(-1,-1);

        public int Count { get { return numItems; } }
        
        public UpPriQu():this (capacity: 1500) { }
        public UpPriQu(int capacity)
        {
            sortedDict = new Dictionary<Point, int>(capacity);
            this.capacity = capacity;
            numItems = 0;
        }

        public void Enqueue(Point point, int value)
        {
            sortedDict.Add(point, value);
            numItems++;
            var tempDict = from entry in sortedDict orderby entry.Value ascending select entry;
            sortedDict = tempDict.ToDictionary();
        }

        public void Update(Point point,int newValue) 
        {
            sortedDict[point] = newValue;
            var tempDict = from entry in sortedDict orderby entry.Value ascending select entry;
            sortedDict = tempDict.ToDictionary();
        }
        public bool Contains(Point point)
        {
            if(sortedDict.ContainsKey(point)) return true;
            return false;
        }

        public int Distance(Point point)
        {
            if(sortedDict.TryGetValue(point, out int result)) return result;
            //return sortedDict[point];
            return -1;
        }

        public virtual Point Dequeue()
        {
            Point shortest = new Point();
            foreach (Point point in sortedDict.Keys)
            {
                shortest = point;
                sortedDict.Remove(point);
                break;
            }

            numItems--;
            //var tempDict = from entry in sortedDict orderby entry.Value ascending select entry;
            //sortedDict = tempDict.ToDictionary();
            return shortest;



        }


    }
}
