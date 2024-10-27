using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class recurso : MonoBehaviour
{
    private bool isSelected;
    private int indexLayerArea;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.parent.parent.name);
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
        gameObject.SetActive(false);
    }

    public void CheckIfItsInMyBiome()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down,out hit, 1f, LayerMask.GetMask("casilla")))
        {
            if (indexLayerArea != hit.transform.GetComponent<NavMeshModifier>().area)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
