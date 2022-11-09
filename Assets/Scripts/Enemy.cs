using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector3 _enemyDirection = new Vector3(0,-1,0);
    private Vector3 _randomSpawn;
    private bool _fireReady;
    [SerializeField] private float _enemySpawn = 6.8f;
    [SerializeField] private float _enemySpeed = 4.0f;
  
    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject laser;
    [SerializeField] private float _coolDownLaser;
    [SerializeField] private int _movementID;
    [SerializeField] private GameObject _enemyShield;
    [SerializeField] private bool _setShield;
    SpriteRenderer _disableShield;

    [SerializeField] private float _shieldPercent = 0.85f;

     private float _distance;
    [SerializeField] private float _attackRange;
    
    Vector3 _centre;
    private float _radius = 1f;
    private float _angle;
    private float _rotateSpeed = 5f;
    private int _rightOrLeft = 0;
    private Player  _player;
    public float _playerPOS;
    private Vector3 _playerDistance;
    private PowerUp[] _powerUpsPOS;
    private Laser[] _laser;
    private bool _destroyPowerUp = false;
    bool _spinning =false;
    int _totalSpins;
    private void Start()
    {
        RandomShieldSpawn();
        if (GameObject.Find("Player"))
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            
        }
        else Debug.Log("Player Object Destroyed");

        if(this.gameObject.name == "Battering_Ram_Enemy" || this.gameObject.name == "Battering_Ram_Enemy(Clone)") 
        {
            _movementID = 3;
        }
        else if (this.gameObject.name == "Smart_Enemy" || this.gameObject.name == "Smart_Enemy(Clone)")
        {
            _movementID = 4;
        }
        else 
        { 
            _movementID = Random.Range(0,3);
        }
        _fireReady = true;
        _centre = (new Vector3(transform.position.x,-0.28f,0f));
    }
    void Update()
    {
        PickingMovement();
        DetectPowerUp();
       if(_fireReady == true) 
        { 
            Shoot();
        }
    }
    public void AdjustShieldChance(float percentage) 
    {
        _shieldPercent -= percentage;
    }
    public void ResetShieldChance(float percentage)
    {
        _shieldPercent = percentage;
    }
    private void RandomShieldSpawn() 
    { 
        if(Random.value > _shieldPercent) 
        { 
            _setShield = true;
            _enemyShield = Instantiate(_enemyShield,new Vector3(transform.position.x,transform.position.y, transform.position.z), Quaternion.identity);
            _enemyShield.transform.parent = this.transform;
           _disableShield = _enemyShield.GetComponent<SpriteRenderer>();
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
            case 3:
                RamAttack();
                break;
            case 4:
                SmartAttack();
                break;
            default:
                EnemyMovement();
                break;
        }
    }

    private void DetectPowerUp()
    {
        _powerUpsPOS = FindObjectsOfType<PowerUp>();
        Vector3 _powerDistance;
        float _infront;
        if (_powerUpsPOS == null)
        {
            Debug.Log("No Power-UPs on Screen");
        }
        foreach (var p in _powerUpsPOS)
        {
            _powerDistance = p.transform.position - this.transform.position;
            Debug.Log(_powerDistance.magnitude + " " + p.name);
            if (_powerDistance.magnitude < 3) 
            { 
                _infront = Vector3.Dot(Vector3.down, _powerDistance.normalized);
                if (_infront <= 1 && _infront >= 0.98)
                {
                    _destroyPowerUp = true;
                    Debug.Log("Destroy Power UP" + " " + _infront);
                }
                else _destroyPowerUp = false;
            }
        }
    }

    private void DetectLaserUp()
    {
        _laser = FindObjectsOfType<Laser>();
        Vector3 _laserDistance;
        float _infront;
       if (_laser == null)
         {
             Debug.Log("No lasers on Screen");
         }
        foreach (var l in _laser)
        {
            if (l.gameObject.tag == "Laser") 
            { 
                _laserDistance = l.transform.position - this.transform.position;
                if (_laserDistance.magnitude < 3)
                {
                    _infront = Vector3.Dot(Vector3.down, _laserDistance.normalized);
                    if (_infront <= 1 && _infront >= 0.88)
                    {
                       this.transform.Translate(Vector2.right * 5 * Time.deltaTime);
                    }
                }
            }
        }
    }

            
            

    public bool DestroyPowerUp() 
    {
       return _destroyPowerUp;
    }
            
            
            

    private void SmartAttack()
    {
        DetectLaserUp();
        Vector3 _enemyEyes = Vector3.down;
        _playerDistance = _player.transform.position - this.transform.position;
        _playerPOS = Vector3.Dot(_enemyEyes, _playerDistance.normalized);
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        if(_playerPOS < 0) 
        {
            this.gameObject.tag = "Smart_Enemy";
            if (_fireReady == true)
            {
                Shoot();
            }
        }
        else

        {
            this.gameObject.tag = "Enemy";
            if (_fireReady == true)
            {
               Shoot(); 
            }
        }
    }
    private void RamAttack()
    {
        if (_player == null)
        {
            return;
        }
      
        _distance = Vector3.Distance(_player.transform.position, this.transform.position);

       if(_distance < 3) 
        {
            transform.Translate((_player.transform.position - transform.position).normalized * _enemySpeed * Time.deltaTime);
        }
        else 
        {
            transform.Translate((Vector3.down) * _enemySpeed * Time.deltaTime);
            _randomSpawn = new Vector3(Random.Range(-10f, 10f), _enemySpawn, 0);
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

        if (other.gameObject.tag == "Laser" || other.gameObject.tag == "Rocket")
        {
            if (_player != null && !_setShield)
            {
                _enemySpeed = 0;
                _player.AddScore(Random.Range(0, 50));
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                Destroy(this.gameObject);

            }
            else
            {
                Destroy(other.gameObject);
                _setShield = false;
                _disableShield.enabled = false;
            }
        }


        else if (other.gameObject.tag == "Player")
        {
            if (_player != null && !_setShield)
            {
                _enemySpeed = 0;
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                _player.Damage();
            }
            else
            {
                _player.Damage();
                _setShield = false;
                _disableShield.enabled = false;
            }


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
