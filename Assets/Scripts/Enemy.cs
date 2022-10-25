using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 _enemyDirection = new Vector3(0,-1,0);
    [SerializeField] private float _enemySpawn = 6.8f;
    [SerializeField] private Vector3 _randomSpawn;
    [SerializeField] private float _enemySpeed = 4.0f;
    private Player  _player;
    [SerializeField] private GameObject _laser;
    [SerializeField] GameObject _explosion;
    [SerializeField] private float _coolDownLaser;
    [SerializeField] private bool _fireReady;
    private void Start()
    {
        if (GameObject.Find("Player"))
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            
        }
        else Debug.Log("Player Object Destroyed");
        _fireReady = true;
    }
    void Update()
    {
        EnemyMovement();
        if(_fireReady == true) 
        { 
            Shoot();
        }
        
    }

    private void EnemyMovement()
    {
        transform.Translate((_enemyDirection) *_enemySpeed * Time.deltaTime);
        if(transform.position.y < -6.5f )
        {
            _randomSpawn = new Vector3(Random.Range(-11.18f,11.18f),_enemySpawn,0);
            transform.position =_randomSpawn;
        } 
    }

    private void Shoot() 
    {
        Debug.Log("PEW PEW ENEMY");
        StartCoroutine(LaserParentChangeRoutine());
        _fireReady = false;
        StartCoroutine(ShootCoolDownRoutine());
    }

    IEnumerator LaserParentChangeRoutine()
    {
       GameObject laser = Instantiate(_laser, transform.position, Quaternion.identity, this.transform);
        laser.gameObject.tag = "Enemy_Laser";
        yield return new WaitForSeconds(0.0001f);;
        laser.transform.parent = transform.parent;
    }





    private void OnTriggerEnter2D(Collider2D other) 
{
    
  
    
    if(other.gameObject.tag == "Laser")
    {
        if(_player != null)
            {
                _enemySpeed = 0;
                _player.AddScore(Random.Range(0,50));
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                Destroy(this.gameObject);
                
            }
            else
            {
                Debug.Log("Missing Player Object");
            }
    }
            
            
    else if(other.gameObject.tag == "Player")
    {
        if(_player != null)
        {
          _enemySpeed = 0;
          Instantiate(_explosion, transform.position, Quaternion.identity);
          Destroy(this.gameObject);
          _player.Damage();
        }
        else
        Debug.Log("COMPONENT MISSING");
            
    }
}

    IEnumerator ShootCoolDownRoutine() 
    {
        yield return new WaitForSeconds(_coolDownLaser);
        _fireReady = true;
    }
        

       



}
