using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Rocket : MonoBehaviour
{
    [SerializeField] float _rocketSpeed;
    [SerializeField] float _offsetAngle;
    private Enemy _seekingEnemy;

    void Start()
    {
        _seekingEnemy = FindObjectOfType<Enemy>();
       
        if( _seekingEnemy == null) 
        {
            transform.Translate(Vector3.up * _rocketSpeed * Time.deltaTime);
        }
    }

    void Update()
    {
        //SeekingEnemy();
        MathROcket();
    }

    private void SeekingEnemy() 
    {
        if (_seekingEnemy != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _seekingEnemy.transform.position, _rocketSpeed * Time.deltaTime);
            Vector3 vectorToTarget = _seekingEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + _offsetAngle));
        }
        else if (_seekingEnemy == null)
        {
            transform.Translate(Vector3.up * _rocketSpeed * Time.deltaTime);
            Destroy(this.gameObject, 3);
        }

    }
    
    private void MathROcket()
    {
        Vector3 a = this.transform.position;
        Vector3 b = _seekingEnemy.transform.position;
        Vector3 _directionToTarget =(b - a).normalized;
        transform.Translate(_directionToTarget  *  Time.deltaTime);
    }           
                
            
            

 

}
