using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//put on each key to key track of ID's and when the keys are in time or not
public class PasswordTiming : MonoBehaviour
{
    private string ID;
    private bool isInTime;
    private bool isBeingPlayed;     //true if keys are being pressed down while in time


    private static int correctNotes;
    private static int totalNotes;
    private bool isAdded = false; //prevents this note being added to the total multiple times



    public void setID(string str)
    {
        ID = str;
    }
    public string getID()
    {
        return this.ID;
    }

    public void KeyIsInTime(string id, bool status)
    {
        //ensures key is added only once when triggered
        if (status && isAdded == false)
        {
            isAdded = true; 
            PassPlaybackMgr.setKeyInTime(id, status, this.gameObject);

        }
        else if (status == false && isAdded == true)
        {
            PassPlaybackMgr.setKeyInTime(id, status);
            isAdded = false;
        }
       
    }

    public void NoteOver(string id)            //if you this the note on time, this tells you it's officially over and you can hit the next note
    {

        PassPlaybackMgr.CheckNotePlayed(id);
 
    }

}
