using UnityEngine;
using System.Collections;

namespace ll
{
	public class GarbageCollector : MonoBehaviour {
		[Tooltip("How long will it take before removing unused assets")]
		[SerializeField] private float intervalGarbageCollection;

		private float timer;

		private void Update () 
		{
			if (timer < Time.time) {
				Resources.UnloadUnusedAssets ();
				timer = Time.time + intervalGarbageCollection;
			}
		}
	}

}
