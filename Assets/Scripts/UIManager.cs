using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private GameObject _playerGameObject;
    private Player _playerScript;
    void Start()
    {
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
    }
}
