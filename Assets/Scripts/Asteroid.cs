using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] GameObject _explosion;
    private Spawn_Mananger _spawnMananger;
    // Start is called before the first frame update
    void Start()
    {
        _spawnMananger = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Mananger>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser") 
        {
            Destroy(collision.gameObject);
           GameObject _explosionPrefab = Instantiate(_explosion, transform.position, quaternion.identity);
            _spawnMananger.StartSpawning();
            Destroy(this.gameObject, 0.25f);
            Destroy(_explosionPrefab, 3);
        }
    }

}
