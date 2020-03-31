using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBikeColorSelector : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] MeshRenderer shipMR;
    [SerializeField] MeshRenderer trailMR;

    private MaterialPropertyBlock shipMaterialBlock;
    private MaterialPropertyBlock trailMaterialBlock;
 
    void Start()
    {
        shipMaterialBlock = new MaterialPropertyBlock();
        trailMaterialBlock = new MaterialPropertyBlock();
    }
 
    [ContextMenu("HMMM")]
    public void ChangePropertyBlock()
    {
        shipMaterialBlock.SetColor("_AccentColor", color);
        shipMR.SetPropertyBlock(shipMaterialBlock);
        trailMaterialBlock.SetColor("_Color", color);
        trailMR.SetPropertyBlock(trailMaterialBlock);
    }
}
