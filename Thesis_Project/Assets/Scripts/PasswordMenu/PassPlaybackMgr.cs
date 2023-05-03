using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

//used to keep track of if keys are being pressed or not. Is set on the same object as Practice manager and Login manager
public class PassPlaybackMgr : MonoBehaviour
{
    private static bool[] isKeyInTime;
    private static bool[] isKeyBeingPlayed;
    private static bool[] isNoteOver;
    private static string[] keyCodes;
    private static GameObject[] currentNotes;
    private static int correctNotes;
    private static int totalNotes;
    private bool isAdded = false; //prevents this note being added to the total multiple times

   

    // Start is called before the first frame update
    void Start()
    {

    }

    //store all keyCode values to check in update
    public static void setupKeyCodes()
    {
        keyCodes = new string[13];
        keyCodes[0] = "[0]";
        keyCodes[1] = "[.]";
        keyCodes[2] = "enter";
        keyCodes[3] = "[1]";
        keyCodes[4] = "[2]";
        keyCodes[5] = "[3]";
        keyCodes[6] = "[4]";
        keyCodes[7] = "[5]";
        keyCodes[8] = "[6]";
        keyCodes[9] = "[7]";
        keyCodes[10] = "[8]";
        keyCodes[11] = "[9]";
        keyCodes[12] = "[+]";
    }

    public static void resetKeyStates()  //resets all states of keys being played as well as all correct and total note inputs found during playback
    {
        correctNotes = 0;
        totalNotes = 0;
        isKeyInTime = new bool[13]; //all playable keys are set to false
        isKeyBeingPlayed = new bool[13]; //all keys being played set to false
        isNoteOver = new bool[13];
        currentNotes = new GameObject[13];
        for (int i = 0; i < 13; i++)
        {
            currentNotes[i] = null;
            isNoteOver[i] = false;
            isKeyInTime[i] = false;
            isKeyBeingPlayed[i] = false;
        }
    }

    public static void setKeyInTime(string id, bool isInTime, GameObject g = null) //also manages correct and total notes since they are triggered when the trigger is hit
    {
        if (isInTime)  //if a new key is in time, add it to the total notes
        {
            totalNotes++;
        }
        switch (id)
        {
            case "[0]":
                if (isInTime == false && isKeyBeingPlayed[0] == true) //if timing is ended and key is being played, add to correct inputs
                {
                    //print("0 was correct");
                    correctNotes++;
                    resetNote(0);
                }
                else if(isInTime == false) //missed note
                {
                  //  print("0");
                    resetNote(0);

                }
                currentNotes[0] = g;
                isKeyInTime[0] = isInTime;
                break;
            case "[1]":
                if (isInTime == false && isKeyBeingPlayed[3] == true) //if timing is ended and key is being played, add to correct inputs
                {
                    //print("1 was correct");
                    correctNotes++;
                    resetNote(3);
                }
                else if (isInTime == false)  //missed note 
                {
                   // print("1");
                    resetNote(3);
                }
                currentNotes[3] = g;
                isKeyInTime[3] = isInTime;
                break;
            case "[4]":
               
                if (isInTime == false && isKeyBeingPlayed[6] == true) //if timing is ended and key is being played, add to correct inputs
                {
                   // print("4 was correct");
                    correctNotes++;
                    resetNote(6);
                }
                else if (isInTime == false)  //missed note 
                {
                  //  print("4");
                    resetNote(6);
                }
                currentNotes[6] = g;
                isKeyInTime[6] = isInTime;
                break;
            case "[7]":
                
                if (isInTime == false && isKeyBeingPlayed[9] == true) //if timing is ended and key is being played, add to correct inputs
                {
                   // print("7 was correct");
                    correctNotes++;
                    resetNote(9);
                }
                else if (isInTime == false)  //missed note 
                {
                  //  print("7");
                    resetNote(9);
                }
                currentNotes[9] = g;
                isKeyInTime[9] = isInTime;
                break;
            case "[.]":
               
                if (isInTime == false && isKeyBeingPlayed[1] == true) //if timing is ended and key is being played, add to correct inputs
                {
                   // print(". was correct");
                    correctNotes++;
                    resetNote(1);
                }
                else if (isInTime == false)  //missed note 
                {
                  //  print(".");
                    resetNote(1);
                }
                currentNotes[1] = g;
                isKeyInTime[1] = isInTime;
                break;
            case "[2]":
                
                if (isInTime == false && isKeyBeingPlayed[4] == true) //if timing is ended and key is being played, add to correct inputs
                {
                   // print("2 was correct");
                    correctNotes++;
                    resetNote(4);
                }
                else if (isInTime == false)  //missed note 
                {
                  //  print("2");
                    resetNote(4);
                }
                currentNotes[4] = g;
                isKeyInTime[4] = isInTime;
                break;
            case "[5]":
               
                if (isInTime == false && isKeyBeingPlayed[7] == true) //if timing is ended and key is being played, add to correct inputs
                {
                  //  print("5 was correct");
                    correctNotes++;
                    resetNote(7);
                }
                else if (isInTime == false)  //missed note 
                {
                  //  print("5");
                    resetNote(7);
                }
                currentNotes[7] = g;
                isKeyInTime[7] = isInTime;
                break;
            case "[8]":
                if (isInTime == false && isKeyBeingPlayed[10] == true) //if timing is ended and key is being played, add to correct inputs
                {
                  //  print("8 was correct");
                    correctNotes++;
                    resetNote(10);
                }
                else if (isInTime == false)  //missed note 
                {
                  //  print("8");
                    resetNote(10);
                }
                currentNotes[10] = g;
                isKeyInTime[10] = isInTime;
                break;
            case "enter":
                if (isInTime == false && isKeyBeingPlayed[2] == true) //if timing is ended and key is being played, add to correct inputs
                {
                   // print("enter was correct");
                    correctNotes++;
                    resetNote(2);
                }
                else if (isInTime == false)  //missed note 
                {
                   // print("enter");
                    resetNote(2);
                }
                currentNotes[2] = g;
                isKeyInTime[2] = isInTime;
                break;
            case "[3]":
                if (isInTime == false && isKeyBeingPlayed[5] == true) //if timing is ended and key is being played, add to correct inputs
                {
                   // print("3 was correct");
                    correctNotes++;
                    resetNote(5); ;
                }
                else if (isInTime == false)  //missed note 
                {
                  //  print("3");
                    resetNote(5);
                }
                currentNotes[5] = g;
                isKeyInTime[5] = isInTime;
                break;
            case "[6]":
                if (isInTime == false && isKeyBeingPlayed[8] == true) //if timing is ended and key is being played, add to correct inputs
                {
                   // print("6 was correct");
                    correctNotes++;
                    resetNote(8);
                }
                else if (isInTime == false)  //missed note 
                {
                   // print("6");
                    resetNote(8);
                }
                currentNotes[8] = g;
                isKeyInTime[8] = isInTime;
                break;
            case "[9]":
                if (isInTime == false && isKeyBeingPlayed[11] == true) //if timing is ended and key is being played, add to correct inputs
                {
                  //  print("9 was correct");
                    correctNotes++;
                    resetNote(11);
                }
                else if (isInTime == false)  //missed note 
                {
                   // print("9");
                    resetNote(11);
                }
                currentNotes[11] = g;
                isKeyInTime[11] = isInTime;
                break;
            case "[+]":
                if (isInTime == false && isKeyBeingPlayed[12] == true) //if timing is ended and key is being played, add to correct inputs
                {
                   // print("+ was correct");
                    correctNotes++;
                    resetNote(12);
                }
                else if (isInTime == false)  //missed note 
                {
                   // print("+");
                    resetNote(12);
                }
                currentNotes[12] = g;
                isKeyInTime[12] = isInTime;
                break;
            default:
                print("Invalid key was set to be playable, please check object that hit threshold");
                break;
        }
    }

