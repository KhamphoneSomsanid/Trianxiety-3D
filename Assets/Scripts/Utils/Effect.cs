using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public AudioSource[] audioSources = new AudioSource[4];
    // Start is called before the first frame update
    void Start()
    {
        Vibration.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void onRunVibrate() {        
        Vibration.Vibrate(100);
    }

    public void onRunSoundClick() {
        audioSources[0].Play();
    }

    public void onRunSoundCompleteOne() {
        audioSources[1].Play();
    }

    public void onRunSoundCompleteAll() {
        audioSources[2].Play();
    }

    public void onRunSoundFailed() {
        audioSources[3].Play();
    }

}
