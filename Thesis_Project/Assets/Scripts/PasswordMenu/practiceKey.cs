using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class practiceKey : MonoBehaviour
{
    //[SerializeField]
    //private Transform hitMaster;      /////////////////SET THIS IN INSPECTOR     
    private float impactTime;
    private string ID;
    private float duration;

    float beatDistance;
    float scrollSpeed;
    private double durationLength;

    private static Dictionary<string, Color> colorList;     //Set this up

    public void  calculateKey(KeyStroke k)
    {
        impactTime = k.getImpactTime();
        ID = k.getID();
        duration = k.getDuration();

        beatDistance = PracticeMgr.s.getBeatDistance();
        scrollSpeed = PracticeMgr.s.getScrollSpeed();


        //TODO: 

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private Color getColorID(string hitID)
    {
        return Color.red;
    }

    private float getXOffset(string hitID)
    {
        return 1.1f;
    }

    
}
