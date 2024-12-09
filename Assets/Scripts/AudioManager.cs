using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource soundSource;
    public static AudioManager instance;

    [Serializable]
    public struct Sound {
        public string name;
        public AudioClip sound;
    }
    public Sound[] sounds;
    public Sound[] tracks;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null){
            instance = this;
            //DontDestroyOnLoad(gameObject);
        } else{
            Destroy(gameObject);
        }
    }
    
    public void PlaySound(string name) {
        Sound s = Array.Find(sounds, x => x.name == name);
        if (s.name == "") {
            Debug.Log("Sound " + name + "not found");
        } else {
            soundSource.clip = s.sound;
            soundSource.Play();
        }
    }

    public void PlayMusic(string track){
        Sound s = Array.Find(tracks, x => x.name == track);
        if (s.name == "") {
            Debug.Log("Track " + track + "not found");
        } else {
            musicSource.clip = s.sound;
            musicSource.Play();
        }
    }
}
