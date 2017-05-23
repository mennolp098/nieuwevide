using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace ProefProeve.Audio
{
	public class AudioManager : MonoBehaviour 
	{
		#region Static
		private const int MAXCHANNELS = 25;
		private const float FADESTRENGHT = 0.01f;

		public static AudioManager Instance
		{
			get{
				if (_instance != null)
					return _instance;

				//Creating singleton
				GameObject newGameObject = new GameObject ("AudioManager");
				newGameObject.AddComponent<AudioListener> ();
				AudioManager audioManager = newGameObject.AddComponent<AudioManager> ();

				_instance = audioManager;

				if (_instance.Initialize ())
					return _instance;

				Debug.Log ("Initialization failed!");
				return null;
			}
		}

		private static AudioManager _instance;
		private static bool initialized = false;
		#endregion

		public enum AudioType
		{
			Music,
			SFX
		}

		private AudioMixer audioMixer;
		private GameObject audioChannelsContainer;
		private List<AudioChannel> audioChannels = new List<AudioChannel>();
		private bool isDataLoaded = false;

		public float GetPlayTime(AudioAsset asset)
		{
			for (int i = 0; i < audioChannels.Count; i++) {
				if (audioChannels [i].IsPlaying && audioChannels [i].ContainsAudioAsset (asset)) {
					return audioChannels [i].Playtime;
				}
			}

			// Fallback
			return -1;
		}

		/// <summary>
		/// Play the specified audioAsset.
		/// </summary>
		/// <param name="audioAsset">Audio asset.</param>
		/// <param name="loop">Loop audio asset</param">
		/// <param name="canMultiply">If set to <c>true</c> can have multiple sounds playing of the same sound.</param>
		public AudioChannel Play(AudioAsset audioAsset,bool loop = false, bool duplicateAllowed = false)
		{
			if (audioAsset == null) {
				Debug.LogWarning ("Audio asset not found!");
				return null;
			}

			// If the audioasset can not be dupplicate playing check all channels if it is already playing
			if (!duplicateAllowed) {
				foreach (AudioChannel audioChannel in audioChannels) {
					if (audioChannel.IsPlaying && audioChannel.ContainsAudioAsset (audioAsset)) {
						return audioChannel;
					}
				} 
			}

			// Start playing the audioasset on a channel that is not in use.
			foreach (AudioChannel audioChannel in audioChannels) {
				if (!audioChannel.IsPlaying) {
					audioChannel.Play (audioAsset, loop);

					// When playing a audioasset for the first time it will initialize the audiomixer.
					// With that information we can reset the volume of the SFX and Music to their original state.
					if (!isDataLoaded)
						LoadSavedData ();
					
					return audioChannel;
				}
			}

			Debug.LogError ("[AudioManager] No more available channels!");
			return null;
		}

		/// <summary>
		/// Replace the specified replaceAsset to inAsset.
		/// </summary>
		/// <param name="replaceAsset">Replace asset.</param>
		/// <param name="inAsset">In asset.</param>
		/// <param name="replacePlaytime">If set to <c>true</c> replace playtime aswell.</param>
		public AudioChannel Replace(AudioAsset replaceAsset, AudioAsset inAsset, bool replacePlaytime = false, bool loop = false, bool dupplicateAllowed = false)
		{
			float curPlaytime = GetPlayTime (replaceAsset);

			Stop (replaceAsset);

			AudioChannel inChannel = Play (inAsset, loop, dupplicateAllowed);

			if(curPlaytime != -1 && replacePlaytime)
				inChannel.Playtime = curPlaytime;

			return inChannel;
		}

		/// <summary>
		/// Replace the specified replaceAsset to inAsset.
		/// </summary>
		/// <param name="replaceAsset">Replace asset.</param>
		/// <param name="inAsset">In asset.</param>
		/// <param name="replacePlaytime">If set to <c>true</c> replace playtime aswell.</param>
		public AudioChannel ReplaceWithFade(AudioAsset replaceAsset, AudioAsset inAsset, bool replacePlaytime = false, bool loop = false, bool dupplicateAllowed = false)
		{
			float curPlaytime = GetPlayTime (replaceAsset);

			FadeOut (replaceAsset,Stop,replaceAsset);

			AudioChannel inChannel = Play (inAsset, loop, dupplicateAllowed);
			inChannel.Volume = 0;
			FadeIn (inAsset);

			if(curPlaytime != -1 && replacePlaytime)
				inChannel.Playtime = curPlaytime;

			return inChannel;
		}

		/// <summary>
		/// Replaces the audioasset with fade in ONLY, if you wanna fade both audio assets look at <c>ReplaceWithFade</c>.
		/// </summary>
		/// <returns>The with fade in.</returns>
		/// <param name="replaceAsset">Replace asset.</param>
		/// <param name="inAsset">In asset.</param>
		/// <param name="replacePlaytime">If set to <c>true</c> replace playtime.</param>
		/// <param name="loop">If set to <c>true</c> loop.</param>
		/// <param name="dupplicateAllowed">If set to <c>true</c> dupplicate allowed.</param>
		public AudioChannel ReplaceWithFadeIn(AudioAsset replaceAsset, AudioAsset inAsset, bool replacePlaytime = false, bool loop = false, bool dupplicateAllowed = false)
		{
			float curPlaytime = GetPlayTime (replaceAsset);

			AudioChannel inChannel = Play (inAsset, loop, dupplicateAllowed);
			inChannel.Volume = 0;
			FadeIn (inAsset,Stop,replaceAsset);

			if(curPlaytime != -1 && replacePlaytime)
				inChannel.Playtime = curPlaytime;

			return inChannel;
		}

		/// <summary>
		/// Fades the playing audioasset out.
		/// </summary>
		/// <param name="audioAsset">Audio asset.</param>
		public void FadeOut(AudioAsset audioAsset, System.Action<AudioAsset> callback = null,AudioAsset callbackParam = null)
		{
			for (int i = 0; i < audioChannels.Count; i++) {
				if (audioChannels [i].ContainsAudioAsset (audioAsset) && audioChannels [i].IsPlaying) {
					StartCoroutine (FadeOutCoroutine(audioChannels[i], callback, callbackParam));
					break;
				}
			}
		}

		/// <summary>
		/// Fades the playing audioasset.
		/// </summary>
		/// <param name="audioAsset">Audio asset.</param>
		public void FadeIn(AudioAsset audioAsset)
		{
			for (int i = 0; i < audioChannels.Count; i++) {
				if (audioChannels [i].ContainsAudioAsset (audioAsset) && audioChannels [i].IsPlaying) {
					StartCoroutine (FadeInCoroutine(audioChannels[i], audioAsset));
					break;
				}
			}
		}

		/// <summary>
		/// Fades the playing audioasset in to specified volume.
		/// </summary>
		/// <param name="audioAsset">Audio asset.</param>
		public void FadeIn(AudioAsset audioAsset, float volume)
		{
			for (int i = 0; i < audioChannels.Count; i++) {
				if (audioChannels [i].ContainsAudioAsset (audioAsset) && audioChannels [i].IsPlaying) {
					StartCoroutine (FadeInCoroutine(audioChannels[i], volume));
					break;
				}
			}
		}

		/// <summary>
		/// Stop a channel that is playing the Audio Asset
		/// </summary>
		/// <param name="audioAsset">Audio asset.</param>
		public void Stop(AudioAsset audioAsset)
		{
			foreach (AudioChannel audioChannel in audioChannels) {
				if (audioChannel.ContainsAudioAsset (audioAsset) && audioChannel.IsPlaying) {
					audioChannel.Stop ();
				}
			}
		}

		/// <summary>
		/// Stop the specified audioChannel.
		/// </summary>
		/// <param name="audioChannel">Audio channel.</param>
		public void Stop(AudioChannel audioChannel)
		{
			audioChannel.Stop ();
		}

		/// <summary>
		/// Stops all audiochannels.
		/// </summary>
		public void StopAll()
		{
			foreach (AudioChannel audioChannel in audioChannels) {
				if (audioChannel.IsPlaying) {
					audioChannel.Stop ();
				}
			}
		}

		/// <summary>
		/// Pauses a channel that is playing the Audio Asset
		/// </summary>
		/// <param name="audioAsset">Audio asset.</param>
		public void Pause(AudioAsset audioAsset)
		{
			foreach (AudioChannel audioChannel in audioChannels) {
				if (audioChannel.ContainsAudioAsset (audioAsset) && audioChannel.IsPlaying) {
					audioChannel.Pause ();
				}
			}
		}

		/// <summary>
		/// Pauses the specified audioChannel.
		/// </summary>
		/// <param name="audioChannel">Audio channel.</param>
		public void Pause(AudioChannel audioChannel)
		{
			audioChannel.Pause ();
		}

		/// <summary>
		/// Pauses all audiochannels.
		/// </summary>
		public void PauseAll()
		{
			foreach (AudioChannel audioChannel in audioChannels) {
				if (audioChannel.IsPlaying) {
					audioChannel.Pause ();
				}
			}
		}

		/// <summary>
		/// Resumes a channel that is playing the Audio Asset
		/// </summary>
		/// <param name="audioAsset">Audio asset.</param>
		public void Resume(AudioAsset audioAsset)
		{
			foreach (AudioChannel audioChannel in audioChannels) {
				if (audioChannel.ContainsAudioAsset (audioAsset) && audioChannel.IsPlaying) {
					audioChannel.Resume ();
				}
			}
		}

		/// <summary>
		/// Resumes the specified audioChannel.
		/// </summary>
		/// <param name="audioChannel">Audio channel.</param>
		public void Resume(AudioChannel audioChannel)
		{
			audioChannel.Resume ();
		}

		/// <summary>
		/// resumes all audiochannels.
		/// </summary>
		public void ResumeAll()
		{
			foreach (AudioChannel audioChannel in audioChannels) {
				if (audioChannel.IsPlaying) {
					audioChannel.Resume ();
				}
			}
		}

		/// <summary>
		/// Toggles volume on master
		/// </summary>
		public void ToggleMute()
		{
			float vol = 0;
			audioMixer.GetFloat ("Master", out vol);
			if (vol == 0) {
				audioMixer.SetFloat ("Master", -80); //-80 being fully muted
				return;
			}
			audioMixer.SetFloat ("Master", 0);
		}

		/// <summary>
		/// Toggles volume on audioMixer groups
		/// </summary>
		/// <param name="type">Type.</param>
		public void ToggleMute(AudioType type)
		{
			string typeString = "";
			switch (type) {
			case AudioType.Music:
				typeString = "Music";
				break;
			case AudioType.SFX:
				typeString = "SFX";
				break;
			default:
				break;
			}

			float vol = 0;
			audioMixer.GetFloat (typeString, out vol);
			if (vol == 0) {
				audioMixer.SetFloat (typeString, -80);
				return;
			}
			audioMixer.SetFloat (typeString, 0);
		}

		/// <summary>
		/// Determines whether the master is muted.
		/// </summary>
		/// <returns><c>true</c> if this instance is muted; otherwise, <c>false</c>.</returns>
		public bool IsMuted()
		{
			float vol = 0;
			audioMixer.GetFloat ("Master", out vol);
			return (vol != 0);
		}

		/// <summary>
		/// Determines whether one the specified AudioType is muted.
		/// </summary>
		/// <returns><c>true</c> if the specified AudioType is muted; otherwise, <c>false</c>.</returns>
		/// <param name="type">Type.</param>
		public bool IsMuted(AudioType type)
		{
			string typeString = "";
			switch (type) {
			case AudioType.Music:
				typeString = "Music";
				break;
			case AudioType.SFX:
				typeString = "SFX";
				break;
			default:
				break;
			}

			float vol = 0;
			audioMixer.GetFloat (typeString, out vol);
			return (vol != 0);
		}

		/// <summary>
		/// Sets the volume.
		/// </summary>
		/// <param name="volume">Volume.</param>
		public void SetVolume(float volume)
		{
			PlayerPrefs.SetFloat ("MASTERVOLUME", volume);
			audioMixer.SetFloat ("Master", volume);
		}

		/// <summary>
		/// Sets the volume of specified AudioType.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="volume">Volume.</param>
		public void SetVolume(AudioType type, float volume) //TODO: volume range
		{
			string typeString = "";
			string saveDataString = "";
			switch (type) {
			case AudioType.Music:
				typeString = "Music";
				saveDataString = "MUSICVOLUME";
				break;
			case AudioType.SFX:
				typeString = "SFX";
				saveDataString = "SFXVOLUME";
				break;
			default:
				break;
			}

			// Saving and setting volume for specific audio type
			PlayerPrefs.SetFloat (saveDataString, volume);
			audioMixer.SetFloat (typeString, volume);
		}

		/// <summary>
		/// Gets the master volume.
		/// </summary>
		/// <returns>The master volume.</returns>
		public float GetVolume()
		{
			float vol = 0;
			audioMixer.GetFloat ("Master", out vol);
			return vol;
		}

		/// <summary>
		/// Gets the volume from specified AudioType.
		/// </summary>
		/// <returns>The volume of specified AudioType.</returns>
		/// <param name="type">Type.</param>
		public float GetVolume(AudioType type)
		{
			string typeString = "";
			switch (type) {
			case AudioType.Music:
				typeString = "Music";
				break;
			case AudioType.SFX:
				typeString = "SFX";
				break;
			default:
				break;
			}

			float vol = 0;
			audioMixer.GetFloat (typeString, out vol);

			return vol;
		}
	    
	    /// <summary>
	    /// Is audio asset currently playing
	    /// </summary>
	    /// <param name="asset"></param>
	    /// <returns>true if asset is playing; else false;</returns>
	    public bool IsPlaying(AudioAsset asset)
	    {
	        foreach (var channel in audioChannels)
	        {
	            if(channel.IsPlaying && channel.ContainsAudioAsset(asset))
	            {
	                return true;
	            }
	        }
	        return false;
	    }

		/// <summary>
		/// Raises the enable event.
		/// </summary>
		private void OnEnable () {
			if (!Initialize ()) {
				Debug.Log ("Audiomanager not correctly initialized");
			}
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		private bool Initialize()
		{
			if (_instance != null && _instance != this) {
				Destroy (this.gameObject);
			}
			
			if(initialized) 
				return true;
			
			audioMixer = Resources.Load ("AudioMixer", typeof(AudioMixer)) as AudioMixer;

			audioChannelsContainer = new GameObject ();
			audioChannelsContainer.name = "AudioChannelsContainer";
			audioChannelsContainer.transform.parent = this.transform;

			for (int i = 0; i < MAXCHANNELS; i++) {
				CreateAudioChannel ();
			}

			_instance = this;

			initialized = true;

			return true;
		}

        private void OnDestroy()
        {
            initialized = false;
            _instance = null;
        }

		private void LoadSavedData()
		{
			// Getting all saved data.
			float musicVolume = PlayerPrefs.GetFloat ("MUSICVOLUME", 0);
			float sfxVolume = PlayerPrefs.GetFloat ("SFXVOLUME", 0);
			float masterVolume = PlayerPrefs.GetFloat ("MASTERVOLUME", 0);

			// Setting audio type volumes.
			SetVolume (AudioType.Music, musicVolume);
			SetVolume (AudioType.SFX, sfxVolume);

			// Setting master volume.
			SetVolume (masterVolume);

			isDataLoaded = true;
		}

		/// <summary>
		/// Creates a new audio channel.
		/// </summary>
		/// <returns>The audio channel.</returns>
		private AudioChannel CreateAudioChannel()
		{
			GameObject newAudioChannelContainer = new GameObject ();
			newAudioChannelContainer.name = "AudioChannel-" + audioChannels.Count;
			newAudioChannelContainer.transform.parent = audioChannelsContainer.transform;
			AudioChannel newAudioChannel = newAudioChannelContainer.AddComponent<AudioChannel> ();
			audioChannels.Add (newAudioChannel);

			return newAudioChannel;
		}

		/// <summary>
		/// Fades the playing audioasset.
		/// </summary>
		/// <param name="audioAsset">Audio asset.</param>
		private void FadeIn(AudioAsset audioAsset, System.Action<AudioAsset> callback = null,AudioAsset callbackParam = null)
		{
			for (int i = 0; i < audioChannels.Count; i++) {
				if (audioChannels [i].ContainsAudioAsset (audioAsset) && audioChannels [i].IsPlaying) {
					StartCoroutine (FadeInCoroutine(audioChannels[i], audioAsset, callback,callbackParam));
					break;
				}
			}
		}

		/// <summary>
		/// Fades the playing audioasset in to specified volume.
		/// </summary>
		/// <param name="audioAsset">Audio asset.</param>
		private void FadeIn(AudioAsset audioAsset, float volume, System.Action<AudioAsset> callback = null,AudioAsset callbackParam = null)
		{
			for (int i = 0; i < audioChannels.Count; i++) {
				if (audioChannels [i].ContainsAudioAsset (audioAsset) && audioChannels [i].IsPlaying) {
					StartCoroutine (FadeInCoroutine(audioChannels[i], volume, callback,callbackParam));
					break;
				}
			}
		}

		/// <summary>
		/// Fades the audiochannel out.
		/// </summary>
		/// <returns>The out coroutine.</returns>
		/// <param name="channelToFadeOut">Channel to fade out.</param>
		/// <param name="callback">Callback.</param>
		private IEnumerator FadeOutCoroutine(AudioChannel channelToFadeOut, System.Action<AudioAsset> callback = null, AudioAsset callbackParam = null)
		{
			bool isFading = true;
			while (isFading) {
				if (channelToFadeOut.Volume > 0) {
					channelToFadeOut.Volume -= FADESTRENGHT;
				} else {
					isFading = false;
				}
				yield return new WaitForEndOfFrame ();
			}

			if (callback != null)
				callback (callbackParam);
		}

		/// <summary>
		/// Fades the audiochannel in to volume of specified volume parameter.
		/// </summary>
		/// <returns>The in coroutine.</returns>
		/// <param name="channelToFadeOut">Channel to fade out.</param>
		/// <param name="volume">Volume.</param>
		/// <param name="callback">Callback.</param>
		private IEnumerator FadeInCoroutine(AudioChannel channelToFadeOut, float volume, System.Action<AudioAsset> callback = null,AudioAsset callbackParam = null)
		{
			bool isFading = true;
			while (isFading) {
				if (channelToFadeOut.Volume < volume/100) {
					channelToFadeOut.Volume += FADESTRENGHT;
				} else {
					isFading = false;
				}
				yield return new WaitForEndOfFrame ();
			}

			if (callback != null)
				callback (callbackParam);

			yield break;
		}

		/// <summary>
		/// Fades the audiochannel in to volume of specified audioasset.
		/// </summary>
		/// <returns>The in coroutine.</returns>
		/// <param name="channelToFadeOut">Channel to fade out.</param>
		/// <param name="assetForVolume">Asset for volume.</param>
		/// <param name="callback">Callback.</param>
		private IEnumerator FadeInCoroutine(AudioChannel channelToFadeOut, AudioAsset assetForVolume, System.Action<AudioAsset> callback = null,AudioAsset callbackParam = null)
		{
			bool isFading = true;
			while (isFading) {
				if (channelToFadeOut.Volume < assetForVolume.Volume/100) {
					channelToFadeOut.Volume += FADESTRENGHT;
				} else {
					isFading = false;
				}
				yield return new WaitForEndOfFrame ();
			}

			if (callback != null)
				callback (callbackParam);

			yield break;
		}

	}

}
