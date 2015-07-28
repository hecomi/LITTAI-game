using UnityEngine;
using System.Collections.Generic;

public class Sound : MonoBehaviour
{
	static private Sound Instance;

	[System.Serializable]
	public struct Data
	{
		public string name;
		public AudioClip clip;
		public float volume;
	}
	public List<Data> sounds;
	private Dictionary<string, Data> dict_ = new Dictionary<string, Data>();

	void Awake()
	{
		Instance = this; 
	}

	void Start()
	{
		foreach (var sound in sounds) {
			if (sound.name == "") continue;
			Data data;
			data.name = sound.name;
			data.clip = sound.clip;
			data.volume = sound.volume == 0f ? 1f : sound.volume;
			dict_.Add(sound.name, data);
		}
	}

	void PlayImpl(string name)
	{
		if (dict_.ContainsKey(name)) {
			var sound = dict_[name];
			AudioSource.PlayClipAtPoint(
				sound.clip, 
				GlobalObjects.localStage.transform.position, 
				sound.volume);
		} else {
			Debug.LogWarning(name + " does not exist in sound list");
		}
	}

	public static void Play(string name)
	{
		Instance.PlayImpl(name);
	}
}
