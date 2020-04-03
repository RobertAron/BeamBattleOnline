using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBikeColorSelector : MonoBehaviour
{
    [SerializeField] MeshRenderer shipMR = default;
    [SerializeField] MeshRenderer trailMR = default;

    private MaterialPropertyBlock shipMaterialBlock;
    private MaterialPropertyBlock trailMaterialBlock;
 
    void Start()
    {
        shipMaterialBlock = new MaterialPropertyBlock();
        trailMaterialBlock = new MaterialPropertyBlock();
    }
 
    public void SetAccentColor(Color color){
        shipMaterialBlock.SetColor("_PrimaryColor", color);
        shipMR.SetPropertyBlock(shipMaterialBlock);
    }

    public void SetPrimaryColor(Color color){
        trailMaterialBlock.SetColor("_Color", color);
        trailMR.SetPropertyBlock(trailMaterialBlock);
        shipMaterialBlock.SetColor("_AccentColor", color);
        shipMR.SetPropertyBlock(shipMaterialBlock);
    }
}
