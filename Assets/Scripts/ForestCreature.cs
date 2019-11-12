using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestCreature : AI
{
    public float age;
    public float lifeSpan;
    protected int hitPoint = 1;
    public bool isDead = false;

    public void DecreaseHP()
    {
        hitPoint--;
    }

    public void IncreaseHP()
    {
        hitPoint++;
    }
}
