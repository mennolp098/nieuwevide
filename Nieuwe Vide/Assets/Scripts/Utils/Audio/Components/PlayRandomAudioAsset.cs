using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefProeve.Audio
{
	public class PlayRandomAudioAsset : AudioComponent 
	{
		[SerializeField] private AudioAsset[] _assets;

		public void SetAudioAssets(AudioAsset[] audioAssets)
		{
            _assets = audioAssets;
		}

		public void Play(bool loop = false, bool dupplicateAllowed = false)
		{
			AudioManager.Instance.Play(_assets[Random.Range(0,_assets.Length-1)], loop, dupplicateAllowed);
		}
	}

}
