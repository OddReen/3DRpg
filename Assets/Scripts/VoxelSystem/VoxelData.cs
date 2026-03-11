using UnityEngine;

public static class VoxelData
{
    public static readonly int ChunkWidth = 10;
    public static readonly int ChunkHeight = 15;

    public static readonly int TextureAtlasSizeInBlocks = 4;
    public static float NormalizedBlockTextureSize
    {
        get{ return 1f/(float) TextureAtlasSizeInBlocks; }
    }

    public static readonly Vector3Int[] vertices = new Vector3Int[8]
    {
        new Vector3Int(0,0,0),
        new Vector3Int(1,0,0),
        new Vector3Int(1,1,0),
        new Vector3Int(0,1,0),
        new Vector3Int(0,0,1),
        new Vector3Int(1,0,1),
        new Vector3Int(1,1,1),
        new Vector3Int(0,1,1)
    };
    public static readonly Vector3Int[] faceDirections = new Vector3Int[6]
    {
        new Vector3Int(0,0,-1),
        new Vector3Int(0,0,1),
        new Vector3Int(0,1,0), // top
        new Vector3Int(0,-1,0), // bot
        new Vector3Int(-1,0,0),
        new Vector3Int(1,0,0)
    };
    public static readonly int[,] faces = new int[6,4]
    {
        // Back, Front, Top, Bottom, Left, Right
		{0, 3, 1, 2}, // Back Face
		{5, 6, 4, 7}, // Front Face
		{3, 7, 2, 6}, // Top Face
		{1, 5, 0, 4}, // Bottom Face
		{4, 7, 0, 3}, // Left Face
		{1, 2, 5, 6} // Right Face
    };
    public static readonly int[] triangles =
    {
        0,1,2,
        1,3,2
    };
    public static readonly Vector2[] uvs = new Vector2[4]
    {
        new Vector2(0,0),
        new Vector2(0,1),
        new Vector2(1,0),
        new Vector2(1,1)
    };
}
