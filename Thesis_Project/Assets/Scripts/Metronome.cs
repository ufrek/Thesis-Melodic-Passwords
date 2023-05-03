using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{

    [SerializeField]
    private Text prompt;

    private bool isPlaying = false;
    private float cubeScale = 0.01f;

    //emtronome variables
    private double bpm = 0F;           //calculated BPM output of the metronome
    public float currentTick = 0.0f;
    private float nextTick = 0.0F; // The next tick in dspTime
    private double sampleRate = 0.0F;
    private bool ticked = false;
    private float interval = 0;
    private float newClick = 0;
    private float prevClick = 0;
    private float lerpVal = 0;
    private float now = 0;          //time elapsed since current click
  
    
    //set in inspector
    public GameObject bouncingCube; //used as visual cue of how the timing is going
    public Button playClipButton;
    public Button doneButton;
    public SongLoader clipPlayer;

    //debug variables

    public float bounceScale = 1;
    public Color bounceColor;
    public float maxBounceScale = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void toggleCount()      //called in event system for button
    {
        bool playStatus = clipPlayer.GetComponent<AudioSource>().isPlaying;
        if ( playStatus == false)
        {
            isPlaying = true;
            playClipButton.GetComponentInChildren<Text>().text = "Stop";
        }
        else
        {
            isPlaying = false;
            playClipButton.GetComponentInChildren<Text>().text = "Play";
        }

        clipPlayer.playClip();                       //NOTE: maygbe change this to loop later
    }

    private void Update()
    {
        if (isPlaying && !Input.GetMouseButtonDown(0) && Input.anyKeyDown)
        {
//TODO: changge this later to play clip loop
                       
            
            prevClick = newClick;
            newClick = Time.time;

            if (prevClick != 0)
            {
                interval = newClick - prevClick;
                bpm = (double)60 / interval;    //probably not used
                newBeat();
            }
        }
        if (isPlaying && prevClick != 0)
        {
            now += Time.deltaTime;
            //print("Now " + now + ". Mext " + nextTick);
            if (now > nextTick)
            {
                
                newBeat();
            }
            else
            {
                //interpolate from max size and color down to min size and color over the calculated interval
                Vector3 yTemp = transform.localScale;
                lerpVal = 1 + (now - nextTick) / (nextTick - currentTick);      //normalizes current time to range from currenTick to next Tick (i.e. the blinks of the light
                //print("lerp Val " + lerpVal);

                bounceScale= Mathf.Lerp(1f, .01f, lerpVal);
                
                yTemp.y = bounceScale;
                bouncingCube.gameObject.transform.localScale = yTemp;

                float whiteScale = Mathf.Lerp(.01f, 1f, lerpVal);
                bounceColor = new Color(0, bounceScale, whiteScale, 1);       //interpolates the color back to white over time
                bouncingCube.gameObject.GetComponent<Image>().color = bounceColor;


            }
        }

        if (isPlaying && clipPlayer.GetComponent<AudioSource>().isPlaying == false)
        {
            isPlaying = false;
            playClipButton.GetComponentInChildren<Text>().text = "Play";
        }


    

    }
    public void resetPlayStatus()
    {
        isPlaying = false;
        playClipButton.GetComponentInChildren<Text>().text = "Play";

    }
    private void newBeat()
    {
        Vector3 yTemp = transform.localScale;
        bounceScale = 1;
        yTemp.y = bounceScale;
        bouncingCube.gameObject.transform.localScale = yTemp;
        bounceColor = Color.green;
        bouncingCube.gameObject.GetComponent<Image>().color = bounceColor;
        currentTick = Time.time;
        now = currentTick;
        nextTick = currentTick + interval;
    }

    public double getBeatIntervalM()
    {
        return interval;
    }

    public void resetPrompt()
    {
        prompt.color = Color.white;
        prompt.text = "Play your clip and tap any key along to the beat of the song.";
    }
    public void showWarning()
    {
        prompt.color = Color.red;
        prompt.text = "Please tap to the beat of the clip before continuing.";         
    }

    public void resetBPM()
    {
        bpm = 0;
    }

}
