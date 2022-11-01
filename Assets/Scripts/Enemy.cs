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
    [SerializeField] private int _movementID;

    Vector3 _centre;
    private float _radius = 1f;
    private float _angle;
    private float _rotateSpeed = 5f;
    private int _rightOrLeft = 0;

    private Player  _player;
    
    private void Start()
    {
        if (GameObject.Find("Player"))
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            
        }
        else Debug.Log("Player Object Destroyed");
        
        _fireReady = true;
        _centre = (new Vector3(transform.position.x,-0.28f,0f));
        _movementID = Random.Range(0,3);
    }
    void Update()
    {
        PickingMovement();

       if(_fireReady == true) 
        { 
            Shoot();
        }
    }
        
    
   private void PickingMovement() 
    {
        switch (_movementID) 
        { 
            case 0:
                EnemyMovement();
                break;
            case 1:
                SideToSide();
                break;
            case 2:
                SpinInCircle();
                break;
            default:
                EnemyMovement();
                break;
        }
    }
        
        

        
    
   private void EnemyMovement()
    {
        transform.Translate((_enemyDirection) * _enemySpeed * Time.deltaTime);
        if(transform.position.y < -6.5f )
        {
            _randomSpawn = new Vector3(Random.Range(-10f,10f),_enemySpawn,0);
            Destroy(this.gameObject);
        } 
    }


   
   private void SideToSide() 
    {
        switch (_rightOrLeft) 
        {
            case 0: 
                if(transform.position.y > 0.28) 
                {
                    transform.Translate((Vector3.down) * _enemySpeed * Time.deltaTime);
                }
                else
                
                 transform.Translate((Vector3.right) * 10f * Time.deltaTime);
                
                if(transform.position.x > 9.5f) 
                {
                    transform.Translate((Vector3.down) * 20f * Time.deltaTime);
                    _rightOrLeft = 1;
                }

                break;
            case 1:
                if (transform.position.y > 0.28)
                {
                    transform.Translate((Vector3.down) * _enemySpeed * Time.deltaTime);
                }
                else
                    transform.Translate((Vector3.left) * 10f * Time.deltaTime);
                if(transform.position.x < -9.5f) 
                {
                    for (int i = 0; i < 5; i++)
                    {
                        transform.Translate((Vector3.down) * 20f * Time.deltaTime);
                    }
                    _rightOrLeft = 0;
                }
                break;
        
        }
    }

    bool _spinning =false;
    int _totalSpins;
   private void SpinInCircle() 
    {

        if (transform.position.y > -0.28 && !_spinning || _totalSpins == 4) 
        { 
          transform.Translate((Vector3.down) * _enemySpeed * Time.deltaTime);
          _totalSpins = 0;
        }
        
        
        else if (transform.position.y <= -0.28 || _spinning)
        {
            _spinning = true;
            _angle += _rotateSpeed * Time.deltaTime;
            var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle), 0) * _radius;
            transform.position = _centre + offset;
            _totalSpins++;

        }

        else if (transform.position.y < -6.5f)
        {
            _randomSpawn = new Vector3(Random.Range(-10f, 10f), _enemySpawn, 0);
            transform.position = _randomSpawn;
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
        if (_laser == null)
        {
            Debug.Log("Laser Missing");
        }
        else 
        { 
            _laser.gameObject.tag = "Enemy_Laser";
            yield return new WaitForSeconds(0.0001f);;
            _laser.transform.parent = transform.parent;
        }
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
        
    public int GetMovementID() 
    {
        return _movementID;
    }
       



}
