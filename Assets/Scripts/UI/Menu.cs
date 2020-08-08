
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

    [Header("Configs")]
    [SerializeField]
    private float timeToStartGame = 0.42f;

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
        yield return new WaitForSeconds(timeToStartGame); // Wait for button animation and sound for better UX
        SceneManager.LoadScene(scene);
    }

}
