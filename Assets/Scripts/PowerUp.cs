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
                switch (_powerUpID) 
                { 
                    case 0:
                        Debug.Log("TRIPLE SHOT");
                        player.SetTripleShot();
                        break;
                    case 1:
                        Debug.Log("SPEED BOOST");
                        player.SetSpeedBoost();
                        break;
                    case 2:
                        Debug.Log("Shields");
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
                
            }
         
            Destroy(this.gameObject);
            
        }
    }
}
