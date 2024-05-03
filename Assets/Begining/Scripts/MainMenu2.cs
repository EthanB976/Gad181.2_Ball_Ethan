using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu2 : MonoBehaviour
{
    
    public void ShakeFighter()
    {
        SceneManager.LoadScene(1);
    }

    
    public void EscapeFromShake()
    {
        SceneManager.LoadScene(2);
    }
}

