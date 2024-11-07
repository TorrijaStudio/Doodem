using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class desertBiome : ABiome
{
    public int damageReduction;
    // Start is called before the first frame update
   

    // Update is called once per frame
    

    public override void ActionBioma(GameObject o)
    {
        var entity = o.GetComponent<Entity>();
        entity.SetCurrentDamage( entity.GetCurrentDamage() - entity.damage * damageReduction/100f);
    }

    public override void LeaveBiome(GameObject o)
    {
        var entity = o.GetComponent<Entity>();
        entity.SetCurrentDamage( entity.GetCurrentDamage() + entity.damage * damageReduction/100f);
    }
 
}
