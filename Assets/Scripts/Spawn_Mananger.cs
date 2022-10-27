using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Mananger : MonoBehaviour
{
    [SerializeField] GameObject _enemy;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject[] _powerUps;
    [SerializeField] float _spawnEnemyTimer;
    [SerializeField] float _rarePowerUpTimer;
    [SerializeField] float _powerUpTimer;
    [SerializeField] private bool _stopspawing = false;

    private Vector3 _spawnPOS;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUp());
        StartCoroutine(RareSpawnPowerUp());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3);
        while (_stopspawing == false)
        {
            yield return new WaitForSeconds(_spawnEnemyTimer);
            _spawnPOS = new Vector3(Random.Range(-9.0f, 9.0f), 6.8f, 0);

            GameObject newEnemy = Instantiate(_enemy, _spawnPOS, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }

    }

    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(3);
        while (_stopspawing == false)
        {
            _powerUpTimer = Random.Range(3.0f, 7.0f);

            int _powerUpsID = Random.Range(0, _powerUps.Length - 1);

            yield return new WaitForSeconds(_powerUpTimer);

            _spawnPOS = new Vector3(Random.Range(-10.0f, 10.0f), 6.8f, 0);

            Instantiate(_powerUps[_powerUpsID], _spawnPOS, Quaternion.identity);
        }
    }

    IEnumerator RareSpawnPowerUp() 
    {
        yield return new WaitForSeconds(3);
        while (_stopspawing == false)
        {

            _rarePowerUpTimer = Random.Range(30.0f,60.0f);
            int _powerUpsID = 5;
            yield return new WaitForSeconds(_rarePowerUpTimer);
            _spawnPOS = new Vector3(Random.Range(-10.0f, 10.0f), 6.8f, 0);
            Instantiate(_powerUps[_powerUpsID], _spawnPOS, Quaternion.identity);
        }

    }


    public void OnPlayerDeath()
    {
        _stopspawing = true;
    }
}
