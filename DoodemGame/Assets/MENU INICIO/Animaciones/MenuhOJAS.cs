using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuhOJAS : MonoBehaviour
{
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

    public GameObject cartel;
    private Animator animCartel;

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
        animCartel = cartel.GetComponent<Animator>();

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

            
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            animCartel.SetTrigger("Start");
        }
    }

    void caerTotem() {
        animAbajo.SetTrigger("Start");
        animMedia.SetTrigger("Start");
        animArriba.SetTrigger("Start");
    }

    void aparecerLogo() {

        animLogo.SetTrigger("Start");
        Debug.Log("saas");
    }
}
