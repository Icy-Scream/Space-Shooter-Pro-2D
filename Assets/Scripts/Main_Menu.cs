using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public void LoadGame() 
    {
        SceneManager.LoadScene(1); //Load Game Level
    }
}
