using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip cardMatch;
    [SerializeField] private AudioClip cardMismatch;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip gameWin;
    [SerializeField] private AudioClip cardFlip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayButtonSFX() => audioSource.PlayOneShot(buttonClick);
    public void PlayCardMatchSFX() => audioSource.PlayOneShot(cardMatch);
    public void PlayCardMismatchSFX() => audioSource.PlayOneShot(cardMismatch);
    public void PlayGameOverSFX() => audioSource.PlayOneShot(gameOver);
    public void PlayGameWinSFX() => audioSource.PlayOneShot(gameWin);
    public void PlayCardFlipSFX() => audioSource.PlayOneShot(cardFlip);
}
