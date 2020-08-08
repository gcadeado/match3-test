
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    private AudioSourcePlayer audioPlayer = null;

    [Header("Audio")]
    [SerializeField]
    private AudioManager audioManager = null;

    [SerializeField]
    private Sound selectSFX = null;

    private void Awake()
    {
        audioPlayer = AudioSourcePlayer.AddAsComponent(gameObject, audioManager);
    }

    public void PlayGame()
    {
        audioPlayer.PlaySound(selectSFX);
        StartCoroutine(ChangeScene("Playground"));
    }

    IEnumerator ChangeScene(string scene)
    {
        yield return new WaitForSeconds(0.333f);
        SceneManager.LoadScene(scene);
    }

}
