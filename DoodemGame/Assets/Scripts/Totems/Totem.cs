using System;
using System.Collections;
using System.Collections.Generic;
using Totems;
using UnityEngine;

public class Totem : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private float TotemOffset = 1.0f;
    [SerializeField] private float TotemPieceHover = 0.30f;
    
    [SerializeField] private Transform head;
    [SerializeField] private Transform body;
    [SerializeField] private Transform feet;
    

    private bool isLocked = false;
    
    
    public void CreateTotem(GameObject h, GameObject b, GameObject f)
    {
        _transform = transform;
        if(!(ValidatePartDebug(h, "Head") && ValidatePartDebug(b, "Body") && ValidatePartDebug(f, "Feet")))
            return;

        var position = _transform.position;
        var up = _transform.up * TotemOffset;
        head = Instantiate(h, position + up, Quaternion.identity, _transform).transform;
        // head.transform.SetParent(_transform);
        
        body = Instantiate(b, position, Quaternion.identity, _transform).transform;
        // body.transform.SetParent(_transform);
        
        feet = Instantiate(f, position - up, Quaternion.identity, _transform).transform;
        // feet.transform.SetParent(_transform);
    }

    private bool ValidatePartDebug(GameObject g, string type)
    {
        if (!g.CompareTag(type))
        {
            Debug.LogError($"Totem {name} was given object {g.name} of type {g.tag} instead of a {type}!");
            return false;
        }

        return true;
    }

    public bool AddPart(TotemPiece pieceToSet, out TotemPiece outPiece)
    {
        outPiece = null;
        var result = pieceToSet.tag switch
        {
            "Head" => TryAddPiece(pieceToSet, ref head, out outPiece),
            "Body" => TryAddPiece(pieceToSet, ref body, out outPiece),
            "Feet" => TryAddPiece(pieceToSet, ref feet, out outPiece),
            _ => false
        };

        return result;
    }

    public Transform GetPiece(string pieceTag)
    {
        return pieceTag switch
        {
            "Head" => head,
            "Body" => body,
            "Feet" => feet,
            _ => null
        };
    }

    private bool TryAddPiece(TotemPiece pieceToSet, ref Transform other, out TotemPiece outPiece)
    {
        if (!other)
        {
            outPiece = null;
            return false;
        }

        outPiece = other.GetComponent<TotemPiece>();
        other = pieceToSet.transform;
        other.GetComponent<TotemPiece>().totem = this;
        other.SetParent(_transform);
        return true;
    }
    
    public void Separate(int mode)
    {
        if(isLocked)    return;
        // if(!(head && feet && body))    return;
        
        var distanceHead = TotemOffset;
        float distanceBody = 0;
        switch (mode)
        {
            case 1:
                distanceHead += TotemPieceHover;
                break;
            case 2:
                distanceHead += TotemPieceHover * 2;
                distanceBody += TotemPieceHover;
                break;
            case 3:
                distanceHead += TotemPieceHover;
                distanceBody += TotemPieceHover;
                break;
        }

        var position = transform.position;
        var up = _transform.up;
        var speed = 0.1f;
        if(head)
            head.GetComponent<TotemPiece>().MoveTo(position + (up * distanceHead), speed);
        if(body)
            body.GetComponent<TotemPiece>().MoveTo(position + (up * distanceBody), speed);
        if(feet)
            feet.GetComponent<TotemPiece>().MoveTo(position - up * TotemOffset , speed);

    }

    public bool CanTakePart(GameObject part)
    {
        return part.tag switch
        {
            "Feet" => feet,
            "Body" => body,
            "Head" => head,
            _ => false
        };
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        _transform = transform;
        // if(head)
        //     totemHead = head.GetComponent<TotemPiece>();
        // if(body)
        //     totemBody = body.GetComponent<TotemPiece>();
        // if(feet)
        //     totemFeet = feet.GetComponent<TotemPiece>();
    }

    public void Deactivate()
    {
        if(isLocked)    return;
        
        Separate(-1);
        // transform.GetChild(0).GetComponent<TotemPiece>().MoveTo(_transform.position + TotemOffset * _transform.up, 0.01f);
        // transform.GetChild(1).GetComponent<TotemPiece>().MoveTo(_transform.position, 0.01f);
        // transform.GetChild(2).GetComponent<TotemPiece>().MoveTo(_transform.position - TotemOffset * _transform.up, 0.01f);
    }
    
    
    // private void OnMouseExit()
    // {
    //     Debug.Log("Uwu??");
    //     totemHead.MoveTo(_transform.position + _transform.up * TotemOffset, 0.01f);
    //     totemBody.MoveTo(transform.position,0.01f);
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Pointer"))
    //     {
    //         Deactivate();
    //     }
    // }

    public void Lock(bool locked)
    {
        isLocked = locked;
    }
    // Update is called once per frame
    private void Update()
    {
        
    }
}
