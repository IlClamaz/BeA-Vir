using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerRestart : MonoBehaviour
{
    private AudioSource[] audioSources;
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.GetComponent<VideoPlayer>().targetTexture.Release();
        audioSources = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (var source in audioSources)
        {
            source.Stop();
        }
    }
}
