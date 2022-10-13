using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField ] private float _playerSpeed = 5.0f;
     [SerializeField] private GameObject laserPrefab;
     [SerializeField] private bool _fireWeapon;
     [SerializeField] private float _fireRate;
     [SerializeField] private float _playerHealth = 100f;
     [SerializeField] private int _lives = 3;
     Spawn_Mananger spawnScript;

    void Start()
    {
        spawnScript = GameObject.FindObjectOfType<Spawn_Mananger>();
        transform.position = new Vector3(0,0,0);
        _fireWeapon = true;
        if (spawnScript == null)
        {
            Debug.Log("THE SPAWNMANGER IS NULL!! MISSSSIINNG ERRROOORR");
        }
    }

    // Update is called once per frame
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

    private void _playerMovement()
    {
         
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-1,0,0) *_playerSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(1,0,0)*_playerSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0,1,0)*_playerSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0,-1,0)*_playerSpeed * Time.deltaTime);
        }
            
    }

    private void playeraxismove()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");


        Vector3 direction = new Vector3(HorizontalInput,VerticalInput,0);
        transform.Translate(direction  *_playerSpeed * Time.deltaTime);
    }
    private void Shoot()
    {
        Vector3 laserpos = new Vector3(transform.position.x,transform.position.y + 0.8f, transform.position.z);
        
        if(Input.GetKeyDown(KeyCode.Space) && _fireWeapon)
        {
          Instantiate(laserPrefab,laserpos,Quaternion.identity);
          _fireWeapon = false;
          StartCoroutine(DelayFireRateRoutine());
        }
    } 

    IEnumerator DelayFireRateRoutine()
    {
        yield return new WaitForSeconds(_fireRate);
        _fireWeapon = true;
    }

    public void Damage()
    {
        _playerHealth += -20;
        
        if(_playerHealth < 1)
        {   _lives--;
            
            if(_lives < 1)
            {
                spawnScript.OnPlayerDeath();
                Destroy(this.gameObject);
                _playerHealth = 0;
            }
            else _playerHealth = 100;
        }
        
    }
    

}
