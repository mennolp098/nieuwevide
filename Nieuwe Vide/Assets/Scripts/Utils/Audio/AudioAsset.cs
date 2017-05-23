using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.IO;

namespace ProefProeve.Audio
{
	public class AudioAsset : ScriptableObject {

		public string AssetPath {
			get{
				return _path;
			}
		}

		public string Name {
			get {
				return _objectName;
			}
		}

		public float Volume {
			get {
				return _volume;
			}
			set {
				_volume = value;
			}
		}

		public AudioClip AudioClip
		{
			get{
				return _audioClip;
			}
			set{
				_audioClip = value;
			}
		}

		public UnityEngine.Audio.AudioMixerGroup Group
		{
			get{
				return _group;
			}
		}

		[SerializeField] private string _objectName;
		[SerializeField] private string _path;
		[SerializeField] private float _volume;
		[SerializeField] private AudioClip _audioClip;
		[SerializeField] private UnityEngine.Audio.AudioMixerGroup _group;
		[SerializeField] private bool _apply;

	#if UNITY_EDITOR
		[MenuItem("AudioManager/Create New AudioAsset From Selection")]
		public static void CreateInstance()
		{
			if (Selection.objects.Length > 0) {
				AudioAsset[] newAudioAssets = new AudioAsset[Selection.objects.Length];
				for (int i = 0; i < Selection.objects.Length; i++) {
					if (Selection.objects[i] is AudioClip) {
						AudioAsset obj = ScriptableObject.CreateInstance<AudioAsset>();
						obj._audioClip = Selection.objects [i] as AudioClip;
						obj._volume = 100;
						obj.name = obj._audioClip.name;
						obj._objectName = obj._audioClip.name;
						AssetDatabase.CreateAsset (obj, AssetDatabase.GetAssetPath (Selection.objects[i]) + ".asset");
						newAudioAssets [i] = obj;
					}
				}

				AssetDatabase.Refresh ();
				AssetDatabase.SaveAssets ();

				Selection.objects = newAudioAssets;
			}
			else {
				AudioAsset obj = ScriptableObject.CreateInstance<AudioAsset>();
				obj.name = "New AudioAsset";
				obj._volume = 100;
				AssetDatabase.CreateAsset (obj, "Assets/Data/Audio/New Audio Asset.asset");
				Debug.LogWarning ("Error no audiosource was selected while creating the Audio Asset");
				Debug.Log ("Creating empty Audio Asset");

				AssetDatabase.Refresh ();
				AssetDatabase.SaveAssets ();
			}

		}

		private void OnValidate()
		{
			if (_objectName != name) {
				name = _objectName;
			}
			if (_apply) {
				_apply = false;
				string assetPath = AssetDatabase.GetAssetPath (Selection.activeObject);

				_path = AssetDatabase.GetAssetPath (_audioClip);
				string rootFolder = "Assets/Data/Audio/Resources/";
				_path = _path.Substring (rootFolder.Length, _path.Length - rootFolder.Length);

				AssetDatabase.RenameAsset (assetPath, name);
			}
		}
		#endif
	}

}
