using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuhOJAS : MonoBehaviour
{

    public Button abajo;
    public Button medio;
    public Button arriba;


    public GameObject hojas1;
    private Animator anim1;
    
    public GameObject hojas2;
    private Animator anim2;
    
    public GameObject hojas3;
    private Animator anim3;
    
    public GameObject hojas4;
    private Animator anim4;
    
    public GameObject texto;
    private Animator animTexto;

    public GameObject parteAbajo;
    private Animator animAbajo;

    public GameObject parteMedia;
    private Animator animMedia;

    public GameObject parteArriba;
    private Animator animArriba;

    public GameObject torrijaLogo;
    private Animator animLogo;


    public GameObject senalAbajo;
    private Animator animSenalAbajo;

    public GameObject senalMedio;
    private Animator animSenalMedio;

    public GameObject senalArriba;
    private Animator animSenalArriba;

    public GameObject cartelDoodem;
    private Animator animCartelDoodem;

    // Start is called before the first frame update
    void Start()
    {
        anim1 = hojas1.GetComponent<Animator>();
        anim2 = hojas2.GetComponent<Animator>();
        anim3 = hojas3.GetComponent<Animator>();
        anim4 = hojas4.GetComponent<Animator>();
        
        animTexto = texto.GetComponent<Animator>();

        animAbajo = parteAbajo.GetComponent<Animator>();
        animMedia = parteMedia.GetComponent<Animator>();
        animArriba = parteArriba.GetComponent<Animator>();

        animLogo = torrijaLogo.GetComponent<Animator>();

        animSenalAbajo = senalAbajo.GetComponent<Animator>();
        animSenalMedio = senalMedio.GetComponent<Animator>();
        animSenalArriba = senalArriba.GetComponent<Animator>();
        animCartelDoodem = cartelDoodem.GetComponent<Animator>();

        arriba.gameObject.SetActive(false);
        medio.gameObject.SetActive(false);
        abajo.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim1.SetTrigger("Start");
            anim2.SetTrigger("Start");
            anim3.SetTrigger("Start");
            anim4.SetTrigger("Start");
            
            animTexto.SetTrigger("Start");

            Invoke("caerTotem", 1.29f);
            

            Invoke("aparecerLogo", 3.5f);
            Invoke("caerDoodem",4.5f);
            Invoke("ActivarBotones", 4.5f); 
            
        }

       
    }

    void caerTotem() {
        animAbajo.SetTrigger("Start");
        animMedia.SetTrigger("Start");
        animArriba.SetTrigger("Start");
        animSenalAbajo.SetTrigger("Start");
        animSenalMedio.SetTrigger("Start");
        animSenalArriba.SetTrigger("Start");
    }

    void aparecerLogo() {

        animLogo.SetTrigger("Start");
        Debug.Log("saas");
    }

    public void BotonAbajo(){
        Debug.Log("abajo");
    }
    public void BotonMedio(){
        Debug.Log("medio");
    }
    public void BotonArriba(){
        Debug.Log("arriba");
    }

    public void ActivarBotones()
    {
        arriba.gameObject.SetActive(true);
        medio.gameObject.SetActive(true);
        abajo.gameObject.SetActive(true);
    }

    public void caerDoodem(){
        animCartelDoodem.SetTrigger("Start");

    }
}
