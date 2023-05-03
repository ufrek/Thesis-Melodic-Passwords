using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RecordPasword : MonoBehaviour
{

    public static bool isRecording = false;
    static float startTime;
    static float endTime;
    static List<KeyStroke> password = new List<KeyStroke>();
    static int minPassLength = 3;
    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    public static void beginRecord()
    {
        PracticeMgr.setHasPracticed(false);  //new passwords must be practiced again before confirmation
        password.Clear();
        isRecording = true;
        startTime = Time.time;  //get begin time
    }

    public static void stopRecord()
    {
        isRecording = false;
        endTime = Time.time;
        PassMaster.forceStopKeyRecord();
        AccountDetails account = UIDisplayManager.s.getAccount();
        account.setPass(password);
        //printPass();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static float getRecordStartTime()
    {
        return startTime;
    }

    public static float getRecordEndTime()
    {
        return endTime;
    }

    public static void addNote(KeyStroke k)
    {
        password.Add(k);
    }

 

    private static void printPass()
    {
        print("Start Time: " + startTime);
        foreach(KeyStroke k in password)
        {
            print("ID: " + k.getID());
            print("Impact Time: " + k.getImpactTime());
            print("Duration: " + k.getDuration());

        }
        print("End Time: " + endTime);
    }

    public static int getPassLength()
    {
        return password.Count;
    }

    public static int getMinPassLength()
    {
        return minPassLength;
    }


}
