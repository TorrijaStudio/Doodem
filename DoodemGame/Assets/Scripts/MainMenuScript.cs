using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        // Cargar la siguiente escena del juego
        SceneManager.LoadScene("main"); // Reemplaza "GameScene" con el nombre de tu escena de juego
    }

    public void Tienda()
    {
        SceneManager.LoadScene("Menu Tienda"); // Reemplaza "GameScene" con el nombre de tu escena de juego

    }

    public void Creditos()
    {
        SceneManager.LoadScene("Menu Creditos");
    }

    public void QuitGame()
    {
        // Salir del juego
        Debug.Log("Quit");
        Application.Quit();
    }
}
