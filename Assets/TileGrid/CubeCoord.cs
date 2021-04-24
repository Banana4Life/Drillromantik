using System;
using UnityEngine;

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
        
        public static CubeCoord NORTH_EAST = new CubeCoord( 1, -1);
        public static CubeCoord EAST       = new CubeCoord( 1,  0);
        public static CubeCoord SOUTH_EAST = new CubeCoord( 0,  1);
        public static CubeCoord SOUTH_WEST = new CubeCoord(-1,  1);
        public static CubeCoord WEST       = new CubeCoord(-1,  0);
        public static CubeCoord NORTH_WEST = new CubeCoord( 0, -1);

        public static CubeCoord[] Neighbors = { NORTH_EAST, EAST, SOUTH_EAST, SOUTH_WEST, SOUTH_WEST, WEST, NORTH_WEST };
        
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
    }
}