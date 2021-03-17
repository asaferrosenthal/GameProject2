using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Utility
{
    public static class Sort
    {
        private static Vector3 _location;
        private static List<GameObject> _list = new List<GameObject>();
        public static List<GameObject> ByDistance(List<GameObject> objects, Vector3 location)
        {
            _location = location;
            _list = objects;

            int low = 0;
            int high = _list.Count - 1;
            
            QuickSort(low, high);

            return _list;
        }

        private static void QuickSort(int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(low, high);
                QuickSort(low, pi -1);
                QuickSort(pi+1,high);
            }
        }
        private static int Partition(int low, int high)
        {
            float pivot = (_list[high].transform.position - _location).magnitude;
            int i = (low - 1);

            for (int j = low; j <= high - 1; j++)
            {
                if ((_list[j].transform.position - _location).magnitude < pivot)
                {
                    i++;
                    Swap(i, j);
                }
            }
            Swap(i + 1, high);
            return (i + 1);
        }
        private static void Swap(int a, int b)
        {
            GameObject tempA = _list[a];
            _list[a] = _list[b];
            _list[b] = tempA;
        }
        
    }
}
