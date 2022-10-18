using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Mananger : MonoBehaviour
{
    [SerializeField] GameObject _enemy;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] float _spawnTimer = 5;
    [SerializeField] private Vector3 _spawnPOS;

    [SerializeField] private bool _stopspawing = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnEnemyRoutine()
    {
        while(_stopspawing == false)
        {
            yield return new WaitForSeconds(_spawnTimer);
           _spawnPOS = new Vector3(Random.Range(-10.0f,10.0f),6.8f,0);
        
            GameObject newEnemy = Instantiate(_enemy,_spawnPOS, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }

    }

    public void OnPlayerDeath()
    {
        _stopspawing = true;
    }
}
