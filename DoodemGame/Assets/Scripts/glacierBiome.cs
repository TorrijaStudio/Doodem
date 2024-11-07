using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glacierBiome : ABiome
{

    // Update is called once per frame
   

    public override void ActionBioma(GameObject o)
    {
        var entity = o.GetComponent<Entity>();
        entity.SetSpeed(entity.GetSpeed()-3.0f);
    }

    public override void LeaveBiome(GameObject o)
    {
        var entity = o.GetComponent<Entity>();
        entity.SetSpeed(entity.GetSpeed()+3.0f);
    }
    
}
