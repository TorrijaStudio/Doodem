using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuhOJAS : MonoBehaviour
{

    public Button abajo; //señal Opciones
    public Button medio; //señal Tienda
    public Button arriba; //señal Jugar
    public Button back; //botón volver al menú ppal
    public Button host; //botón de host
    public Button client; //botón de cliente
    public TMP_InputField code; //cuadro para escirbir el codigo de sala
    public Button expansions; // compra de expansiones
    public Button skins; // tienda de cosmeticos
    public Button ad; // ver anunmcio para ganar recompensa
    public Button coinPurchase; // boton de + para comprar monedas (tienda dinero real)
    public Scrollbar general;
    public Scrollbar music;
    public Scrollbar sfx;


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

    public GameObject cartelMenus;
    private Animator animCartelMenus;

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
        animCartelMenus = cartelMenus.GetComponent<Animator>();

        DesactivarBotonesSenales();
        DesactivarBotonesMenuJugar();
        DesactivarBotonesTienda();
        DesactivarBotonesOpciones();
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
            Invoke("ActivarBotonesSenales", 6.5f); 
            
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


    public void PulsadoSenalAbajo(){
        Debug.Log("se ha pulsado la senal de abajo vale??");
        animCartelDoodem.SetTrigger("pulsar");
        animSenalAbajo.SetTrigger("Pulsado");
        animSenalMedio.SetTrigger("Pulsado");
        animSenalArriba.SetTrigger("Pulsado");
        animAbajo.SetTrigger("pulsar");
        animMedia.SetTrigger("pulsar");
        animArriba.SetTrigger("pulsar");
        animCartelMenus.SetTrigger("Start");
        DesactivarBotonesSenales();
        ActivarBotonesOpciones();



    }

    public void PulsadoSenalMedio(){
        Debug.Log("se ha pulsado la senal de abajo vale??");
        animCartelDoodem.SetTrigger("pulsar");
        animSenalAbajo.SetTrigger("Pulsado");
        animSenalMedio.SetTrigger("Pulsado");
        animSenalArriba.SetTrigger("Pulsado");
        animAbajo.SetTrigger("pulsar");
        animMedia.SetTrigger("pulsar");
        animArriba.SetTrigger("pulsar");
        animCartelMenus.SetTrigger("Start");
        DesactivarBotonesSenales();
        ActivarBotonesTienda();

    }

    public void PulsadoSenalArriba(){
        Debug.Log("se ha pulsado la senal de abajo vale??");
        animCartelDoodem.SetTrigger("pulsar");
        animSenalAbajo.SetTrigger("Pulsado");
        animSenalMedio.SetTrigger("Pulsado");
        animSenalArriba.SetTrigger("Pulsado");
        animAbajo.SetTrigger("pulsar");
        animMedia.SetTrigger("pulsar");
        animArriba.SetTrigger("pulsar");
        animCartelMenus.SetTrigger("Start");
        DesactivarBotonesSenales();
        ActivarBotonesMenuJugar();

    }

    public void ActivarBotonesSenales()
    {
        arriba.gameObject.SetActive(true);
        medio.gameObject.SetActive(true);
        abajo.gameObject.SetActive(true);
    }

    public void DesactivarBotonesSenales(){
        arriba.gameObject.SetActive(false);
        medio.gameObject.SetActive(false);
        abajo.gameObject.SetActive(false);

    }

    private void ActivarBotonesMenuJugar(){
        back.gameObject.SetActive(true);
        host.gameObject.SetActive(true);
        client.gameObject.SetActive(true);
        code.gameObject.SetActive(true);
    }

    private void DesactivarBotonesMenuJugar(){
        back.gameObject.SetActive(false);
        host.gameObject.SetActive(false);
        client.gameObject.SetActive(false);
        code.gameObject.SetActive(false);
    }

    private void ActivarBotonesTienda(){
        back.gameObject.SetActive(true);
        expansions.gameObject.SetActive(true);
        skins.gameObject.SetActive(true);
        ad.gameObject.SetActive(true);
        coinPurchase.gameObject.SetActive(true);
    }

    private void DesactivarBotonesTienda(){
        back.gameObject.SetActive(false);
        expansions.gameObject.SetActive(false);
        skins.gameObject.SetActive(false);
        ad.gameObject.SetActive(false);
        coinPurchase.gameObject.SetActive(false);
    }

    private void ActivarBotonesOpciones(){
        back.gameObject.SetActive(true);
        general.gameObject.SetActive(true);
        music.gameObject.SetActive(true);
        sfx.gameObject.SetActive(true);

    }

    private void DesactivarBotonesOpciones(){
        back.gameObject.SetActive(false);
        general.gameObject.SetActive(false);
        music.gameObject.SetActive(false);
        sfx.gameObject.SetActive(false);
    }

    public void caerDoodem(){
        animCartelDoodem.SetTrigger("Start");

    }

    public void BotonAtras(){
        animCartelMenus.SetTrigger("Back");
        DesactivarBotonesMenuJugar();
        DesactivarBotonesTienda();
        DesactivarBotonesOpciones();
        ActivarBotonesSenales();
        caerTotem();
    }

    public void BotonHost(){
        Debug.Log("ahora estarias hosteando");
    }

    public void BotonClient(){
        Debug.Log("si hubiese code valido estarias de cliente");
    }

    public void ExpansionShop(){
        Debug.Log("esta es la tienda de expansiones mira q guapaaa");
    }

    public void SkinShop(){
        Debug.Log("esta es la tienda de cosmeticos mira q guapaaa");
    }

    public void ViewAd(){
        Debug.Log("viendo anuncio");
    }

    public void coinShop(){
        Debug.Log("esta es la tienda de monedas a soltar la pasta q hace falta");
    }
}
