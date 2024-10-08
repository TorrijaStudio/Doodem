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
    
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject feet;

    private TotemPiece totemHead;
    private TotemPiece totemBody;
    private TotemPiece totemFeet;

    private bool isLocked = false;
    
    
    public void CreateTotem(GameObject h, GameObject b, GameObject f)
    {
        _transform = transform;
        if(!(ValidatePartDebug(h, "Head") && ValidatePartDebug(b, "Body") && ValidatePartDebug(f, "Feet")))
            return;

        var position = _transform.position;
        var up = _transform.up * TotemOffset;
        head = Instantiate(h, position + up, Quaternion.identity, _transform);
        // head.transform.SetParent(_transform);
        
        body = Instantiate(b, position, Quaternion.identity, _transform);
        // body.transform.SetParent(_transform);
        
        feet = Instantiate(f, position - up, Quaternion.identity, _transform);
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

    public void Separate(int mode)
    {
        if(isLocked)    return;
        if(!(head && feet && body))    return;
        
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
            default:
                break;
        }

        var position = _transform.position;
        var up = _transform.up;
        var speed = 0.01f;
        transform.GetChild(0).GetComponent<TotemPiece>().MoveTo(position + (up * distanceHead), speed);
        transform.GetChild(1).GetComponent<TotemPiece>().MoveTo(position + (up * distanceBody), speed);

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
        if(head)
            totemHead = head.GetComponent<TotemPiece>();
        if(body)
            totemBody = body.GetComponent<TotemPiece>();
        if(feet)
            totemFeet = feet.GetComponent<TotemPiece>();
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
