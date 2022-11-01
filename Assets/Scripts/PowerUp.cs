using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
          Player player =  other.GetComponent<Player>();
            
            if (player != null) 
            {
                switch (_powerUpID) 
                { 
                    case 0:
                        player.SetTripleShot();
                        break;
                    case 1:
                        player.SetSpeedBoost();
                        break;
                    case 2:
                        player.SetShield();
                        break;
                    case 3:
                        player.SetCurrentAmmo();
                        break;
                    case 4:
                        player.AddLife();
                        break;
                    case 5:
                        player.SetRocket();
                        break;
                    case 6:
                        player.SetFrozenNegative();
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
