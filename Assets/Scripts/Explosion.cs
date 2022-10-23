using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionClip;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null) 
        {
            Debug.Log("Missing Audio Source");
        }
        else 
        { _audioSource.clip = _explosionClip;
          _audioSource.Play();
           Destroy(gameObject,2f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
