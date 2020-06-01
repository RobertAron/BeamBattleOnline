using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;

[System.Obsolete]
public class BikeVictoryStuff : NetworkBehaviour
{
    [SerializeField] Renderer victoryRingRenderer = default;
    [SerializeField] GameObject victoryStuff = default;
    [SerializeField] BikeMovement bikeMovement = default;
    [SerializeField] TMP_Text mainText = default;
    [SerializeField] TMP_Text shadowText = default;
    [SerializeField] Canvas canvas = default;
    [SerializeField] BoxCollider boxCollider = default;
    Material victoryMaterial = default;

    private void Awake()
    {
        victoryMaterial = victoryRingRenderer.material;
    }

    private void Start() {
        var lossGameText = $"{bikeMovement.GetPlayerName()} WINS!";
        mainText.text = lossGameText;
        shadowText.text = lossGameText;
        shadowText.color = bikeMovement.GetAccentColor();
    }

    [Server]
    public void TurnOffBikeCollider(){
        boxCollider.enabled = false;
    }

    [ClientRpc]
    public void RpcWinAnimationStuff()
    {
        victoryStuff.active = true;
        CanvasTextMovement();
    }

    [TargetRpc]
    public void TargetSetWinningPlayer(NetworkConnection networkConnection)
    {
        mainText.text = "VICTORY";
        shadowText.text = "VICTORY";
    }

    void CanvasTextMovement()
    {
        var canvasRectTransform = canvas.GetComponent<RectTransform>();
        float canvasStartYPosition = -.5f;
        float canvasEndYPosition = canvasRectTransform.position.y;
        canvasRectTransform.position = new Vector3(canvasRectTransform.position.x, -10, canvasRectTransform.position.z);
        Action<float> updateFunction = (float yposition) =>
        {
            canvasRectTransform.position = new Vector3(canvasRectTransform.position.x, yposition, canvasRectTransform.position.z);
        };
        LeanTween.value(
            gameObject,updateFunction,canvasStartYPosition,canvasEndYPosition, .5f
        ).setEaseOutBounce().setDelay(.3f).setOnComplete(VictoryRingExplosion);
    }

    void VictoryRingExplosion()
    {
        victoryMaterial.SetColor("_Color", bikeMovement.GetAccentColor());
        UpdateRingPercentSize(0);
        UpdateRingPercentAlpha(1);
        LeanTween.
            value(gameObject, UpdateRingPercentSize, 0, 1, .7f).
            setEaseInOutExpo().
            setOnComplete(VictoryRingFade);
    }

    void UpdateRingPercentSize(float percent)
    {
        victoryMaterial.SetFloat("_Percent", percent);
    }

    void VictoryRingFade()
    {
        LeanTween.value(gameObject, UpdateRingPercentAlpha, 1, 0, .6f).setEaseOutQuad();
    }

    void UpdateRingPercentAlpha(float percent)
    {
        victoryMaterial.SetFloat("_Alpha", percent);
    }

}
