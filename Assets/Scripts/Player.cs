using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] GameObject _explosion;
    [SerializeField] Sprite _redDamageShip;
    [SerializeField] Sprite _greenShip;
     
    private int _score = 0;
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _playerHealth;
    [SerializeField] private int _lives = 3;
     
    [SerializeField] private GameObject _rocket;
    bool _setRockets = false;
   
    private bool _fireWeapon;
    [SerializeField] private float _fireRate;
    [SerializeField] private int _ammoCount = 15;
    [SerializeField] private int _totalAmmo = 15;
     
     
    [SerializeField] private GameObject _tripleShotPrefab;
    private bool _isTripleShotEnabled = false;
    private float _tripleShotDuration = 5.0f;

    private bool _isSpeedBoostEnabled = false;
    private float _speedBoost = 5.0f;
    private float _speedBoostDuration = 3.0f;

    private bool _isShieldsEnabled = false;
    private int _shieldLevel;
    
    [SerializeField] private float _gas = 10f;
    [SerializeField] private bool _setthrust = true;
    [SerializeField] private bool _refill;

    [SerializeField] GameObject _audioManagerObject;
    private Audio_Manager _audioManager;
  
    private UIManager _uiManager;
    private Spawn_Mananger _spawnScript;
    
    private Coroutine _stopKeyup;

    private Animator _cameraShake;

    void Start()
    {
        _spawnScript = GameObject.FindObjectOfType<Spawn_Mananger>();
        _cameraShake = GameObject.FindObjectOfType<Camera>().GetComponent<Animator>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uiManager.ThrusterSlider(_gas);
        _audioManager = _audioManagerObject.GetComponent<Audio_Manager>();
        transform.position = new Vector3(0,-5,0);
       
        _fireWeapon = true;
       

        if (_cameraShake == null) 
        {
            Debug.Log("MISSING CAMERA");
        }
       
        if (_spawnScript == null)
        {
            Debug.Log("THE SPAWNMANGER IS NULL!! MISSSSIINNG ERRROOORR");
        }
        if (_audioManager == null)
        {
            Debug.Log("Audio MISSING");
        }

        if ( _uiManager == null) 
        {
            Debug.Log("Missing UI Manager");
        }

    }

    void Update()
    {
        PlayerBoundaries();
        PlayerAxisMove();
        Shoot();
        Thruster();
    }
    
   
    public void SetRocket() 
    {
        _setRockets = true;
        StartCoroutine(RocketPowerDownRoutine());
    }
    private void PlayerBoundaries()
    {
        if(transform.position.y >= 0)
        {
            transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
        }
        else if (transform.position.y <= -5)
        {
            transform.position = new Vector3(transform.position.x, -5,0);
        }

        if(transform.position.x >= 11.5f)
        {
            transform.position = new Vector3(-11.5f,transform.position.y,0);
        }
        else if(transform.position.x <= -11.5f)
        {
            transform.position = new Vector3(11.5f, transform.position.y, 0);
        }
    }
    private void PlayerAxisMove()
    {
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");
        Vector3 _direction = new Vector3(_horizontalInput,_verticalInput,0);
        
        if(_isSpeedBoostEnabled == true)
        {
            transform.Translate(_direction * (_playerSpeed + _speedBoost) * Time.deltaTime);
        }

        else 
        { 
            transform.Translate(_direction * _playerSpeed * Time.deltaTime);
        }
    }
    private void Thruster() 
    {
        if (Input.GetKey(KeyCode.LeftShift) && _setthrust)
        {
             _refill = true;
            _setthrust = false;
            _playerSpeed = 10;
            _stopKeyup = StartCoroutine(ThrusterCoolDown());
        }
           
        else if( (Input.GetKeyUp(KeyCode.LeftShift) && _refill ) || (_gas < 1 && _refill)) 
        { 
          _refill = false;  
         _playerSpeed = 8;
         StopCoroutine(_stopKeyup);
         StartCoroutine(ThrusterRoutine()); 
        }
    }
        

    
    IEnumerator ThrusterCoolDown() 
    {
        _refill = true;
        while(_gas >= 1) 
        {
         yield return new WaitForSeconds(0.2f);
         _gas-= 0.2f;
         _uiManager.ThrusterSlider(_gas);
        }
    }
         
    IEnumerator ThrusterRoutine() 
    {
        while (_gas < 10) 
        { 
          yield return new WaitForSeconds(0.2f);
          _gas += 0.2f;
          _uiManager.ThrusterSlider(_gas);
        }
          if (_gas == 10) { _setthrust = true; }
        
    }
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _fireWeapon)
        {
            if (_isTripleShotEnabled)
            {
                StartCoroutine(LaserParentChangeRoutine());
                _fireWeapon = false;
                _audioManager.PlayLaserClip();
                StartCoroutine(DelayFireRateRoutine());
                _ammoCount--;
                LowAmmoAudio();
            }
            else if(_setRockets)
            {
                _audioManager.RocketFireClip();
                Instantiate(_rocket, transform.position, Quaternion.identity);
                _fireWeapon = false;
                StartCoroutine(DelayFireRateRoutine());
            }
            else 
            { 
               StartCoroutine(LaserParentChangeRoutine());
               _fireWeapon = false;
;              _audioManager.PlayLaserClip();
               StartCoroutine(DelayFireRateRoutine());
               _ammoCount--;
               LowAmmoAudio();
            }
        }
       else if (_ammoCount == 0) 
        {
            _fireWeapon = false;
            if (Input.GetKeyDown(KeyCode.R)) 
            {
                _audioManager.PlayAmmoReloadClip();
                _ammoCount = _totalAmmo;
                _fireWeapon = true;
            }
        }
    }
    private void LowAmmoAudio()
    {
        if (_ammoCount == 0) 
        {
            _audioManager.PlayLowAmmoClip();
        }
    }
            
    public void Damage()
    {
        _cameraShake.SetTrigger("CameraShake");
        
        if (_isShieldsEnabled) 
        {
            GameObject _shield = transform.GetChild(0).gameObject;
            switch (_shieldLevel) 
            {
                case 1:
                    _shield.GetComponent<SpriteRenderer>().enabled = false;
                    _shieldLevel--;
                    _isShieldsEnabled = false;
                    return;
                  
                case 2:
                    _shield.GetComponent<SpriteRenderer>().color = Color.white;
                    _shieldLevel--;
                    return;
                   
                case 3:
                    _shield.GetComponent<SpriteRenderer>().color = Color.red;
                    _shieldLevel--;
                    return;
           
                default:
                    break;
            }
        }   
        _playerHealth += -20;
        StartCoroutine(FlashRedCourtine());
        
        if (_playerHealth < 1)
        {  
            _lives--;
            
            _uiManager.UpdateLives(_lives);
            
            if(_lives < 1)
            {
                _spawnScript.OnPlayerDeath();
                _uiManager.GameOver();
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                _playerHealth = 0;
            }
            else _playerHealth = 20;
        }
        
    }
            
            
            
    public void SetTripleShot() 
    {
        _audioManager.PlayPowerUpClip();
        _isTripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    public void SetSpeedBoost() 
    {
        _audioManager.PlayPowerUpClip();
        _isSpeedBoostEnabled = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
        
    }
    public void SetShield() 
    {
        GameObject _shield = transform.GetChild(0).gameObject;
        _isShieldsEnabled = true;
        
        switch (_shieldLevel) 
        {
            case 0:
                _audioManager.PlayPowerUpClip();
                _isShieldsEnabled = true;
                _shield.GetComponent<SpriteRenderer>().enabled = true;
                _shieldLevel++;
                break;
            
            case 1:
                _audioManager.PlayPowerUpClip(); ;
                _isShieldsEnabled = true;
                _shield.GetComponent<SpriteRenderer>().color = Color.red;
                _shieldLevel++;
                break;

            case 2:
                _audioManager.PlayPowerUpClip();
                _isShieldsEnabled = true;
                _shield.GetComponent<SpriteRenderer>().color = Color.yellow;
                _shieldLevel++;
                break;
          default:
               if(IsShieldsOn() == false) 
                { 
                _shieldLevel = 0;
                }
                else {_uiManager.MaxShields(); return; }
                break;
        }
               
    }
    public bool IsShieldsOn()
    {
       return _isShieldsEnabled;
    }
    public void AddScore(int points) 
    {
        _score += points;
    }
    public int GetScore() 
    {
        return _score;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy_Laser") 
        {
            Damage();
            Destroy(collision.gameObject);
        }
    }

    public int GetCurrentAmmo() 
    {
        return _ammoCount;
    }
    public void SetCurrentAmmo() 
    {
        _ammoCount = _totalAmmo;
        _audioManager.PlayAmmoReloadClip();
        if (_fireWeapon == false)
        {
            _fireWeapon = true;
        }
    }
    public int GetTotalAmmo() 
    {
        return _totalAmmo;
    }

    public void AddLife() 
    {
        if(_lives == 3) 
        {
            _uiManager.MaxLives();
            _audioManager.PlayCollectLivesClip();
            return;
        }
        else
        _lives++;
        _uiManager.AddLives(_lives);
        _audioManager.PlayCollectLivesClip();
        StartCoroutine(FlashGreenCourtine());
    }

    IEnumerator LaserParentChangeRoutine()
    {
        Vector3 _laserpos = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        if (_isTripleShotEnabled)
        {
            GameObject _tripleShot = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity, this.transform);
            yield return new WaitForSeconds(0.0001f);
            _tripleShot.transform.parent = transform.parent;
           for(int i = 0; i < _tripleShot.transform.childCount; i++) 
            { 
                _tripleShot.transform.GetChild(i).tag = "Laser";
            }
            Destroy(_tripleShot, 3);
        }
        else
        {
            GameObject _laser = Instantiate(_laserPrefab, _laserpos, Quaternion.identity, this.transform);
            yield return new WaitForSeconds(0.0001f);
            _laser.gameObject.tag = "Laser";
            _laser.transform.parent = transform.parent;
        }
    }       

    IEnumerator DelayFireRateRoutine()
    {
        yield return new WaitForSeconds(_fireRate);
        _fireWeapon = true;
    }
    IEnumerator FlashRedCourtine()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = _redDamageShip;
        yield return new WaitForSeconds(0.3f);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = _greenShip;
    }
    IEnumerator FlashGreenCourtine()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(0.3f);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    IEnumerator TripleShotPowerDownRoutine() 
    {
        yield return new WaitForSeconds(_tripleShotDuration);
        _isTripleShotEnabled = false;
    }

    IEnumerator RocketPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _setRockets = false;
    }

    IEnumerator SpeedBoostPowerDownRoutine() 
    {
        yield return new WaitForSeconds(_speedBoostDuration);
        _isSpeedBoostEnabled = false;
    }
}