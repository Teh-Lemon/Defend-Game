using UnityEngine;
using System.Collections;

public class AudioCenter
{
	#if UNITY_ANDROID && !UNITY_EDITOR
	public static AndroidJavaClass unityActivityClass = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" );
	public static AndroidJavaObject activityObj = unityActivityClass.GetStatic<AndroidJavaObject>( "currentActivity" );
	private static AndroidJavaObject soundObj = new AndroidJavaObject( "com.catsknead.androidsoundfix.AudioCenter", 1, activityObj, activityObj );
	
	public static void playSound( int soundId ) {
		soundObj.Call( "playSound", new object[] { soundId } );
	}
	
	public static int loadSound( string soundName ) {
		return soundObj.Call<int>( "loadSound", new object[] { soundName } );
	}
	
	public static void unloadSound( int soundId ) {
		soundObj.Call( "unloadSound", new object[] { soundId } );
	}
	#else
	public static void playSound( int soundId ) {
		Debug.Log( "Play sound called: " + soundId.ToString() );
	}
	
	public static int loadSound( string soundName ) {
		Debug.Log( "Load sound called: " + soundName );
		return 0;
	}
	
	public static void unloadSound( int soundId ) {
		Debug.Log( "Unload sound called: " + soundId.ToString() );
	}
	#endif
}
