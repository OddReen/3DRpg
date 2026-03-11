using UnityEngine;

public class World : MonoBehaviour
{
    public Material _material;
    public BlockType[] blockTypes;
}

[System.Serializable]
public class BlockType
{
    public string blockName;
    public bool isSolid;

    [Header("Texture Values")]
    public int BackFace;
    public int FrontFace;
    public int TopFace;
    public int BottomFace;
    public int LeftFace;
    public int RightFace;

    public int GetTextureID(int InFaceIndex)
    {
        switch (InFaceIndex)
        {
            case 0:
            return BackFace;

            case 1:
            return FrontFace;

            case 2:
            return TopFace;

            case 3:
            return BottomFace;

            case 4:
            return LeftFace;

            case 5:
            return RightFace;

            default:
                return 0;
        }
    }
}