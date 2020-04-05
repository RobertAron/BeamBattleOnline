using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBikeColorSelector : MonoBehaviour
{
    [SerializeField] MeshRenderer shipMR = default;
    [SerializeField] MeshRenderer trailMR = default;
    [SerializeField] Color defaultAccent = default;
    [SerializeField] Color defaultPrimary = default;
    private MaterialPropertyBlock shipMaterialBlock;
    private MaterialPropertyBlock trailMaterialBlock;
 
    void Start()
    {
        shipMaterialBlock = new MaterialPropertyBlock();
        trailMaterialBlock = new MaterialPropertyBlock();
        string accentDefault = PlayerPrefs.GetString("accentColor",ColorSerializer.ColorToString(defaultAccent));
        string primaryDefault = PlayerPrefs.GetString("primaryColor",ColorSerializer.ColorToString(defaultPrimary));
        SetAccentColor(ColorSerializer.StringToColor(accentDefault));
        SetPrimaryColor(ColorSerializer.StringToColor(primaryDefault));
    }
 
    public void SetAccentColor(Color color){
        shipMaterialBlock.SetColor("_PrimaryColor", color);
        shipMR.SetPropertyBlock(shipMaterialBlock);
        PlayerPrefs.SetString("accentColor",ColorSerializer.ColorToString(color));
    }

    public void SetPrimaryColor(Color color){
        trailMaterialBlock.SetColor("_Color", color);
        trailMR.SetPropertyBlock(trailMaterialBlock);
        shipMaterialBlock.SetColor("_AccentColor", color);
        shipMR.SetPropertyBlock(shipMaterialBlock);
        PlayerPrefs.SetString("primaryColor",ColorSerializer.ColorToString(color));
    }
}
