using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] GameObject _explosion;
    private Spawn_Mananger _spawnMananger;

    void Start()
    {
        _spawnMananger = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Mananger>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser" || collision.tag == "Rocket") 
        {
            Destroy(collision.gameObject);
            Instantiate(_explosion, transform.position, Quaternion.identity);
            _spawnMananger.StartSpawning();
            Destroy(this.gameObject, 0.25f);
        }
    }

}
