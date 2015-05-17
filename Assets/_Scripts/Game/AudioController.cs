using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour 
{
    [SerializeField]
    AudioSource selectAS;
    [SerializeField]
    AudioSource hitAS;
    [SerializeField]
    AudioSource powerupAS;

    public static AudioController Instance { get; set; }

    struct SoundEffect
    {
        public AudioSource UnityASource { get; set; }
        public int AndroidAID { get; set; }

        public SoundEffect(AudioSource unity, int android)
        {
            UnityASource = unity;
            AndroidAID = android;
        }
    }

    SoundEffect[] soundEffects = new SoundEffect[3];

    public enum Sounds
    {
        SELECT,
        HIT,
        POWERUP
    };
    
	// Use this for initialization
	void Awake () 
    {
        Instance = this;
        
        // Load the sound effects
        int soundID;
        // Select
        soundID = AudioCenter.loadSound("Resources/Audio/select.wav");
        soundEffects[(int)Sounds.SELECT] = new SoundEffect(selectAS, soundID);
        // Hit
        soundID = AudioCenter.loadSound("Resources/Audio/hit.wav");
        soundEffects[(int)Sounds.HIT] = new SoundEffect(hitAS, soundID);
        // Powerup
        soundID = AudioCenter.loadSound("Resources/Audio/powerup.wav");
        soundEffects[(int)Sounds.POWERUP] = new SoundEffect(powerupAS, soundID);
	}

    void Play(SoundEffect sound)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AudioCenter.playSound(sound.AndroidAID);
#else
        sound.UnityASource.Play();
#endif
    }

    public void Play(Sounds sound)
    {
        Play(soundEffects[(int)sound]);
    }
}
