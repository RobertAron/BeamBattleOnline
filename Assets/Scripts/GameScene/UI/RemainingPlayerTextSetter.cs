using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[System.Obsolete]
public class RemainingPlayerTextSetter : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI countTextMesh = default;
    [SerializeField] TextMeshProUGUI remainTextMesh = default;

    private void Start()
    {
        countTextMesh.enabled = false;
        remainTextMesh.enabled = false;
    }

    [ClientRpc]
    public void RpcUpdatePlayersRemaining(int playersRemaining)
    {
        countTextMesh.enabled = true;
        remainTextMesh.enabled = true;
        countTextMesh.text = $"{playersRemaining}";
    }
}
