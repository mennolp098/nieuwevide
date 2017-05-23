using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefProeve.Audio
{
	public class AudioComponentHandler : MonoBehaviour {
		[SerializeField] private AudioComponent[] _components;

		/// <summary>
		/// Gets the first component in handler with type T.
		/// </summary>
		/// <returns>The component in handler.</returns>
		/// <param name="type">Type.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T GetComponentInHandler<T>() where T : AudioComponent
		{
			for (int i = 0; i < _components.Length; i++) {
				if (_components [i] is T) {
					return (T)_components [i];
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the component(s) in handler as type T.
		/// </summary>
		/// <returns>The _components in handler.</returns>
		/// <param name="type">Type.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public List<T> GetComponentsInHandler<T>() where T : AudioComponent
		{
			List<T> result = new List<T> ();
			for (int i = 0; i < _components.Length; i++) {
				if (_components [i] is T) {
					result.Add ((T)_components [i]);
				}
			}

			return result;
		}
	}

}
