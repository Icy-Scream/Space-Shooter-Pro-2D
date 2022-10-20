using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int _powerUpID;
    void Update()
    {
        PowerUpMove();
    }

    private void PowerUpMove() 
    {
        
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -7.0f) 
        {
            Destroy(this.gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
          Player player =  collision.GetComponent<Player>();
            if (player != null) 
            {
                if(_powerUpID == 0) 
                {
                    Debug.Log(_powerUpID);
                    player.SetTripleShot();
                }
                else if (_powerUpID == 1) 
                {
                    Debug.Log(_powerUpID + " SPEEED");
                }
                else if (_powerUpID == 2) 
                {
                    Debug.Log(_powerUpID + " SHIELDS");
                }
                
                
            }
         
            Destroy(this.gameObject);
            
        }
    }
}
