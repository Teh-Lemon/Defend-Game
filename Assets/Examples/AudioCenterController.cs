using UnityEngine;
using System.Collections;

public class AudioCenterController : MonoBehaviour {

	int soundId;

	void Start (){
		soundId = AudioCenter.loadSound ("Resources/ExampleSound/enemyKilled.wav");
	}

	public void playUnitySound ()
	{
		SoundCenter.Instance.playSound (0);
	}

	public void playPluginSound ()
	{
		AudioCenter.playSound (soundId);
	}

}
