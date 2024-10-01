using System;
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

    [SerializeField] private MeshRenderer terreno;
    public Vector2 grid;
    void Start()
    {
        terreno.sharedMaterial.SetFloat(ScaleX, grid.x);
        terreno.sharedMaterial.SetFloat(ScaleY, grid.y);
    }

    private void OnValidate()
    {
        terreno.sharedMaterial.SetFloat(ScaleX, grid.x);
        terreno.sharedMaterial.SetFloat(ScaleY, grid.y);
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
    private static readonly int ScaleX = Shader.PropertyToID("_ScaleX");
    private static readonly int ScaleY = Shader.PropertyToID("_ScaleY");

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
            
            if (Physics.Raycast(ray, out hit,100, LayerMask.GetMask("Terreno")))
            {
                var corner = hit.transform.position - hit.transform.lossyScale / 2F;
                var newPos = hit.point - corner;
                var cellSize = new Vector2(hit.transform.lossyScale.x, hit.transform.lossyScale.z) / grid;
                
                var pos = new Vector2Int((int)(newPos.x / cellSize.x), (int)(newPos.z/cellSize.y) );
                objeto.transform.position = new Vector3(corner.x + pos.x * cellSize.x + cellSize.x /2f, 1.82f, corner.z + pos.y * cellSize.y + cellSize.y/2f);
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
