using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Assertions.Must;

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
        public float volumeChangeRelative;
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
            PlayClipAtPoint(s.sound, Camera.main.transform.position, 1 + s.volumeChangeRelative).spatialBlend = 0;
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

	public static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position, float volume)
	{
		GameObject gameObject = new GameObject("One shot audio");
		gameObject.transform.position = position;
		AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
		audioSource.clip = clip;
		audioSource.spatialBlend = 1f;
		audioSource.volume = volume;
		audioSource.Play();
		Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
        return audioSource;
	}
}
