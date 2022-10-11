using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFPS: MonoBehaviour
{
    [SerializeField ] private float _playerSpeed = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
         PlayerBoundaries();
        _playerMovement();
        transform.Translate(new Vector3(1,0,0));
        
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
            transform.Translate(new Vector3(-1,0,0)*_playerSpeed * Time.deltaTime);
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
        transform.Translate(direction *_playerSpeed * Time.deltaTime);
    }
    
}
