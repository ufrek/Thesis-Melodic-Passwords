using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopTheshold : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        PasswordTiming p = collision.gameObject.GetComponent<PasswordTiming>();
       // PasswordTiming.addPossibleNote();
        p.KeyIsInTime(p.getID(), true); //user can press input now
        


    }

    private void OnTriggerExit(Collider collision)
    {
        PasswordTiming p = collision.gameObject.GetComponent<PasswordTiming>();
        p.NoteOver(p.getID());

    }
}
