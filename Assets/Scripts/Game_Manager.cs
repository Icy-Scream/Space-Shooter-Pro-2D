
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            Application.Quit();
        }
    }
}
