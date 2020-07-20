using System.Runtime.InteropServices;
using UnityEngine;

public class GetURL {
    [DllImport("__Internal")]
    public static extern string GetURLFromPage();
}