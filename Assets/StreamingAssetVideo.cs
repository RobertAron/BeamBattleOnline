using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class StreamingAssetVideo : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer = default;

    void Start(){
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = System.IO.Path.Combine (Application.streamingAssetsPath,"Tutorial_3.webm");
        videoPlayer.Play();
    }
}
