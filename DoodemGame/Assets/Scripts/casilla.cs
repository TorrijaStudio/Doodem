using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Netcode;
using UnityEngine;

public class casilla : MonoBehaviour
{
    private GameObject biome;
    private GameObject previousBiome;
    private Material previousMaterial;
    private NavMeshModifier _navMeshModifier;
    private int previousIndexArea;
    private Material originalMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        _navMeshModifier = GetComponent<NavMeshModifier>();
        originalMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Getters and Setters

    public void SetPreviousBiome(GameObject o)
    {
        previousBiome = o;
    }
    public GameObject GetPreviousBiome()
    {
        return previousBiome;
    }
    
    public void SetBiome(GameObject o)
    {
        biome = o;
    }
    public GameObject GetBiome()
    {
        return biome;
    }
   
    public void SetPreviousMat(Material m)
    {
        previousMaterial = m;
    }
    public Material GetPrevousMat()
    {
        return previousMaterial;
    }
    public void SetAreaNav(int index)
    {
        _navMeshModifier.area = index;
    }

    public int GetAreaNav()
    {
        return _navMeshModifier.area;
    }

    public void SetPreviousIndexArea(int index)
    {
        previousIndexArea = index;
    }

    public int GetPreviousIndexArea()
    {
        return previousIndexArea;
    }
    #endregion

    public void ResetCasilla()
    {
        previousBiome = null;
        biome = null;
        previousIndexArea = 0;
        _navMeshModifier.area = 0;
        previousMaterial = originalMaterial;
        GetComponent<MeshRenderer>().material = originalMaterial;
    }
    
}
