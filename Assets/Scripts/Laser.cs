using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _laserSpeed;
    [SerializeField] private int _ID;
    [SerializeField] private float _destroyTimer;

    void Start()
    {
        StartCoroutine(DestroyLaserRoutine());
      
       if(transform.parent.tag == "Enemy") 
        {
            _ID = 2;
            Debug.Log("SHOOTING ENEMY");
        }
        else if(transform.parent.tag == "Player" || transform.parent.tag == "TripleShot") 
        {  
            _ID = 1; 
            Debug.Log("SHOOTING PLAYER"); 
        }
    }

    void Update()
    {
        LaserTranslate(_ID);
    }

    private void LaserTranslate(int GameObjectID)
    {
        if (GameObjectID == 1) 
        { 
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime); 
        }
        else if(GameObjectID == 2) 
        { 
            transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime); 
        }
    }
    IEnumerator DestroyLaserRoutine()
    {
        yield return new WaitForSeconds(_destroyTimer);
        Destroy(this.gameObject);
    }
}
