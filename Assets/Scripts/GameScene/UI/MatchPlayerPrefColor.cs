using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchPlayerPrefColor : MonoBehaviour
{
    [SerializeField] TMP_Text tMP_Text = default;
    [SerializeField] Button button = default;
    PlayerPrefsController playerPrefsController = new PlayerPrefsController();
    // Start is called before the first frame update
    void Start()
    {   
        tMP_Text.color = playerPrefsController.accentColor;
        var colors = button.colors;
        colors.highlightedColor = playerPrefsController.accentColor;
        colors.pressedColor =  playerPrefsController.accentColor;
        button.colors = colors;
    }
}
