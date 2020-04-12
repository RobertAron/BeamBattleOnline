using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTextChanger: MonoBehaviour
{
    [SerializeField] TMP_InputField tMP_InputField = default;
    PlayerPrefsController playerPrefsController = new PlayerPrefsController();
    private void Start() {
        tMP_InputField.text = playerPrefsController.playerName;
    }

    public void OnTextChange(string newText){
        playerPrefsController.playerName = newText;
        
    }
}
