using UnityEngine;
using System.Collections.Generic;
using System;

public class ColorSerializer {
    public static string ColorToString(Color color){
        return $"{color.r},{color.g},{color.b},{color.a}";
    }
    public static Color StringToColor(string stringColor){
        string[] rgba = stringColor.Split(',');
        var colors = new List<string>(rgba).ConvertAll<float>(ele => {
            return float.Parse(ele);
        });
        if(rgba.Length==4) return new Color(colors[0],colors[1],colors[2],colors[3]);
        return new Color();
    }
}