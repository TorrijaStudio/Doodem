using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class wall : NetworkBehaviour
{
    public int contadorInicial = 30; 
    public bool cuentaRegresiva = true; 
    public float intervalo = 1f;
  

    private TextMeshProUGUI _text;
    private int contadorActual;
    private float tiempoTranscurrido;
    public bool startTimer = true;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        contadorActual = contadorInicial;
        ActualizarTexto();
    }

    void Update()
    {
        tiempoTranscurrido += Time.deltaTime;

        if (startTimer && tiempoTranscurrido >= intervalo)
        {
            if (cuentaRegresiva)
            {
                contadorActual--;
                if (contadorActual <= 0)
                {
                    startTimer = false;
                    if (GameManager.Instance.clientId==0)
                    {
                        GameManager.Instance.ExecuteOnAllClientsClientRpc();
                    }
                    //transform.position +=Vector3.up*100;
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
        int minutos =contadorActual / 60;
        int segundos = contadorActual % 60;
        _text.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        _text.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}
