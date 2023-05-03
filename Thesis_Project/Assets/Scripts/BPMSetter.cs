using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BPMSetter : MonoBehaviour
{
    private static BPMSetter _BPMSetterInstance;
    public float bpm;
    private float beatInterval, beatTimer, beatIntervalDB, beatTimerDB;
    public static bool beatFull, beatDB;
    public static int beatCountFull, beatCountDB;

    //tap variable
    public float[] tapTime = new float[4];
    public static int tapCount;
    public static bool isMakingCustomBeat;

    //UI elements
    [SerializeField]
    public Text promptText;
    [SerializeField]
    public SongLoader clipPlayer;
    [SerializeField]
    public AudioSource countDownChime;
    [SerializeField]
    public AudioClip sCountdownSound;



    string defaultUIText = "The Beat Counter will give you a count of 4 before playing the clip back. It gives you a brief moment to get into the rhythm. \nTo create a beat count, click the 'Begin Counting Button'";
    string makeBeatText = "The song will play on the count of 4 taps. \nTap along to the beat of the music with the space bar 4 times.";
    
    string testTempo = "Make sure the cube is flashing colors to the beat of the clip. \nIf it doesn't flash on beat, click 'Begin Counting' to make a new Beat Counter. \nIf you are happy with it, click 'Finish'";
    

    private void Awake()
    {
        promptText.text = defaultUIText;
        if(_BPMSetterInstance != null && _BPMSetterInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _BPMSetterInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Tapping();
    }

    void Tapping()
    {
        //this is triggered by menu button to make custom counting beat
        if (Input.GetKeyUp(KeyCode.F1))
        {
            isMakingCustomBeat = true;
            tapCount = 0;
        }

        if (isMakingCustomBeat)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (tapCount < 4)
                {
                    tapTime[tapCount] = Time.realtimeSinceStartup;
                    tapCount++;
                }
                if (tapCount == 4)
                {
                    float averageTime = ((tapTime[1] - tapTime[0]) + (tapTime[2] - tapTime[1]) + (tapTime[3] - tapTime[2])) / 3;
                    bpm = (float)System.Math.Round((double)60 / averageTime, 2);
                    tapCount = 0;
                    beatTimer = 0;
                    beatTimerDB = 0;
                    beatCountFull = 0;
                    beatCountDB = 0;
                    isMakingCustomBeat = false;
                }
            }
        }
    }

    void BeatDetection()
    {
        //full beat count
        beatFull = false;
        beatInterval = 60 / bpm;
        beatTimer += Time.deltaTime;
        if (beatTimer >= beatInterval)
        {
            beatTimer -= beatInterval;
            beatFull = true;
            beatCountFull++;
            //print("full");
        }
        //divided beat count
        beatDB = false;
        beatIntervalDB = beatInterval / 8;
        beatTimerDB += Time.deltaTime;
        if (beatTimerDB >= beatIntervalDB)
        {
            beatTimerDB -= beatIntervalDB;
            beatDB = true;
            beatCountDB++;
        }
    }

    public void CreateCustomBeat()
    {
        promptText.text = makeBeatText;

        StartCoroutine("ClipCountDown");
       
    }

    IEnumerator ClipCountDown()
    {
        int counter = 3;
        while (counter > 0)
       {
            yield return new WaitForSeconds(1);
            countDownChime.PlayOneShot(sCountdownSound);
            counter--;
        };

        yield return new WaitForSeconds(1);
        clipPlayer.playClip();
        isMakingCustomBeat = true;
        tapCount = 0;

    }
}
