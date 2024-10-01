using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class terreno : MonoBehaviour
{
    public Vector2 grid;

    [SerializeField] private MeshRenderer _meshRenderer;
    private static readonly int ScaleX = Shader.PropertyToID("_ScaleX");
    private static readonly int ScaleY = Shader.PropertyToID("_ScaleY");
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public Vector2 GetGrid()
    {
        return grid;
    }
  
    private void OnValidate()
    {
        _meshRenderer.sharedMaterial.SetFloat(ScaleX, grid.x);
        _meshRenderer.sharedMaterial.SetFloat(ScaleY, grid.y);
    }
}
