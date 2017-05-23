using UnityEngine;
using System.Collections;

namespace ProefProeve.Audio
{
	public class AudioChannel : MonoBehaviour {
		private AudioAsset _audioAsset;
		private AudioSource _audioSource;

		/// <summary>
		/// Gets a value indicating whether this instance is playing the audiosource.
		/// </summary>
		/// <value><c>true</c> if this instance is playing; otherwise, <c>false</c>.</value>
		public bool IsPlaying
		{
			get{
				if (_audioSource == null)
					return false;
				return _audioSource.isPlaying;
			}
		}

		/// <summary>
		/// Returns value depending if it contains the audioasset or not
		/// </summary>
		/// <returns><c>true</c>, if audio channel contains audioasset, <c>false</c> otherwise.</returns>
		/// <param name="audioAsset">Audio asset.</param>
		public bool ContainsAudioAsset(AudioAsset audioAsset)
		{
			if (_audioAsset == audioAsset) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets or sets the playtime of the audiosource.
		/// </summary>
		/// <value>The playtime.</value>
		public float Playtime
		{
			get{
				return _audioSource.time;
			}
			set{
				_audioSource.time = value;
			}
		}

		/// <summary>
		/// Gets or sets the volume from 0.0 to 1.0.
		/// </summary>
		/// <value>The volume.</value>
		public float Volume
		{
			get{
				return _audioSource.volume;
			}
			set{
				_audioSource.volume = value;
			}
		}

		public void Play(AudioAsset audioAsset, bool loop = false)
		{
			if (_audioSource == null) {
				_audioSource = gameObject.AddComponent<AudioSource> ();
			}

			_audioAsset = audioAsset;
			_audioSource.volume = _audioAsset.Volume / 100; //Audio ranges from 0.0 to 1.0
            _audioSource.loop = loop;
            _audioSource.playOnAwake = false;
            _audioSource.clip = _audioAsset.AudioClip;
            _audioSource.outputAudioMixerGroup = _audioAsset.Group;
            _audioSource.Play();
		}

		public void Stop()
		{
			_audioSource.Stop ();
		}

		public void Pause()
		{
			_audioSource.Pause ();
		}

		public void Resume()
		{
			_audioSource.UnPause ();
		}
	}
}
