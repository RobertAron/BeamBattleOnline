using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickRefreshUi : MonoBehaviour
{
    [SerializeField] ContentSizeFitter contentSizeFitter = default;
    // Start is called before the first frame update
    void Start()
    {
        contentSizeFitter.enabled = true;
    }
}
