using System;
using System.Collections;
using System.Collections.Generic;
using HelloWorld;
using Unity.AI.Navigation;
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
    public static int ClientID;
    public int indexPrefab;


    [SerializeField] private MeshRenderer terreno;
    private Vector2 _grid;
    private bool _selected;
    private List<Transform> cartas;
    // public Seleccionable Instance;
    
    void Start()
    {
        cartas = new List<Transform>();
        foreach (Transform t in transform.parent)
        {
            if(t!=transform)
                cartas.Add(t);
        }
        _grid = terreno.gameObject.GetComponent<terreno>().GetGrid();
        ClientID = -1;
        // if(Instance)

    }
    
    GameObject InstanciarObjeto(Vector3 position)
    {
        
        var a = Instantiate(objetoACrear, position, objetoACrear.transform.rotation);
        a.name += " soy un objeto tonto que explota";
        return a;

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
                        1.1f, corner.z + pos.y * cellSize.y + cellSize.y/2f);
                }
            }
        }
    
        if (Input.GetMouseButtonUp(0))
        {
            if (_selected && objeto)
            {
                // objeto.GetComponent<NavMeshAgent>().enabled = true;
                // objeto.GetComponent<Entity>().enabled = true;
                Debug.Log(name + ": " + ClientID);
                SpawnServer(objeto.transform.position, ClientID);
                //objeto.GetComponent<Entity>().SetAgent();
                StartCoroutine(BorrarObjeto(objeto));
                _selected = false;
                objeto = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("ClientID: " + ClientID);
        }
    }

    private IEnumerator BorrarObjeto(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(obj);
    }
    
    private void SpawnServer(Vector3 pos, int playerId)
    {
        // Debug.Log(playerId);
        if(IsSpawned)
            GameManager.Instance.SpawnServerRpc(playerId, indexPrefab, pos);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _selected = true;
        foreach (var c in cartas)
        {
            c.GetComponent<Seleccionable>().SetFalse();
        }
    }
    public void SetFalse()
    {
        _selected = false;
    }
}
