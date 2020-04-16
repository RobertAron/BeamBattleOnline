using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[System.Obsolete]
public class RemainingPlayerTextSetter : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh = default;

    [ClientRpc]
    public void RpcUpdatePlayersRemaining(int playersRemaining){
        textMesh.text = $"{playersRemaining}";
    }
}
