using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
     [SerializeField] private GameObject _laserPrefab;
     [SerializeField] private float _playerSpeed;
     [SerializeField] private float _playerHealth;
                      private int _score = 0;
     [SerializeField] private int _lives = 3;
     
     [SerializeField] private GameObject _rocket;
     [SerializeField] bool _setRockets = false;
   
    [SerializeField] private bool _fireWeapon;
    [SerializeField] private float _fireRate;
    [SerializeField] private int _ammoCount = 15;
    [SerializeField] private int _totalAmmo = 15;
     
     
     private bool _isTripleShotEnabled = false;
    [SerializeField] private GameObject _tripleShotPrefab;
     private float _tripleShotDuration = 5.0f;

     private bool _isSpeedBoostEnabled = false;
     private float _speedBoost = 5.0f;
     private float _speedBoostDuration = 3.0f;

    [SerializeField] private bool _isShieldsEnabled = false;
    [SerializeField] private int _shieldLevel;
      
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserAudioClip;
    [SerializeField] private AudioClip _powerUpAudioClip;
    [SerializeField] private AudioClip _lowAmmoClip;
    [SerializeField] private AudioClip _collectLivesClip;
    [SerializeField] private AudioClip _ammoReload;

    private UIManager _uiManager;
    private Spawn_Mananger _spawnScript;
    [SerializeField] GameObject _explosion;
   
    void Start()
    {
        _spawnScript = GameObject.FindObjectOfType<Spawn_Mananger>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        
        transform.position = new Vector3(0,-5,0);
       
        _fireWeapon = true;
       
        if (_spawnScript == null)
        {
            Debug.Log("THE SPAWNMANGER IS NULL!! MISSSSIINNG ERRROOORR");
        }

        if( _uiManager == null) 
        {
            Debug.Log("Missing UI Manager");
        }

        if(_audioSource == null) 
        {
            Debug.Log("AUDO SOURCE ON PLAYER NULL");
        }
        else { _audioSource.clip = _laserAudioClip; }
    }

    void Update()
    {
        PlayerBoundaries();
        PlayerAxisMove();
        Shoot();
        ShootRocket();
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
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(HorizontalInput,VerticalInput,0);
        Thruster();
        if(_isSpeedBoostEnabled == true)
        {
            transform.Translate(direction * (_playerSpeed + _speedBoost) * Time.deltaTime);
        }

        else 
        { 
            transform.Translate(direction * _playerSpeed * Time.deltaTime);
        }
        

    }

    private void Thruster() 
    {
      if (Input.GetKey(KeyCode.LeftShift))
        {
            _playerSpeed = 10;
            Debug.Log("THRUSTER " + _playerSpeed);
        }
        else { _playerSpeed = 8; }
    }

    private void ShootRocket() 
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        { 
            Instantiate(_rocket, transform.position, Quaternion.identity);
        }
        
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _fireWeapon)
        {
            if (_isTripleShotEnabled)
            {
                StartCoroutine(LaserParentChangeRoutine());
                _fireWeapon = false;
                PlayLaserAudio();
                StartCoroutine(DelayFireRateRoutine());
                _ammoCount--;
                LowAmmoAudio();
            }
            else 
            {
               StartCoroutine(LaserParentChangeRoutine());
               _fireWeapon = false;
;              PlayLaserAudio();
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
                SwitchAudioClip(_ammoReload);
                _ammoCount = _totalAmmo;
                _fireWeapon = true;
            }
        }
    }
    private void LowAmmoAudio()
    {
        if (_ammoCount == 0) 
        {
            SwitchAudioClip(_lowAmmoClip);
        }
    }
    public void Damage()
    {
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
        SwitchAudioClip(_powerUpAudioClip);
        _isTripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    public void SetSpeedBoost() 
    {
        SwitchAudioClip(_powerUpAudioClip);
        _isSpeedBoostEnabled = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
        
    }
    public void SetShield() 
    {
        GameObject shield = transform.GetChild(0).gameObject;
        _isShieldsEnabled = true;
        
        switch (_shieldLevel) 
        {
            case 0:
                SwitchAudioClip(_powerUpAudioClip);
                _isShieldsEnabled = true;
                shield.GetComponent<SpriteRenderer>().enabled = true;
                _shieldLevel++;
                break;
            
            case 1:
                SwitchAudioClip(_powerUpAudioClip);
                _isShieldsEnabled = true;
                shield.GetComponent<SpriteRenderer>().color = Color.red;
                _shieldLevel++;
                break;

            case 2:
                SwitchAudioClip(_powerUpAudioClip);
                _isShieldsEnabled = true;
                shield.GetComponent<SpriteRenderer>().color = Color.yellow;
                _shieldLevel++;
                break;
          default:
               if(IsShieldsOn() == false) 
                { 
                _shieldLevel = 0;
                }
                else { Debug.Log("MAX SHIELDS"); return; }
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

    private void PlayLaserAudio() 
    {
        _audioSource.clip = _laserAudioClip;
        _audioSource.Play();
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
       SwitchAudioClip(_ammoReload);
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
            Debug.Log("MAX LIVES");
            return;
        }
        else
        _lives++;
        _uiManager.AddLives(_lives);
        SwitchAudioClip(_collectLivesClip);
        StartCoroutine(FlashGreenCourtine());
    }

    IEnumerator LaserParentChangeRoutine()
    {
        Vector3 _laserpos = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        if (_isTripleShotEnabled)
        {
            GameObject tripleShot = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity, this.transform);
            yield return new WaitForSeconds(0.0001f);
            tripleShot.transform.parent = transform.parent;
           for(int i = 0; i < tripleShot.transform.childCount; i++) 
            { 
                tripleShot.transform.GetChild(i).tag = "Laser";
            }
            Destroy(tripleShot, 3);
        }
        else
        {
            GameObject laser = Instantiate(_laserPrefab, _laserpos, Quaternion.identity, this.transform);
            yield return new WaitForSeconds(0.0001f);
            laser.gameObject.tag = "Laser";
            laser.transform.parent = transform.parent;
        }
    }       

    IEnumerator DelayFireRateRoutine()
    {
        yield return new WaitForSeconds(_fireRate);
        _fireWeapon = true;
    }
    IEnumerator FlashRedCourtine()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.3f);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
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
    IEnumerator SpeedBoostPowerDownRoutine() 
    {
        yield return new WaitForSeconds(_speedBoostDuration);
        _isSpeedBoostEnabled = false;
    }
    private void SwitchAudioClip(AudioClip audioclip) 
    {
        _audioSource.clip = audioclip;
        _audioSource.Play();
    }
}
            
            




    





