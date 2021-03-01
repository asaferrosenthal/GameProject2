using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class DetectColliderLayer
    {
        /// <summary>
        /// Detects colliders on a layer within a radius of a given location, then adds the unknown colliders to a given list.
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="radius"></param>
        /// <param name="location"></param>
        /// <param name="known"></param>
        /// <returns></returns>

        public static List<GameObject> InLayerRadius(int layer, float radius, Vector3 location, List<GameObject> known)
        {
            Collider[] targets = Physics.OverlapSphere(location, radius, layer);

            foreach (Collider potential in targets)
            {
                GameObject gPotential = potential.gameObject;
                if(!known.Contains(gPotential))
                {
                    known.Add(gPotential);
                }
            }
            
            return known;
        }
    }
}
