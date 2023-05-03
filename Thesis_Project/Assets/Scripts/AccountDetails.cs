using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountDetails : MonoBehaviour
{

    //TODO: make password variables
    private string username;
    private AudioClip selectedSong;
    private float clipBegin;
    private float clipEnd;
    double beatInterval;

    private List<KeyStroke> pass;
    
    // Start is called before the first frame update
    void Start()
    {
        pass = new List<KeyStroke>();
        username = "";

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void setUserName(string name)
    {
        username = name;
    }

    public string getUserName()
    {
        return username;
    }

    public void setClipDetails(AudioClip clip, float begin, float end)
    {
        selectedSong = clip;
        clipBegin = begin;
        clipEnd = end;
    }

    public void setSelectedClip(AudioClip song)
    {
        selectedSong = song;
    }

    public float[] getClipEndpoints()
    {
        float[] endpts = new float[2];
        endpts[0] = clipBegin;
        endpts[1] = clipEnd;
        return endpts;
    }

    public void setBeatInterval(double b)
    {
        beatInterval = b;
    }

    public double getBeatInterval()
    {
        return beatInterval;
    }

    public AudioClip getSelectedClip()
    {
        return selectedSong;
    }

    public void setPass(List<KeyStroke> p)
    {
        pass = p;
    }
    public List<KeyStroke> getPass()
    {
        return pass;
    }
}
