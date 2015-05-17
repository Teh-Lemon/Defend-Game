using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SoundCenter : MonoBehaviour {

	private static SoundCenter instance;
	public IDictionary <int, AudioClip> audioClips = new Dictionary<int, AudioClip> ();
	public float arcadeMusicVolume;
	public float menuMusicVolume;


	public static SoundCenter Instance {
		get {
			if (instance == null){
				throw new System.InvalidOperationException( "Sound center is not ready yet!" );}
			return instance;
		}
	}
	
	void Awake() {
		instance = this;
		loadAudioClips ();
	}

	public void playSound (int sound)
	{
		GetComponent<AudioSource>().PlayOneShot (audioClips [sound]);
	}
	
	public void stopSound ()
	{
		GetComponent<AudioSource>().Stop ();
	}


	public void loadAudioClips ()
	{
		Object [] clipArr = Resources.LoadAll ("ExampleSound", typeof(AudioClip));
		AudioClip [] clipArray = new AudioClip [clipArr.Length];
		for (int i = 0; i < clipArr.Length; i++){
			clipArray [i] = (AudioClip) clipArr [i];
			addSoundToDictionary(i, clipArray [i]);
		}
	}

	public void addSoundToDictionary (int name, AudioClip clip)
	{
		if (!audioClips.ContainsKey(name))
			audioClips.Add(name, clip);
	}

}
