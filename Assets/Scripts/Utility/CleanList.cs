using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class CleanList
    {
        public static List<GameObject> RemoveDead(List<GameObject> list, int layerMask)
        {
            List<GameObject> deadPool = new List<GameObject>();
            foreach (GameObject ele in list)
            {
                if ( ( layerMask | ele.layer) != layerMask) deadPool.Add(ele);
            }

            foreach (GameObject ele in deadPool)
            {
                list.Remove(ele);
            }
            return list;
        }
    }
}
