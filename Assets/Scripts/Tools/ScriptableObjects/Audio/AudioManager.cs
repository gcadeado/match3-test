using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Act as a Singleton Dictionary so other objects can access to any sound they want to play.
/// It must be filled in the inspector with all music and sounds.
/// </summary>
[CreateAssetMenu(fileName = "AudioManager", menuName = "Audio/AudioManager")]
public class AudioManager : ScriptableObject
{

    //We remember at runtime the Sound Player in the audioManager to be able to 
    //perform global operation on them (like mute/resume all)

    [SerializeField]
    [ReadOnly]
    private List<AudioSourcePlayer> soundSources = new List<AudioSourcePlayer>();

    #region Public Methods



    public void RegisterAudioSourcePlayer(AudioSourcePlayer player)
    {
        soundSources.Add(player);
    }

    public void UnregisterAudioSourcePlayer(AudioSourcePlayer player)
    {
        soundSources.Remove(player);
    }
    #endregion
}
