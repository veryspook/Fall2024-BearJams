using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventSounds : MonoBehaviour
{
    public void PlaySound(string soundName)
    {
        AudioManager.instance.PlaySound(soundName);
    }
}
