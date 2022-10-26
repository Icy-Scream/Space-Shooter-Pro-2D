using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Mananger : MonoBehaviour
{
    [SerializeField] GameObject _enemy;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject[] _powerUps;
    [SerializeField] float _spawnTimer;
    [SerializeField] float _powerUpTimer;
    [SerializeField] private bool _stopspawing = false;
    
    private Vector3 _spawnPOS;

    public void StartSpawning() 
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUp());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3);
        while(_stopspawing == false)
        {
            yield return new WaitForSeconds(_spawnTimer);
           _spawnPOS = new Vector3(Random.Range(-9.0f,9.0f),6.8f,0);
        
            GameObject newEnemy = Instantiate(_enemy,_spawnPOS, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }

    }

    IEnumerator SpawnPowerUp() 
    {
        yield return new WaitForSeconds(3);
        while (_stopspawing == false) 
        {
            _powerUpTimer = Random.Range(3.0f, 7.0f);
            yield return new WaitForSeconds(_powerUpTimer);
            _spawnPOS = new Vector3(Random.Range(-10.0f, 10.0f), 6.8f, 0);
            Instantiate(_powerUps[Random.Range(0, _powerUps.Length)],_spawnPOS, Quaternion.identity);
        }
    }
    public void OnPlayerDeath()
    {
        _stopspawing = true;
    }
}
