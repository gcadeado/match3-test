using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : Singleton<MusicPlayer>
{

    [SerializeField]
    protected AudioManager audioManager = null;

    [SerializeField]
    private Sound mainMusic = null;

    private AudioSourcePlayer audioPlayer = null;

    protected override void OnAwake()
    {
        audioPlayer = AudioSourcePlayer.AddAsComponent(gameObject, audioManager);
    }

    void Start()
    {
        audioPlayer.PlaySound(mainMusic);
    }

}
