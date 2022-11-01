using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Projectile : MonoBehaviour
{
    private Player _player;
    [SerializeField] private Vector3 _trackPlayerPOS;
    [SerializeField] private float _distance;
    [SerializeField] GameObject _explosion;
    [SerializeField] private bool _exploding;
    [SerializeField] private float _detonationTimer = 3f;
    [SerializeField] private float _speed;
       
    void Start()
    {
       _player = FindObjectOfType<Player>();
        if( _player == null) 
        {
            Debug.Log("Player Missing");
        }
        StartCoroutine(DetonationTimerRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        ExplosionRadius();
    }

    private void ExplosionRadius()
    {
        _trackPlayerPOS = _player.transform.position;
        _distance = Vector3.Distance(transform.position, _trackPlayerPOS);
        
        transform.Translate(Vector3.down * _speed * Time.deltaTime); 
        if (_distance <= 1) 
        {
            _player.Damage();
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (_exploding)
        {
            if(_distance <= 1.7) 
            { 
                Instantiate(_explosion, transform.position, Quaternion.identity);
                _player.Damage();
                Destroy(gameObject);
            }
            else 
            {
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
            

    IEnumerator DetonationTimerRoutine() 
    { 
        yield return new WaitForSeconds(_detonationTimer);
        _exploding = true;
    }
}
