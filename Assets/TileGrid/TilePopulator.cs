using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TileGrid
{
    public class TilePopulator : MonoBehaviour
    {
        private float[] _weights;

        private void Start()
        {
            var techTree = Global.FindTechTree();
            _weights = new float[techTree.Structures.Length];
            for (var i = 0; i < techTree.Structures.Length; i++)
            {
                _weights[i] = techTree.Structures[i].spawnWeight;
            }

        }

        public void Populate(CubeCoord coord, GameObject tileObject)
        {
            var techTree = Global.FindTechTree();
            var structure = Util.chooseWeighted(_weights, techTree.Structures);
            
            var tileScript = tileObject.GetComponent<TileScript>();
            tileScript.Init(coord, techTree);
            tileScript.AssignStructure(structure);
        }
        
        public void PopulateOrigin(GameObject tileObject)
        {
            var techTree = Global.FindTechTree();
            var structure = techTree.Structures.First(s => s.IsBase());

            var tileScript = tileObject.GetComponent<TileScript>();
            tileScript.Init(CubeCoord.ORIGIN, techTree);
            tileScript.AssignStructure(structure);
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