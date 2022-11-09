
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserAudioClip;
    [SerializeField] private AudioClip _powerUpAudioClip;
    [SerializeField] private AudioClip _lowAmmoClip;
    [SerializeField] private AudioClip _collectLivesClip;
    [SerializeField] private AudioClip _ammoReload;
    [SerializeField] private AudioClip _rocketFire;
    public void PlayLaserClip() 
    { 
        _audioSource.clip = _laserAudioClip;
        _audioSource.Play();
    }
    public void PlayPowerUpClip()
    {
        _audioSource.clip = _powerUpAudioClip;
        _audioSource.Play();
    }
    public void PlayLowAmmoClip()
    {
        _audioSource.clip = _lowAmmoClip;
        _audioSource.Play();
    }
    public void PlayCollectLivesClip()
    {
        _audioSource.clip = _collectLivesClip;
        _audioSource.Play();
    }
    public void PlayAmmoReloadClip()
    {
        _audioSource.clip = _ammoReload;
        _audioSource.Play();
    }

    public void RocketFireClip() 
    {
        _audioSource.clip = _rocketFire;
        _audioSource.Play();
    }

}
