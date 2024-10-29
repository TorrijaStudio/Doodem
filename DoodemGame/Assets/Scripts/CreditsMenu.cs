using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CreditsMenu : MonoBehaviour
{
    public void Back()
    {
        // Cargar la siguiente escena del juego
        SceneManager.LoadScene("MainMenu"); // Reemplaza "GameScene" con el nombre de tu escena de juego
    }

    }

