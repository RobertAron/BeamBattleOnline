using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBikeColorSelector : MonoBehaviour
{
    [SerializeField] MeshRenderer shipMR = default;
    [SerializeField] MeshRenderer trailMR = default;
    private MaterialPropertyBlock shipMaterialBlock;
    private MaterialPropertyBlock trailMaterialBlock;
    PlayerPrefsController playerPrefsController = new PlayerPrefsController();
 
    void Start()
    {
        shipMaterialBlock = new MaterialPropertyBlock();
        trailMaterialBlock = new MaterialPropertyBlock();
        SetAccentColor(playerPrefsController.accentColor);
        SetPrimaryColor(playerPrefsController.primaryColor);
    }
 
    public void SetAccentColor(Color color){
        shipMaterialBlock.SetColor("_AccentColor", color);
        shipMR.SetPropertyBlock(shipMaterialBlock);
        trailMaterialBlock.SetColor("_Color", color);
        trailMR.SetPropertyBlock(trailMaterialBlock);
        playerPrefsController.accentColor = color;
    }

    public void SetPrimaryColor(Color color){
        shipMaterialBlock.SetColor("_PrimaryColor", color);
        shipMR.SetPropertyBlock(shipMaterialBlock);
        playerPrefsController.primaryColor = color;
    }
}
