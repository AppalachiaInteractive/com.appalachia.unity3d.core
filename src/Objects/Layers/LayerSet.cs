#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Appalachia.Core.Objects.Layers
{
    public struct LayerSet
    {
        public LayerSet(params int[] layersInSet)
        {
            layers = new List<int>();
            _layerLookup = new HashSet<int>();
            layerMask = 0;

            for (var i = 0; i < layersInSet.Length; i++)
            {
                AssignLayer(layersInSet[i], _layerLookup, layers, ref layerMask);
            }

            oppositeLayerMask = ~layerMask;
        }

        public LayerSet(LayerSet other, params int[] layersInSet)
        {
            layers = new List<int>();
            _layerLookup = new HashSet<int>();
            layerMask = 0;

            for (var i = 0; i < layersInSet.Length; i++)
            {
                AssignLayer(layersInSet[i], _layerLookup, layers, ref layerMask);
            }

            for (var i = 0; i < other.layers.Count; i++)
            {
                AssignLayer(other.layers[i], _layerLookup, layers, ref layerMask);
            }

            oppositeLayerMask = ~layerMask;
        }

        public LayerSet(params LayerSet[] others)
        {
            layers = new List<int>();
            _layerLookup = new HashSet<int>();
            layerMask = 0;

            for (var i = 0; i < others.Length; i++)
            {
                for (var j = 0; j < others[i].layers.Count; j++)
                {
                    AssignLayer(others[i].layers[j], _layerLookup, layers, ref layerMask);
                }
            }

            oppositeLayerMask = ~layerMask;
        }

        #region Fields and Autoproperties

        public readonly List<int> layers;
        public LayerMask layerMask;
        public LayerMask oppositeLayerMask;
        private readonly HashSet<int> _layerLookup;

        #endregion

        public bool ContainsLayer(GameObject go)
        {
            return _layerLookup.Contains(go.layer);
        }

        public bool ContainsLayer(int layer)
        {
            return _layerLookup.Contains(layer);
        }

        private static void AssignLayer(
            int layer,
            HashSet<int> layerLookup,
            List<int> layers,
            ref LayerMask mask)
        {
            if (layerLookup.Contains(layer))
            {
                return;
            }

            layerLookup.Add(layer);
            layers.Add(layer);
            mask |= 1 << layer;
        }
    }
}
