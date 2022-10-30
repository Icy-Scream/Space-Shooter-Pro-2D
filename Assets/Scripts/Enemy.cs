using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 _enemyDirection = new Vector3(0,-1,0);
    [SerializeField] private float _enemySpawn = 6.8f;
    [SerializeField] private Vector3 _randomSpawn;
    [SerializeField] private float _enemySpeed = 4.0f;
  
    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject laser;
    [SerializeField] private float _coolDownLaser;
    [SerializeField] private bool _fireReady;
   
    private Player  _player;
    
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
        //SideToSide();
        //EnemyMovement();
        SpinInCircle();

       if(_fireReady == true) 
        { 
            Shoot();
        }
        
    }

   private void EnemyMovement()
    {
        transform.Translate((_enemyDirection) * _enemySpeed * Time.deltaTime);
        if(transform.position.y < -6.5f )
        {
            _randomSpawn = new Vector3(Random.Range(-10f,10f),_enemySpawn,0);
            transform.position =_randomSpawn;
        } 
    }


   int _rightOrLeft = 0;
   private void SideToSide() 
    {
        switch (_rightOrLeft) 
        {
            case 0: transform.Translate((Vector3.right) * _enemySpeed * Time.deltaTime);
                if(transform.position.x > 9.5f) 
                {
                  transform.Translate((Vector3.down) * 20f * Time.deltaTime);
                  _rightOrLeft = 1;
                }
                break;
            case 1: transform.Translate((Vector3.left) * _enemySpeed * Time.deltaTime);
                if(transform.position.x < -9.5f) 
                {
                    transform.Translate((Vector3.down) * 20f * Time.deltaTime);
                    _rightOrLeft = 0;
                }
                break;
        
        }
    }

            float angle = 50;
   private void SpinInCircle() 
    {
        transform.Translate((Vector3.down) * _enemySpeed * Time.deltaTime);
        if(transform.position.y <= -0.28) 
        {
            float x, y;
            float radius = 50;


            x = radius * Mathf.Cos(angle);
            y = radius * Mathf.Sin(angle);

            transform.Translate(new Vector2(x, y) * _enemySpeed * Time.deltaTime);
            angle += 1;
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
       GameObject _laser = Instantiate(laser, transform.position, Quaternion.identity, this.transform);
        _laser.gameObject.tag = "Enemy_Laser";
        yield return new WaitForSeconds(0.0001f);;
        _laser.transform.parent = transform.parent;
    }





    private void OnTriggerEnter2D(Collider2D other) 
    {

    if(other.gameObject.tag == "Laser" || other.gameObject.tag == "Rocket")
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
