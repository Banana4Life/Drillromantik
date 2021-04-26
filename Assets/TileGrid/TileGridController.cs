using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TileGrid
{
    [RequireComponent(typeof(TilePopulator))]
    public class TileGridController : MonoBehaviour
    {
        public GameObject TilePrefab;
        public int initialRings = 3;
        private MeshRenderer _renderer;
        private TilePopulator _populator;
        private Dictionary<CubeCoord, GameObject> _knownTiles = new Dictionary<CubeCoord, GameObject>();

        private Coroutine spawnRoutine = null;
        public GameObject worldUi;

        private Dictionary<StructureType, int> _typeCount = new Dictionary<StructureType, int>();
    
        void Start()
        {
            _renderer = TilePrefab.GetComponentInChildren<MeshRenderer>();
            _populator = GetComponent<TilePopulator>();
            spawnOrigin();
            var spiralCoords = CubeCoord.ShuffledRings(CubeCoord.ORIGIN, 1, initialRings);
            enqueue(() => SpawnAll(spiralCoords, 0.5f, 0.1f));
            //StartCoroutine(SpawnAll(CubeCoord.Spiral(CubeCoord.ORIGIN, 1).Take(40), 0.5f, 0.1f));
            //StartCoroutine(SpawnAll(CubeCoord.Ring(CubeCoord.ORIGIN, 2), 0.1f, 2f));
            //StartCoroutine(SpawnEdgeTiles());
        }

        private void enqueue(Func<IEnumerator> spawner)
        {
            Coroutine old = spawnRoutine;
            
            IEnumerator NewRoutine()
            {
                yield return old;
                yield return StartCoroutine(spawner());
            }

            spawnRoutine = StartCoroutine(NewRoutine());
        }

        IEnumerator SpawnAll(IEnumerator<CubeCoord> coords, float delay, float gap)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            while (coords.MoveNext())
            {
                spawnAndPopulateTile(coords.Current);
                if (gap > 0)
                {
                    yield return new WaitForSeconds(gap);
                }
            }
        }

        public void SpawnNewAroundAsync(CubeCoord coord)
        {
            var enumerator = CubeCoord.ShuffledRings(coord, 1, 3).WhereNot(_knownTiles.ContainsKey);

            enqueue(() => SpawnAll(enumerator, 0f, 0.2f));
        }

        private void spawnOrigin()
        {
            var tile = spawnTile(CubeCoord.ORIGIN);
            _populator.PopulateOrigin(tile);
        }

        public List<GameObject> GetNeighborTiles(CubeCoord coord)
        {
            return CubeCoord.Neighbors
                .Select(c => c + coord)
                .Where(_knownTiles.ContainsKey)
                .Select(c => _knownTiles[c])
                .ToList();
        }

        private GameObject spawnTile(CubeCoord pos)
        {
            var objectScale = TilePrefab.transform.localScale;
            var tileSize = _renderer.bounds.size;
            tileSize.Scale(objectScale);
            
            var instance = Instantiate(TilePrefab, transform, true);
            instance.name = $"{pos}";
            var worldPos = pos.ToWorld(0, tileSize);
            instance.transform.position = worldPos;
            _knownTiles[pos] = instance;
            return instance;
        }

        private GameObject spawnAndPopulateTile(CubeCoord pos)
        {
            var go = spawnTile(pos);
            _populator.Populate(pos, go);
            return go;
        }

        public int typeCount(StructureType type)
        {
            return _typeCount.ContainsKey(type) ? 0 : _typeCount[type];
        }

        public void CountStructure(StructureType structureType, int p1)
        {
            var cnt = _typeCount.ContainsKey(structureType) ? 0 : _typeCount[structureType];
            _typeCount[structureType] = cnt + p1;
        }
    }
}
