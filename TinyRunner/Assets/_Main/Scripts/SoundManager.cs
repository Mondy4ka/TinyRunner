using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioSource _sfxSource;

    [SerializeField] private AudioClip _switchClip;
    [SerializeField] private AudioClip _jumpClip;
}