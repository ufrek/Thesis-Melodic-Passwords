using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PracticeMgr : MonoBehaviour
{
    [SerializeField]
    public static PracticeMgr s;   //needs to be set in inspector as start() won't run due to being inactive at the beginning
    [SerializeField]
    private GameObject topThreshold;
    [SerializeField]
    private GameObject bottomThreshold;

    [Header("Beat Marker Variables")]
    [SerializeField]
    private GameObject beatPrefab;
    [SerializeField]
    private RectTransform beatStartPosition;
    [SerializeField]
    public GameObject beatMaster;
    private Vector3 beatMasterStartPos;
    [SerializeField]
    private AccountDetails account;
    [SerializeField]
    private float beatDistance;      //adjust this to scroll faster or slower
    [SerializeField]
    private float scrollSpeed;
    [SerializeField]
    private int extraBeatMarkers = 6;

    [Header("KeyStroke Variables")]
    [SerializeField]
    private GameObject[] keyHitPrefabs;  //set in inspector
    [SerializeField]
    private GameObject keyHitMaster;    //adjust this to scroll faster or slower
    [SerializeField]
    private float keyHitSlotOffsetX = 102;     //sets the offset for each slot off by this much
    //[SerializeField]
    //private RectTransform textOffset;         //set where the text should sit
    private Vector3 keyHitMasterStartPos;

    [SerializeField]
    private Text loginStatus;

    

    private Dictionary<string, Vector4> keyHitColors;

    private static List<GameObject> beatMarkers;
    private static List<KeyStroke> keyStrokeHits;
    private static List<GameObject> visibleKeyHits;

    private float[] clipEndpts;
    private float clipDuration;
    private double beatDuration;
    private float totalBeats;
    float thresholldMidPt;         //starting y position of beat markers

    bool isScrolling = false;
    bool isInitialized = false;
    private float practiceStartTime;   //needed to check time from beginning of practice playsession

    private static bool hasPracticed = false; //set to true ONLY when login was successful

    // Start is called before the first frame update
    void Awake()
    {
        s = this;
      
       
    }

    public void initialize()  //run only at event when practice mode is selected
    {
        buildKeyHitColors();
        PassPlaybackMgr.setupKeyCodes();
        loginStatus.text = "";
        thresholldMidPt = bottomThreshold.transform.position.y + ((topThreshold.transform.position.y - bottomThreshold.transform.position.y) / 2);  //get midpoint and beat start point
        beatMasterStartPos = beatMaster.transform.position;
        keyHitMasterStartPos = keyHitMaster.transform.position;
        beatStartPosition.position = new Vector3(beatStartPosition.position.x, thresholldMidPt, beatStartPosition.position.z);  //set midPoint between 2 threshold lines to be start pt
        beatMarkers = new List<GameObject>();
        keyStrokeHits = new List<KeyStroke>();
        visibleKeyHits = new List<GameObject>();
        keyStrokeHits = account.getPass();
        isInitialized = true;

    }

    //clears beat markers and puts new markers in starting positions
    private void loadBeatMarkers()
    {
        if (beatMarkers.Count != 0)
        {
            foreach (GameObject g in beatMarkers)
            {
                Destroy(g);
            }
            beatMarkers.Clear();
        }
           
        for (int i = -1; i < (int)totalBeats; i++)
        {
            GameObject g = Instantiate(beatPrefab,beatMaster.transform);  //make everything a child of one object that moves
            //g.GetComponent<RectTransform>().position = new Vector3(beatStartPosition.position.x, beatStartPosition.position.y + (beatDistance * (i + 1)), beatStartPosition.position.z);
            g.GetComponent<RectTransform>().position = new Vector3(g.transform.position.x, thresholldMidPt + (beatDistance * (i + 1)), g.transform.position.z);
            if (ToggleTrack.isTrackOn == false)
            {
                g.GetComponent<Image>().enabled = false;
            }
            beatMarkers.Add(g);

        }
       
    }

    private void loadMusicNotes()
    {
        foreach (GameObject h in visibleKeyHits)
        {
            Destroy(h);
        }
        visibleKeyHits.Clear();

        foreach (KeyStroke k in keyStrokeHits)
        {
       
            string ID = k.getID();
            GameObject g; 
            //make hit object and set which slot it sits in
            int slot;   // 1 - 4 for x offsets of different ID values
            switch (ID)
            {
                case "[0]":
                    g = Instantiate(keyHitPrefabs[0], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("[0]");
                    //g.GetComponentInChildren<Text>().text = "0";
                    slot = 0;
                    break;
                case "[1]":
                    g = Instantiate(keyHitPrefabs[3], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("[1]");
                    //g.GetComponentInChildren<Text>().text = "1";
                    slot = 0;
                    break;
                case "[4]":
                    g = Instantiate(keyHitPrefabs[6], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("[4]");
                    //g.GetComponentInChildren<Text>().text = "4";
                    slot = 0;
                    break;
                case "[7]":
                    g = Instantiate(keyHitPrefabs[9], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("[7]");
                    //g.GetComponentInChildren<Text>().text = "7";
                    slot = 0;
                    break;
                case "[.]":
                    g = Instantiate(keyHitPrefabs[1], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("[.]");
                    //g.GetComponentInChildren<Text>().text = ".";
                    slot = 1;
                    break;
                case "[2]":
                    g = Instantiate(keyHitPrefabs[4], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("[2]");
                    //g.GetComponentInChildren<Text>().text = "2";
                    slot = 1;
                    break;
                case "[5]":
                    g = Instantiate(keyHitPrefabs[7], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("[5]");
                    //g.GetComponentInChildren<Text>().text = "5";
                    slot = 1;
                    break;
                case "[8]":
                    g = Instantiate(keyHitPrefabs[10], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("[8]");
                    //g.GetComponentInChildren<Text>().text = "8";
                    slot = 1;
                    break;
                case "enter":
                    g = Instantiate(keyHitPrefabs[2], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("enter");
                    //g.GetComponentInChildren<Text>().text = "Ent";
                    slot = 2;
                    break;
                case "[3]":
                    g = Instantiate(keyHitPrefabs[5], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("[3]");
                    //g.GetComponentInChildren<Text>().text = "3";
                    slot = 2;
                    break;
                case "[6]":
                    g = Instantiate(keyHitPrefabs[8], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("[6]");
                    //g.GetComponentInChildren<Text>().text = "6";
                    slot = 2;
                    break;
                case "[9]":
                    g = Instantiate(keyHitPrefabs[11], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("[9]");
                    //g.GetComponentInChildren<Text>().text = "9";
                    slot = 2;
                    break;
                case "[+]":
                    g = Instantiate(keyHitPrefabs[12], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("[+]");
                    // g.GetComponentInChildren<Text>().text = "+";
                    slot = 3;
                    break;
                default:
                    print("Invalid ID");
                    g = Instantiate(keyHitPrefabs[0], keyHitMaster.transform);
                    g.GetComponent<PasswordTiming>().setID("");
                    slot = -1;
                    break;
            }
            
            if (ToggleTrack.isTrackOn == false)  //make invisible if toggled track is off
            {
                g.GetComponent<Image>().enabled = false;
            }

            //get necessary info from key hit 
            float impactTime = k.getImpactTime();
            float duration = k.getDuration();

            if ((impactTime + duration) > clipEndpts[1])  //if the recorded duration is longer than the end of the clip, cut it back to the end of the clip
            {
                duration = clipEndpts[1] - impactTime;
            }
            //Vector4 c = keyHitColors[ID];
            //g.GetComponent<Image>().color = new Color(c.x, c.y, c.z, c.w);

            RectTransform textOffset = g.GetComponent<RectTransform>();   //get the initial positioning of the text

           

            //get y offset
            float totalDistanceOffset = (4 * beatDistance) + (scrollSpeed * impactTime); //IMPORTANT: this is a count of 4 before the beat PLUS the scaled distance

            //scale height instead of edit height directly
            float originalHeight = g.GetComponent<RectTransform>().sizeDelta[1];
            
            //keep this no matter what
            float totalHeightDuration = scrollSpeed * duration; //get height stretched duration
            float scalingOffset = totalHeightDuration / originalHeight;
            
            Vector3 ogScale = g.GetComponent<RectTransform>().localScale;
            g.GetComponent<RectTransform>().localScale = new Vector3(ogScale.x, scalingOffset, ogScale.z);
            //g.GetComponent<RectTransform>().sizeDelta = new Vector2( g.GetComponent<RectTransform>().sizeDelta.x,totalHeightDuration);
            g.transform.position = new Vector3( g.transform.position.x + (slot * keyHitSlotOffsetX), totalDistanceOffset, g.transform.position.z);
            visibleKeyHits.Add(g);
            //g.AddComponent<Text>();   //this is harder to implement than I thought

        }
       
    }

    private void calculateClipDetails()
    {
        clipEndpts = account.getClipEndpoints();
        clipDuration = clipEndpts[1] - clipEndpts[0];
        beatDuration = account.getBeatInterval();
        totalBeats = (float)(clipDuration / beatDuration) + extraBeatMarkers;      // NOTE: Adding extra bars here for visual clarity
    }

    public void loadPracticeElements()  //happens everytime you click "Play Clip"
    {
        PassPlaybackMgr.resetKeyStates();  //make all notes playable to false
        loginStatus.text = "";
        calculateClipDetails();
        loadBeatMarkers();
        totalBeats -= extraBeatMarkers; //take the extra beats out once we make the extra count down markers
        scrollSpeed = (float)(totalBeats * beatDistance / clipDuration); //get the scroll speed for how fast the beats move
        loadMusicNotes();
    }

    public void setPlayState(bool isPlaying, bool clipEndedNaturally)
    {
        isScrolling = isPlaying;
        if (isPlaying == false)   //practice playthrough ahs ended, reset positions and check if password is correct
        {
            //if clip ended playing, check if the password is correct
            if (clipEndedNaturally && PassPlaybackMgr.checkPassPlaythrough())
            {
                loginStatus.text = "Login Successful!";
                hasPracticed = true;
            }
            else if(clipEndedNaturally && PassPlaybackMgr.checkPassPlaythrough() == false)
            {
                loginStatus.text = "Login Failed...\nPlease try again.";
            }

            beatMaster.transform.position = beatMasterStartPos;  //if false, reset position of all beats
            keyHitMaster.transform.position = keyHitMasterStartPos;
                        
            //NOTE reset all notes as well
        }
    }





    public void stertPracticeTime()
    {
        practiceStartTime = Time.time;
    }

    void Update()
    {
        if (isScrolling)
        {
            Vector3 pos = new Vector3(beatMaster.transform.position.x, beatMaster.transform.position.y - (Time.deltaTime * scrollSpeed), beatMaster.transform.position.z); //scroll down
            beatMaster.transform.position = pos;

            pos = new Vector3(keyHitMaster.transform.position.x, keyHitMaster.transform.position.y - (Time.deltaTime * scrollSpeed), keyHitMaster.transform.position.z);
            keyHitMaster.transform.position = pos;
            /*foreach(GameObject g in beatMarkers)
            {
                float originalPos = g.transform.position.y;
                originalPos -= (Time.fixedDeltaTime * scrollSpeed);
            }*/

            //put notes here as well
        }
        
    }

    public bool getInitializationStatus()
    {
        return isInitialized;
    }

    public float getBeatDistance()
    {
        return beatDistance;
    }

    public double getBeatDuration()
    {
        return beatDuration;
    }

    public float getScrollSpeed()
    {
        return scrollSpeed;
    }

    private float getHitStartingPt()
    {
        return thresholldMidPt;
    }

    public static bool getHasPracticed()
    {
        return hasPracticed;
    }



    public static void setHasPracticed(bool b)
    {
        hasPracticed = b;
    }

  

    private void buildKeyHitColors()
    {
        keyHitColors = new Dictionary<string, Vector4>();
        keyHitColors.Add("[0]", new Vector4(224, 111, 111, 255));
        keyHitColors.Add("[.]", new Vector4(248, 167, 167, 255));
        keyHitColors.Add("enter", new Vector4(243, 219, 219, 255));
        keyHitColors.Add("[1]", new Vector4(255, 181, 112, 255));
        keyHitColors.Add("[2]", new Vector4(253, 196, 144, 255));
        keyHitColors.Add("[3]", new Vector4(236, 210, 186, 255));
        keyHitColors.Add("[4]", new Vector4(142, 186, 241, 255));
        keyHitColors.Add("[5]", new Vector4(161, 195, 236, 255));
        keyHitColors.Add("[6]", new Vector4(185, 203, 226, 255));
        keyHitColors.Add("[7]", new Vector4(187, 128, 245, 255));
        keyHitColors.Add("[8]", new Vector4(197, 150, 243, 255));
        keyHitColors.Add("[9]", new Vector4(227, 204, 250, 255));
        keyHitColors.Add("[+]", new Vector4(250, 218, 250, 255));
    }

   
}