    private static void resetNote(int index)
    {
        isKeyBeingPlayed[index] = false;
        isNoteOver[index] = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            for (int i = 0; i < isKeyInTime.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]) && isKeyInTime[i] == true)
                {
                    //print("hit");
                    isKeyBeingPlayed[i] = true;
                }
            }
        } 
    
            for (int i = 0; i < isKeyInTime.Length; i++)
            {
               
                //we have you registered as hitting the key in time, but you aren't holding it down long enough
                //makes it so you can't hit that key anymore
                if (isKeyBeingPlayed[i] == true && Input.GetKeyUp(keyCodes[i]) == true && isNoteOver[i] == false)
                {
                    print("miss");
                    isKeyInTime[i] = false;
                    isKeyBeingPlayed[i] = false;
                }

                //you have successfully played the note for the desired duration
                if (isKeyBeingPlayed[i] == true && isNoteOver[i] == true)
                {
                   // print("over");
                    isNoteOver[i] = false;
                    if (currentNotes[i] != null)
                    {
                        GameObject hit = currentNotes[i];
                        currentNotes[i] = null;
                        Destroy(hit);
                     
                    }
            
                    else
                        print("Error: current note was told to be destroyed but it is null");
                    setKeyInTime(keyCodes[i], false);
                }
                    
            }
               
                
            
        
    }

    public static bool checkPassPlaythrough()
    {
        print("Correct Notes: " + correctNotes);
        print("Total Notes: " + totalNotes);
        if (correctNotes == totalNotes)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void CheckNotePlayed(string id)
    {
        switch (id)
        {
            case "[0]":
                isNoteOver[0] = true;
             
                break;
            case "[1]":
                isNoteOver[3] = true;
               
                break;
            case "[4]":
                isNoteOver[6] = true;
   
                break;
            case "[7]":
                isNoteOver[9] = true;
         
                break;
            case "[.]":
                isNoteOver[1] = true;
         
                break;
            case "[2]":
                isNoteOver[4] = true;
      
                break;
            case "[5]":
                isNoteOver[7] = true;

                break;
            case "[8]":
                isNoteOver[10] = true;
     
                break;
            case "enter":
                isNoteOver[2] = true;
           
                break;
            case "[3]":
                isNoteOver[5] = true;
           
                break;
            case "[6]":
                isNoteOver[8] = true;
          
                break;
            case "[9]":
                isNoteOver[11] = true;
         
                break;
            case "[+]":
                isNoteOver[12] = true;
     
                break;
            default:
                print("Invalid key was set to be playable, please check object that hit threshold");
                break;
        }
    }

}
