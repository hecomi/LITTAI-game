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

	void PlayImpl(string name, Vector3 pos)
	{
		if (dict_.ContainsKey(name)) {
			var sound = dict_[name];
			var obj = new GameObject(name);
			obj.transform.position = pos;
			obj.transform.SetParent(transform);
			var source = obj.AddComponent<AudioSource>();
			source.spatialBlend = 1f;
			source.rolloffMode = AudioRolloffMode.Logarithmic;
			source.PlayOneShot(sound.clip, sound.volume);
			Destroy(obj, sound.clip.length + 1f);
		} else {
			Debug.LogWarning(name + " does not exist in sound list");
		}
	}

	public static void Play(string name)
	{
		if (SystemInfo.graphicsDeviceID == 0) return;

		Instance.PlayImpl(name, Camera.main.transform.position);
	}

	public static void Play(string name, Vector3 pos)
	{
		if (SystemInfo.graphicsDeviceID == 0) return;

		Instance.PlayImpl(name, pos);
	}
}
