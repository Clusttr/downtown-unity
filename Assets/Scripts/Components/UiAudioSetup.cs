using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiAudioSetup : MonoBehaviour
{
    public AudioSetup sound;
    public AudioEffect effect;

    public void Play()
    {
        //if (Menu.Instance != null)
        //    Menu.Instance.audioEffect.Play(sound, behaviour);
        //else if (Gameplay.Instance != null)
        //    Gameplay.Instance.audioEffect.Play(sound, behaviour);

        effect.Play(sound);
    }
}
