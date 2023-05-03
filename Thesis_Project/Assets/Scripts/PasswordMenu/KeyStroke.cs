using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class KeyStroke
{
    private float impactTime;
    private string ID;
    private float duration;
    public KeyStroke(float impact, string ID)
    {
        impactTime = impact;
        this.ID = ID;
    }

    public void setDuration(float dur)
    {
        duration = dur;
    }

    public float getImpactTime()
    {
        return impactTime;
    }

    public string getID()
    {
        return ID;
    }

    public float getDuration()
    {
        return duration;
    }
}
