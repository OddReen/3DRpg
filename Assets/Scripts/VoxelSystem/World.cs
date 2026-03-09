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
}