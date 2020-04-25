using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchPlayerPrefColor : MonoBehaviour
{
    [SerializeField] TMP_Text tMP_Text = default;
    PlayerPrefsController playerPrefsController = new PlayerPrefsController();
    // Start is called before the first frame update
    void Start()
    {   
        tMP_Text.color = playerPrefsController.accentColor;
    }
}
