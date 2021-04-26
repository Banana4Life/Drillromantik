using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TileGrid
{
    public struct CubeCoord
    {
        private static readonly float[] CUBE_TO_WORLD = {
            1f, 1f / 2f,
            0f, 3f / 4f,
        };
        public static readonly float[] WORLD_TO_CUBE = {
            3f / 4f * 4f / 3f, -1f / 2f * 4f / 3f,
            0f               , 4f / 3f           ,
        };

        public static CubeCoord ORIGIN = new CubeCoord(0, 0);
        
        public static CubeCoord NORTH_EAST = new CubeCoord( 0, 1);
        public static CubeCoord EAST       = new CubeCoord( 1,  0);
        public static CubeCoord SOUTH_EAST = new CubeCoord( 1,  -1);
        public static CubeCoord SOUTH_WEST = new CubeCoord(0,  -1);
        public static CubeCoord WEST       = new CubeCoord(-1,  0);
        public static CubeCoord NORTH_WEST = new CubeCoord( -1, 1);

        public static CubeCoord[] Neighbors = { NORTH_EAST, EAST, SOUTH_EAST, SOUTH_WEST, WEST, NORTH_WEST };
        
        public int Q { get; }
        public int R { get; }
        public int S { get; }

        public CubeCoord(int q, int r, int s)
        {
            if (q + r + s != 0)
            {
                throw new Exception("q + r + s must be 0");
            }
            Q = q;
            R = r;
            S = s;
        }

        public CubeCoord(int q, int r) : this(q, r, -q - r)
        {
            
        }

        public float Length => (Math.Abs(Q) + Math.Abs(R) + Math.Abs(S)) / 2.0f;
        public double Distance(CubeCoord b) => (this - b).Length;

        public Vector3 ToWorld(int y, Vector3 size) => new Vector3((CUBE_TO_WORLD[0] * Q + CUBE_TO_WORLD[1] * R) * size.x, y,
            (CUBE_TO_WORLD[2] * Q + CUBE_TO_WORLD[3] * R) * size.z);
        
        public static CubeCoord FromWorld(Vector3 p) => new CubeCoord(Mathf.RoundToInt(WORLD_TO_CUBE[0] * p.x + WORLD_TO_CUBE[1] * p.z), Mathf.RoundToInt(WORLD_TO_CUBE[2] * p.x + WORLD_TO_CUBE[3] * p.z));

        public static CubeCoord operator +(CubeCoord a, CubeCoord b) => new CubeCoord(a.Q + b.Q, a.R + b.R, a.S + b.S);
        public static CubeCoord operator -(CubeCoord a, CubeCoord b) => new CubeCoord(a.Q - b.Q, a.R - b.R, a.S - b.S);
        public static CubeCoord operator *(CubeCoord a, int b) => new CubeCoord(a.Q * b, a.R * b, a.S * b);

        public override string ToString()
        {
            return $"{nameof(Q)}: {Q}, {nameof(R)}: {R}, {nameof(S)}: {S}";
        }

        public static IEnumerator<CubeCoord> Ring(CubeCoord center, int radius)
        {
            if (radius == 0)
            {
                yield return center;
            }
            else
            {
                var cube = (center + (WEST * radius));
                foreach (var direction in Neighbors)
                {
                    for (var i = 0; i < radius; ++i)
                    {
                        yield return cube;
                        cube += direction;
                    }
                }
            }
        }

        public static IEnumerator<CubeCoord> Spiral(CubeCoord center, int startRing = 0, int maxRings = -1)
        {
            for (var i = 0; i < maxRings || maxRings == -1; ++i)
            {
                var coords = Ring(center, startRing + i);
                while (coords.MoveNext())
                {
                    yield return coords.Current;
                }
            }
        }

        public static IEnumerator<CubeCoord> ShuffledRings(CubeCoord center, int startRing = 0, int maxRings = -1)
        {
            for (var i = 0; i < maxRings || maxRings == -1; ++i)
            {
                foreach (var coord in Ring(center, startRing + i).ToList().Shuffled())
                {
                    yield return coord;
                }
            }
        }
    }

    public static class EnumeratorExt
    {
        public static IEnumerator<T> Where<T>(this IEnumerator<T> e, Predicate<T> p)
        {
            while (e.MoveNext())
            {
                var c = e.Current;
                if (p(c))
                {
                    yield return c;
                }
            }
        }

        public static IEnumerator<T> WhereNot<T>(this IEnumerator<T> e, Predicate<T> p) => Where(e, v => !p(v));

        public static IEnumerator<T> Take<T>(this IEnumerator<T> e, int n)
        {
            while (n >= 0 && e.MoveNext())
            {
                yield return e.Current;
                n--;
            }
        }

        public static IList<T> ToList<T>(this IEnumerator<T> e)
        {
            var list = new List<T>();
            while (e.MoveNext())
            {
                list.Add(e.Current);
            }

            return list;
        }

        public static IList<T> Shuffled<T>(this IList<T> l)
        {
            var copy = new List<T>(l);
            int n = copy.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, n + 1);
                var value = copy[k];
                copy[k] = copy[n];
                copy[n] = value;
            }
            return copy;
        }
    } 
}