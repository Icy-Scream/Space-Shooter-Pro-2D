using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 _enemyDirection = new Vector3(0, -1, 0);
    [SerializeField] private float _enemySpawn = 6.8f;
    [SerializeField] private Vector3 _randomSpawn;
    [SerializeField] private float _enemySpeed = 4.0f;
    [SerializeField] private GameObject _explosion;
    private Player _player;

    private void Start()
    {
        if (GameObject.Find("Player"))
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
        }
        else Debug.Log("Player Object Destroyed");

    }
    void Update()
    {
        EnemyMovement();
    }

    private void EnemyMovement()
    {
        transform.Translate((_enemyDirection) * _enemySpeed * Time.deltaTime);
        if (transform.position.y < -6.5f)
        {
            _randomSpawn = new Vector3(Random.Range(-11.18f, 11.18f), _enemySpawn, 0);
            transform.position = _randomSpawn;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {



        if (other.gameObject.tag == "Laser")
        {
            if (_player != null)
            {
                _enemySpeed = 0;
                _player.AddScore(Random.Range(0, 50));
                ExplosionEffect();
                Destroy(other.gameObject);
                Destroy(this.gameObject,0.5f);
            }
            else
            {
                Debug.Log("Missing Player Object");
            }
        }


        else if (other.gameObject.tag == "Player")
        {
            if (_player != null)
            {
                _enemySpeed = 0;
                ExplosionEffect();
                Destroy(this.gameObject,0.5f);
                _player.Damage();
            }
            else
                Debug.Log("COMPONENT MISSING");

        }
    }

    private void ExplosionEffect() 
    {
        Instantiate(_explosion, this.transform.position, Quaternion.identity);
    }

}
