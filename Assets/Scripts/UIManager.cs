using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartLevel;
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private bool _isGameOver;
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _maxShields;
    [SerializeField] private TMP_Text _maxHealth;
    private Player _playerScript;
   
    void Start()
    {
        _restartLevel.gameObject.SetActive(false);
        if(_playerGameObject != null)
        {
            _playerScript = _playerGameObject.GetComponent<Player>();
        }    
    }

    void Update()
    {
        if(_playerGameObject!= null)
        {
            _scoreText.text = "Score: " + _playerScript.GetScore();
           if(_playerScript.GetCurrentAmmo() == 0) 
            {
              _ammoText.text = "Ammo: " + _playerScript.GetCurrentAmmo() + "/" + _playerScript.GetTotalAmmo() + " 'R' RELOAD";
            }else
            _ammoText.text =  "Ammo: " + _playerScript.GetCurrentAmmo() +"/"+ _playerScript.GetTotalAmmo();
        }
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1); 
        }

    }

    public void MaxLives() 
    {
        StartCoroutine(MaxLivesFlickerRoutine());
    }
    

    public void UpdateLives(int currentLives) 
    {
        if (currentLives >= 0) 
        {
           transform.GetChild(1).transform.GetChild(currentLives).GetComponent<Image>().enabled = false;    
        }
        else return;
            
    }

    public void AddLives(int currentLives) 
    {
        Debug.Log("CURRENT LIVES " + currentLives);
       transform.GetChild(1).transform.GetChild(currentLives - 1).GetComponent<Image>().enabled = true;
    }


    public void GameOver()
    {
        _isGameOver = true;
        _restartLevel.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
 
    }

    public void MaxShields() 
    {
        StartCoroutine(MaxShieldsFlickerRoutine());
    }

    public void ThrusterSlider(float thrust)
    {
        _slider.value = thrust;
    }


    IEnumerator GameOverFlickerRoutine() 
    {
        while (true) 
        { 
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator MaxShieldsFlickerRoutine()
    {
        
        for(int i = 0; i <= 2; i++)
        {
            _maxShields.text = "MAX SHIELDS";
            yield return new WaitForSeconds(0.5f);
            _maxShields.text = "";
            yield return new WaitForSeconds(0.5f);
        }
        _maxShields.text = "";
    }

    IEnumerator MaxLivesFlickerRoutine()
    {

        for (int i = 0; i <= 2; i++)
        {
            _maxHealth.text = "MAX LIVES";
            yield return new WaitForSeconds(0.5f);
            _maxHealth.text = "";
            yield return new WaitForSeconds(0.5f);
        }
        _maxHealth.text = "";
    }







}
