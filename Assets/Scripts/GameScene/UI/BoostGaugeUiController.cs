using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Obsolete]
public class BoostGaugeUiController : MonoBehaviour
{

    
    #region  Singleton
    public static BoostGaugeUiController instance;
    private void Awake()
    {
        if (instance == null)
        {
        instance = this;
        }
    }
    #endregion
    
    [SerializeField] int bostElementMult = 4;
    [SerializeField] Sprite upTriangle = default;
    [SerializeField] Sprite downTriangle = default;
    [SerializeField] Transform meterParent = default;
    [SerializeField] List<Image> boostGuageElements = default;
    [SerializeField] GameObject meterElementPrefab = default;

    private void Start() {
        UpdateBoostIconsDraw(0);
    }


    public void SubscribeToBike(BikeMovement bikeMovement){
        bikeMovement.OnMaxBoostChange += UpdateBoostIconsDraw;
        UpdateBoostIconsDraw(bikeMovement.maxBoostTimeAvailable);
        bikeMovement.OnBoostChange += UpdateFillAmount;
        UpdateBoostIconsDraw(bikeMovement.currentBoostTimeAvailable);
    }

    void UpdateBoostIconsDraw(float maxBoostTime){
        boostGuageElements.Clear();
        foreach(Transform child in meterParent){
            Destroy(child.gameObject);
        }
        for(int i=0;i<maxBoostTime*bostElementMult;++i){
            var go = Instantiate(meterElementPrefab,meterParent);
            Image elementImage = go.GetComponent<Image>();
            elementImage.sprite = i%2==0?upTriangle:downTriangle;
            boostGuageElements.Add(elementImage);
        }
    }

    void UpdateFillAmount(float boostTime){
        boostTime *= bostElementMult;
        for(int i=0;i<boostGuageElements.Count;++i){
            Color newColor = boostGuageElements[i].color;
            if(Mathf.Floor(boostTime)>i) newColor.a = 1;
            else if(boostTime>i) newColor.a = boostTime%1;
            else newColor.a = 0;
            boostGuageElements[i].color = newColor;
        }
    }
}
