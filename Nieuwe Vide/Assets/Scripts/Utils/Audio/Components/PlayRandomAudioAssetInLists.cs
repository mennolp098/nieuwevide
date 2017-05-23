using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefProeve.Audio{
	public class PlayRandomAudioAssetInLists : AudioComponent {
		[System.Serializable]
		private struct LinkedAudioAssets
		{
			public string audioAssetName;
			public AudioAsset[] linkedAudioAsset;
		}

		[SerializeField] private List<LinkedAudioAssets> _linkedAudioAssets = new List<LinkedAudioAssets>();

		private Dictionary<string,AudioAsset[]> audioAssets = new Dictionary<string, AudioAsset[]>();
		private bool isInitialized = false;

		private bool Init()
		{
			if (isInitialized)
				return true;

			// Adding everything in a simple to use dictionary and clearing the list.
			for (int i = _linkedAudioAssets.Count-1; i >= 0; i--) {
				audioAssets.Add (_linkedAudioAssets [i].audioAssetName, _linkedAudioAssets [i].linkedAudioAsset);
			}

			if (_linkedAudioAssets.Count == 0 || audioAssets.Count == 0) {
				Debug.LogWarning ("Something went wrong in the conversion of list to dictionary please revisit code!");
				return false;
			}

			isInitialized = true;
			return true;
		}

		/// <summary>
		/// Plays a random audio inside list with linked name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="loop">If set to <c>true</c> loop audio.</param>
		/// <param name="dupplicateAllowed">If set to <c>true</c> dupplicate audio playing allowed.</param>
		public void PlayRandomAudio(string name, bool loop = false, bool dupplicateAllowed = false)
		{
			if (!Init ())
				return;

			AudioAsset audioToPlay = audioAssets [name] [Random.Range (0, audioAssets [name].Length - 1)];
			AudioManager.Instance.Play(audioToPlay, loop, dupplicateAllowed);
		}

		/// <summary>
		/// Plays a random audio inside list with ID of assets in list.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="loop">If set to <c>true</c> loop audio.</param>
		/// <param name="dupplicateAllowed">If set to <c>true</c> dupplicate audio playing allowed.</param>
		public void PlayRandomAudio(int listID, bool loop = false, bool dupplicateAllowed = false)
		{
			if (!Init ())
				return;

			string key = _linkedAudioAssets [listID].audioAssetName;
			AudioAsset audioToPlay = audioAssets [key] [Random.Range (0, audioAssets [key].Length - 1)];
			AudioManager.Instance.Play(audioToPlay, loop, dupplicateAllowed);
		}
	}

}
