using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
     [SerializeField] private GameObject _laserPrefab;
     [SerializeField] private float _playerSpeed = 5.0f;
     [SerializeField] private float _playerHealth = 100f;
     [SerializeField] private int _score = 0;
     [SerializeField] private int _lives = 3;
   
     [SerializeField] private bool _fireWeapon;
     [SerializeField] private float _fireRate;
     
     private bool _isTripleShotEnabled = false;
     [SerializeField] private GameObject _tripleShotPrefab;
     [SerializeField] private float _tripleShotDuration = 5.0f;

    private bool _isSpeedBoostEnabled = false;
    [SerializeField] private float _speedBoost = 5.0f;
    [SerializeField] private float _speedBoostDuration = 3.0f;
    [SerializeField] private bool _isShieldsEnabled = false;
     
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserAudioClip;
    [SerializeField] private AudioClip _powerUpAudioClip;
   
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
        playeraxismove();
        Shoot();
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
    private void playeraxismove()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(HorizontalInput,VerticalInput,0);
        if(_isSpeedBoostEnabled == true)
        {
            transform.Translate(direction * (_playerSpeed + _speedBoost) * Time.deltaTime);
        }
        else
            transform.Translate(direction * _playerSpeed * Time.deltaTime);

    }
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _fireWeapon)
        {
            if (_isTripleShotEnabled)
            {
                StartCoroutine(LaserParentChangeRoutine());
                PlayLaserAudio();
                _fireWeapon = false;
                StartCoroutine(DelayFireRateRoutine());
            }
            else 
            {
               StartCoroutine(LaserParentChangeRoutine());
;              PlayLaserAudio();
              _fireWeapon = false;
              StartCoroutine(DelayFireRateRoutine());
            }
        }
    } 
    public void Damage()
    {
        if (_isShieldsEnabled) 
        {
            _isShieldsEnabled = false;
            GameObject _shield = transform.GetChild(0).gameObject;
            _shield.GetComponent<SpriteRenderer>().enabled = false;
            return;    
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
        PlayPowerUpAudio();
        _isTripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    public void SetSpeedBoost() 
    {
        PlayPowerUpAudio();
        _isSpeedBoostEnabled = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
        
    }
    public void SetShield() 
    {
        PlayPowerUpAudio();
        _isShieldsEnabled = true;
        GameObject shield = transform.GetChild(0).gameObject;
        shield.GetComponent<SpriteRenderer>().enabled = true;
        
    }
    public void AddScore(int points) 
    {
        _score += points;
    }
    public int GetScore() 
    {
        return _score;
    }
    private void PlayPowerUpAudio() 
    {
        _audioSource.clip = _powerUpAudioClip;
        _audioSource.Play();
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
}
            
            




    





