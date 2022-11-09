using System.Collections;
using UnityEngine;

public class Bomb_Projectile : MonoBehaviour
{
    private Player _player;
    private Vector3 _trackPlayerPOS;
    private float _distance;
    private bool _exploding;
    [SerializeField] GameObject _explosion;
    
    [Range(1f,4f)]
    [SerializeField] private float _detonationTimer;
    
    [SerializeField] private float _speed;
    
       
    void Start()
    {
       _player = FindObjectOfType<Player>();
        _detonationTimer = Random.Range(1f, 4f);
        if( _player == null) 
        {
            Debug.Log("Player Missing");
        }
        StartCoroutine(DetonationTimerRoutine());
    }

    void Update()
    {
        ExplosionRadius();
        
    }

    private void ExplosionRadius()
    {
        if (_player == null) {Debug.Log("Player Dead"); return; }
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
