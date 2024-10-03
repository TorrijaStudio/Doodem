using System;
using System.Collections;
using System.Collections.Generic;
using HelloWorld;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Seleccionable : NetworkBehaviour, IPointerDownHandler
{
    public GameObject objetoACrear;
    private GameObject objeto;
    public static ulong clientID;

    [SerializeField] private MeshRenderer terreno;
    private Vector2 _grid;
    private bool _selected;
    
    void Start()
    {
        _grid = terreno.gameObject.GetComponent<terreno>().GetGrid();
    }
    
    GameObject InstanciarObjeto(Vector3 position)
    {
        
        return Instantiate(objetoACrear, position, objetoACrear.transform.rotation);
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (_selected)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
            
                if (Physics.Raycast(ray, out hit,100, LayerMask.GetMask("Terreno")))
                {
                    var corner = hit.transform.position - hit.transform.lossyScale / 2F;
                    var newPos = hit.point - corner;
                    var cellSize = new Vector2(hit.transform.lossyScale.x, hit.transform.lossyScale.z) / _grid;
                
                    var pos = new Vector2Int((int)(newPos.x / cellSize.x), (int)(newPos.z/cellSize.y) );
                    if (objeto == null) {objeto = InstanciarObjeto(Input.mousePosition);}
                    objeto.transform.position = new Vector3(corner.x + pos.x * cellSize.x + cellSize.x /2f, 
                        1.82f, corner.z + pos.y * cellSize.y + cellSize.y/2f);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_selected && objeto)
            {
                // objeto.GetComponent<NavMeshAgent>().enabled = true;
                // objeto.GetComponent<Entity>().enabled = true;
                SpawnServer(objeto.transform.position, clientID);
                //objeto.GetComponent<Entity>().SetAgent();
                StartCoroutine(BorrarObjeto(objeto));
                _selected = false;
                objeto = null;
            }
        }
    }

    private IEnumerator BorrarObjeto(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(obj);
    }
    
    private void SpawnServer(Vector3 pos, ulong obj)
    {
        GameManager.Instance.SpawnServerRpc(obj, 0, pos);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _selected = true;
    }
}
