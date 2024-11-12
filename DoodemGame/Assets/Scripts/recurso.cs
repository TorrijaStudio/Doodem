using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Netcode;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class recurso : MonoBehaviour
{
    private bool isSelected;
    private int indexLayerArea;
    public Recursos _typeRecurso;
    private static Random random;
    
    // Start is called before the first frame update
    void Start()
    {
        indexLayerArea = transform.parent.parent.GetComponent<ABiome>().indexLayerArea;
        StartCoroutine(AddPosition());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator AddPosition()
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.AddPositionSomething(transform.position,gameObject);
        GameManager.Instance.entidatesPrueba.Add(gameObject);
    }
    public bool GetSelected()
    {
        return isSelected;
    }

    public void SetSelected(bool b)
    {
        isSelected = b;
    }

    public void PickRecurso()
    {
        // gameObject.SetActive(false);
        //Destroy(gameObject);
        transform.position += Vector3.right*100;
    }

    public void CheckIfItsInMyBiome()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down,out hit, 1f, LayerMask.GetMask("casilla")))
        {
            if (indexLayerArea != hit.transform.GetComponent<NavMeshModifier>().area || hit.transform.GetComponent<casilla>().GetBiome().
                    GetComponent<NetworkObject>().OwnerClientId != transform.root.GetComponent<NetworkObject>().OwnerClientId)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
