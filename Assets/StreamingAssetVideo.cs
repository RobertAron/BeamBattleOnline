using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class StreamingAssetVideo : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer = default;
    void Awake()
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Tutorial_3.webm");
    }
}
