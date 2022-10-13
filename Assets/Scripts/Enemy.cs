using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 _enemyDirection = new Vector3(0,-1,0);
    [SerializeField] private float _enemySpawn = 6.8f;
    [SerializeField] private Vector3 _randomSpawn;
    [SerializeField] private float _enemySpeed = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
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
}
