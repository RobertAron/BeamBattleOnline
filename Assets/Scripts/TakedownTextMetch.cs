using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TakedownTextMetch : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh = default;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefsController hm = new PlayerPrefsController();
        int takedownCount = hm.killCount;
        textMesh.text = $"{takedownCount} TAKEDOWNS";   
    }
}
