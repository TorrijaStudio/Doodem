using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class playerInfo : NetworkBehaviour
{
    public NetworkVariable<int> idPlayer = new(writePerm:NetworkVariableWritePermission.Server);
    public ulong obj;

    public int PlayerId
    {
        get => idPlayer.Value;
        set => idPlayer.Value = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.players[PlayerId] = this;
        Debug.Log(name + ": " + PlayerId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
