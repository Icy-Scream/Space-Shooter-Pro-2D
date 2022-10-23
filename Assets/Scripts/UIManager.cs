using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartLevel;
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private bool _isGameOver;
    private Player _playerScript;
    void Start()
    {
        _restartLevel.gameObject.SetActive(false);
        if(_playerGameObject != null)
        {
            _playerScript = _playerGameObject.GetComponent<Player>();
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerGameObject!= null)
        {
            _scoreText.text = "Score: " + _playerScript.GetScore();
        }
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1); 
        }

    }
    

    public void UpdateLives(int currentLives) 
    {
        transform.GetChild(1).transform.GetChild(currentLives).GetComponent<SpriteRenderer>().enabled = false;
    }

    public void GameOver()
    {
        _isGameOver = true;
        _restartLevel.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
 
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
        
        
        




}
