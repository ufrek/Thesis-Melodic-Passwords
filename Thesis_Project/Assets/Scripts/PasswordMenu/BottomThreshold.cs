using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomThreshold : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)   //NOTE: bottom collider is set to allow a bit of leeway by only covering the bottom half of the bar
    {
        PasswordTiming p = collision.gameObject.GetComponent<PasswordTiming>();

        p.KeyIsInTime(p.getID(), false);
   
    }

    private void OnTriggerExit(Collider collision)
    {
        GameObject g = collision.gameObject;
       
    }
}
