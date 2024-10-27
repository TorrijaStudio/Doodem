using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class wall : NetworkBehaviour
{
    public int contadorInicial = 60; 
    public bool cuentaRegresiva = true; 
    public float intervalo = 1f; 
    public TextMeshPro[] contadorTexto = new TextMeshPro[2]; 

    private int contadorActual;
    private float tiempoTranscurrido;

    void Start()
    {
        contadorActual = contadorInicial;
        ActualizarTexto();
    }

    void Update()
    {
        tiempoTranscurrido += Time.deltaTime;

        if (tiempoTranscurrido >= intervalo)
        {
            if (cuentaRegresiva)
            {
                contadorActual--;
                if (contadorActual <= 0)
                {
                    if (IsHost)
                    {
                        GameManager.Instance.StartGame();
                    }
                    transform.position +=Vector3.up*100;
                    contadorActual = 0;
                }
            }
            else
            {
                contadorActual++;
            }

            ActualizarTexto();
            tiempoTranscurrido = 0f;
        }
    }

    void ActualizarTexto()
    {
        if (contadorTexto != null)
        {
            int minutos =contadorActual / 60;
            int segundos = contadorActual % 60;
            contadorTexto[0].text = string.Format("{0:00}:{1:00}", minutos, segundos);
            contadorTexto[1].text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }
}
