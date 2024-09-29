using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Seleccionable : MonoBehaviour,IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    //private bool siendoArrastrado = false;
    public GameObject objetoACrear;
    private GameObject objeto;

    

    void Start()
    {
      
    }
   

    GameObject InstanciarObjeto(Vector3 position)
    {
        Debug.Log(position);
        //GameObject instanciado = Instantiate(objetoACrear, new Vector3(11.37f,1.26f,-6.37f), objetoACrear.transform.rotation);
        //NavMeshHit hit;
        //if (NavMesh.SamplePosition(instanciado.transform.position, out hit, 0.89f, NavMesh.AllAreas))
        //{
        //    instanciado.transform.position = hit.position;
        //}
        return Instantiate(objetoACrear, position, objetoACrear.transform.rotation);
    }


    void Update()
    {
        
    }

    private bool isDragging = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        objeto = InstanciarObjeto(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                objeto.transform.position = new Vector3(hit.point.x, 1.82f, hit.point.z);
                Debug.Log("Posición del objeto: " + objeto.transform.position);
            }
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        objeto.GetComponent<NavMeshAgent>().enabled = true;
        objeto.GetComponent<Entity>().enabled = true;
        objeto.GetComponent<Entity>().SetAgent();
        objeto = null;
    }
}
