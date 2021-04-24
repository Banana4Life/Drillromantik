using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TileGrid
{
    
    public class TilePopulator : MonoBehaviour
    {
        public TechTree techTree;
        private float[] _weights;

        private void Start()
        {
            _weights = new float[techTree.Structures.Length];
            for (var i = 0; i < techTree.Structures.Length; i++)
            {
                _weights[i] = techTree.Structures[i].spawnWeight;
            }
        }

        public void Populate(CubeCoord coord, GameObject tileObject)
        {
            var structure = Util.chooseWeighted(_weights, techTree.Structures);
            
            var tileScript = tileObject.GetComponent<TileScript>();
            if (structure.prefab)
            {
                GameObject go = Instantiate(structure.prefab, tileObject.transform, false);
                go.name = $"{structure.name}";
                tileScript.currentStructure = go;
            }
            tileScript.Structure = structure;
            tileScript.pos = coord;
        }
    }

    public class Util
    {

        public static float[] cumulativeDensity(IReadOnlyList<float> weights)
        {
            var cdf = new float[weights.Count];
            var prev = 0f;
            for (var i = 0; i < weights.Count; i++)
            {
                prev += weights[i];
                cdf[i] = prev;
            }

            return cdf;
        }
    
        private static T binaryFindSelectedValue<T>(IReadOnlyList<T> values, IReadOnlyList<float> cdf, int lower, int upper, float selection)
        {
            var mid = (lower + upper) / 2;

            float lowerEdge;
            if (mid == 0)
            {
                lowerEdge = 0f;
            }
            else
            {
                lowerEdge = cdf[mid - 1];
            }
            var upperEdge = cdf[mid];

            if (selection < lowerEdge)
            {
                return binaryFindSelectedValue(values, cdf, lower, mid, selection);
            }

            if (selection >= upperEdge)
            {
                return binaryFindSelectedValue(values, cdf, mid, upper, selection);
            }
        
            return values[mid];
        }

        public static T chooseWeighted<T>(IReadOnlyList<float> weights, IReadOnlyList<T> values, float selection)
        {
            var cdf = cumulativeDensity(weights);
            var sum = cdf[cdf.Length - 1];


            return binaryFindSelectedValue(values, cdf, 0, values.Count, selection * sum);
        }

        public static T chooseWeighted<T>(IReadOnlyList<float> weights, IReadOnlyList<T> values)
        {
            return chooseWeighted(weights, values, UnityEngine.Random.value);
        }
    }
}