using System.Collections;
using UnityEngine;

public class Spawn_Mananger : MonoBehaviour
{
    private bool _startTimer = false;
    private bool _stopspawing = false;
    private Vector3 _spawnPOS;
    
    [SerializeField] private float _difficultyTimer;
    [SerializeField] private bool _increaseDifficulty;
    [SerializeField] private int _difficultyLvl = 1;
    
    [SerializeField] GameObject[] _enemy;
    [SerializeField] private int _enemySpawnRateMax = 4;
    [SerializeField] private int _enemySpawnRate = 1;
    [SerializeField] float _spawnEnemyTimer;
    [SerializeField] private int _newEnemy = 0;
    [SerializeField] GameObject _enemyContainer;
    
    [SerializeField] GameObject[] _powerUps;
    [SerializeField] float _rarePowerUpTimer;
    [SerializeField] float _powerUpTimer = 5f;

    private void Update()
    {
        IncreaseDifficulty();
    }
    public void StartSpawning()
    {
        resetShieldPercent();
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUp());
        StartCoroutine(RareSpawnPowerUp());
        StartTimer();
    }

    IEnumerator SpawnEnemyRoutine()
    {

        yield return new WaitForSeconds(3);
        while (_stopspawing == false)
        {
            while (!_increaseDifficulty)
            {
                yield return new WaitForSeconds(_spawnEnemyTimer);
                for (int i = 0; i < _enemySpawnRate; i++)
                {
                    _spawnPOS = new Vector3(Random.Range(-9.0f, 9.0f), Random.Range(5f, 9.0f), 0);
                    if (Random.value > 0.5)
                    {
                        GameObject newEnemy = Instantiate(_enemy[Random.Range(0, _newEnemy + 1)], _spawnPOS, Quaternion.identity);
                        newEnemy.transform.parent = _enemyContainer.transform;

                    }
                    else
                    {
                        GameObject newEnemy = Instantiate(_enemy[0], _spawnPOS, Quaternion.identity);
                        newEnemy.transform.parent = _enemyContainer.transform;
                    }
                }
            }
            _difficultyLvl++;
            
            if(_enemySpawnRate <= _enemySpawnRateMax) 
            { 
                _enemySpawnRate++;
            }
            
            if(_difficultyLvl < 6) 
            { 
                for(int i = 0; i <= 1; i++) 
                    { 
                     _enemy[i].GetComponent<Enemy>().AdjustShieldChance(0.1f);
                    }
            }
            else if(_difficultyLvl == 7) 
            { 
               Instantiate(_enemy[4], _spawnPOS, Quaternion.identity);
            }
                
          
            if (_difficultyLvl == 3 ||  _difficultyLvl == 4 || _difficultyLvl == 5)
            {
                _newEnemy++;
            }
            
            _increaseDifficulty = false;
        }
    }
    private void resetShieldPercent()
    {
        for (int i = 0; i <= 1; i++)
        {
            _enemy[i].GetComponent<Enemy>().ResetShieldChance(1f);
        }
    }
    private void StartTimer()
    {
        _startTimer = true;
        _difficultyTimer = Time.time + 30f;
    }   

    private void IncreaseDifficulty() 
    {
      if(Time.time > _difficultyTimer && _startTimer) 
        {
            _increaseDifficulty = true;
            _difficultyTimer = Time.time + 30f;
        }
    }
    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(3);
        while (_stopspawing == false)
        {
           yield return new WaitForSeconds(_powerUpTimer);
           if (Random.value > 0.6) //%50 percent chance
            {
                int _powerUpsID = Random.Range(0,4);
                _spawnPOS = new Vector3(Random.Range(-10.0f, 10.0f), 6.8f, 0);
                Instantiate(_powerUps[_powerUpsID], _spawnPOS, Quaternion.identity);
            }
           if (Random.value > 0.4) //%80 percent chance (1 - 0.2 is 0.8)
            {
                _spawnPOS = new Vector3(Random.Range(-10.0f, 10.0f), 6.8f, 0);
                Instantiate(_powerUps[3], _spawnPOS, Quaternion.identity);
            }
           if (Random.value > 0.9) //%30 percent chance (1 - 0.7 is 0.3)
            { //code here
                _spawnPOS = new Vector3(Random.Range(-10.0f, 10.0f), 6.8f, 0);
                Instantiate(_powerUps[4], _spawnPOS, Quaternion.identity);
            }



;



        }
    }

    IEnumerator RareSpawnPowerUp() 
    {
        yield return new WaitForSeconds(3);
        while (_stopspawing == false)
        {
            _rarePowerUpTimer = Random.Range(30.0f,60.0f);
            yield return new WaitForSeconds(_rarePowerUpTimer);
            if (Random.value > 0.2) //%80 percent chance (1 - 0.2 is 0.8)
            {
                _spawnPOS = new Vector3(Random.Range(-10.0f, 10.0f), 6.8f, 0);
                Instantiate(_powerUps[5], _spawnPOS, Quaternion.identity);
            }
            else 
            { 
                _spawnPOS = new Vector3(Random.Range(-10.0f, 10.0f), 6.8f, 0);
                Instantiate(_powerUps[6], _spawnPOS, Quaternion.identity);
            
            }
               
        }

    }


    public void OnPlayerDeath()
    {
        _stopspawing = true;
    }
}
