using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerItemContainerUtil : MonoBehaviour
{
    [SerializeField] GameObject colorPickerItemPrefab;

    [ContextMenu("FillColors")]
    void FillWithColorItems()
    {
        float hueSpread = 1f / 9f;
        float valueSpread = 0.25f;
        for (int column = 0; column < 8; ++column)
        {
            for (int row = 0; row < 4; ++row)
            {
                Debug.Log(hueSpread * column);
                Color color = Color.HSVToRGB(
                    hueSpread * column,
                    .25f + valueSpread * row,
                    .75f
                );
                var go = Instantiate(colorPickerItemPrefab, this.transform);
                go.GetComponentInChildren<ColorPickerItemLogic>().Init(color);
            }
        }
    }

    [ContextMenu("Kill All Children")]
    public void KillChildren()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
