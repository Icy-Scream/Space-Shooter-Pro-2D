using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BANK : MonoBehaviour
{
    [SerializeField] private float _currentbalance;
    [SerializeField] private float _Interest;
    [SerializeField] private float _PlayerSalary;
    [SerializeField] private float _newbalance;
    [SerializeField] private float _PlayerSavingValue;
    [SerializeField] private float _PlayerSavingInterest;
    [SerializeField] private float _YearsWork;
   // Start is called before the first frame update
    void Start()
    {
        _currentbalance = 500;
        _PlayerSalary = 100000;
        _PlayerSavingInterest = 0.05f;
        _Interest = 0.10f;
        _YearsWork = 0; 
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePlayerSavings();
        CaculatePlayerYearlyInterest();
    }

    private void CalculatePlayerSavings()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
        _PlayerSavingValue = _PlayerSalary*_PlayerSavingInterest;
        }

    }

    private void CaculatePlayerYearlyInterest()
    {
      if(Input.GetKeyDown(KeyCode.Q))
      {
       if(_YearsWork < 1 )
       {
        _currentbalance += _PlayerSavingValue;
        _YearsWork++;
       }
       else if (_YearsWork >= 1)
       {
       for(int i=0; i <= 40; i++)
       {
         _newbalance = _currentbalance * _Interest;
         _currentbalance += (_newbalance + _PlayerSavingValue);
         _YearsWork++;
         if(_YearsWork == 2){Debug.Log(_currentbalance + _YearsWork);}
         if(_YearsWork == 10){Debug.Log(_currentbalance + _YearsWork);}
         if(_YearsWork == 20){Debug.Log(_currentbalance + _YearsWork);}
         if(_YearsWork == 30){Debug.Log(_currentbalance + _YearsWork);}
         if(_YearsWork == 40){Debug.Log(_currentbalance + _YearsWork);}
       }

       }
      }

    }

    



    
}
