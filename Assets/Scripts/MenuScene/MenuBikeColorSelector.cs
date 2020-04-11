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
 
    #region  Singleton
    public static MenuBikeColorSelector instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion


    void Start()
    {
        shipMaterialBlock = new MaterialPropertyBlock();
        trailMaterialBlock = new MaterialPropertyBlock();
        SetAccentColor(playerPrefsController.accentColor);
    }
 
    public void SetAccentColor(Color color){
        TooltipActions[] tooltipActions = FindObjectsOfType<TooltipActions>();
        foreach(TooltipActions tooltipAction in tooltipActions){
            tooltipAction.LerpToColor(color);
        }
        playerPrefsController.accentColor = color;
        shipMaterialBlock.SetColor("_AccentColor", color);
        shipMR.SetPropertyBlock(shipMaterialBlock);
        trailMaterialBlock.SetColor("_Color", color);
        trailMR.SetPropertyBlock(trailMaterialBlock);
    }
}
