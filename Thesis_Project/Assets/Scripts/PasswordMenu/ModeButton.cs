using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//has an ID for identifying which mode is selected
public class ModeButton : MonoBehaviour
{

    static int currentMode = -1;  // 0 = record mode, 1 = free play, 2 = practice
    public int buttonID = -1; //sanity check, to prevent unset button
    public bool isPressed = false;

    private static ExplanationMgr expMgr;
    [SerializeField]
    public static RecordPasword recordMgr;
    private static SongLoader audioPlayer;
    [SerializeField]
    public static AuthButton authB;
    private static AccountDetails account;
    //button variables
    Graphic targetGraphic;
    Color normalColor;
    Button button;
    bool isRecError = false;
    bool isRecAuthError = false;

    static bool isRecording = false;
    static bool startRecord = false;

    //works the same as above to record authentication input
    static bool hasPassword = false;
    static bool isRecAuth = false;
    static bool startRecAuth = false;

     void Awake()
    {
        GameObject mgr = GameObject.FindGameObjectWithTag("UI");
        audioPlayer = mgr.GetComponent<SongLoader>();
        account = mgr.GetComponent<AccountDetails>();
        expMgr = GameObject.Find("ExplanationMenuMgr").GetComponent <ExplanationMgr > ();
        button = GetComponent<Button>();
        targetGraphic = GetComponent<Graphic>();

        ColorBlock cb = button.colors;
        cb.disabledColor = cb.normalColor;
        button.colors = cb;


    }
    // Start is called before the first frame update
    void Start()
    {

        button = GetComponent<Button>();
        button.targetGraphic = null;

        keyUp();

       if (buttonID == currentMode)
            keyDown();

        
    }
    public void onClick() //TODO: Rearrange how this works
    {
       
        
        if (currentMode == 0  && buttonID != 0)//checks for incorrect button operation while recording
        {
            //Debug.Log("error");
            if (audioPlayer.checkPlayStatus())
                audioPlayer.playClipCountdown();   
            //PassMaster.clearPassword();
            RecordPasword.stopRecord();

            isRecError = true;
            //isRecording = false;
            //startRecord = false;

          

        }

        if (currentMode == 1 && buttonID != 1)  //transition from free play
        {
            if(audioPlayer.checkPlayStatus())
                audioPlayer.playClipCountdown();


        }

        if (currentMode == 2 && buttonID != 2)//checks for incorrect button operation while authenticating
        {
            //Debug.Log(" Auth error");
   
            if (audioPlayer.checkPlayStatus())
                audioPlayer.playClipCountdown();
            //PassMaster.clearAuth();
            //authB.onStop();
            //isRecAuthError = true;
             //be sure to set in inspector for OnClick event
        }

        PassMaster.selectMode(buttonID);  //be sure to set in inspector for OnClick event, handles all mode change operations



        //update current mode by buttonID
        if (currentMode != buttonID)
            isPressed = true;
        else
            isPressed = false;

  //record mode implementation
        if (!isRecording && buttonID == 0)//begins record here
        {
            //PassMaster.clearPassword();
              // startRecord = true; //tells us we started recording
               UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

             
            
           
        }
           

        //implement stop recording
        if (isRecording && buttonID == 0 && !startRecord)  
        {
           // print(isRecording);
            //isRecording = false;
            //stop recording
            //recB.onStop();
            //PassMaster.selectMode(1);
           // PassMaster.printPassword();
            //PassMaster.confirmPassword();
        }

        if (startRecord && buttonID == 0)
        {
           // startRecord = false; //begin record off
            //isRecording = true; //enter record mode
        }


        if (isRecError)//sets prompt again after initial select Mode change
        {
          
           // PassMaster.recError();
            //isRecError = false;
            //startRecord = false; //reset due to mode change
            //isRecording = false;
            
        }


        //signIn Mode implementation
        if (!isRecAuth && buttonID == 2)//begins record here
        {
           // PassMaster.clearAuth();
            //startRecAuth = true; //tells us we started recording
            //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

           // authB.onRecord();


        }


        //implement stop recording
        if (isRecAuth && buttonID == 2 && !startRecAuth)  //
        {
            //print("auth clicked");
           // isRecAuth = false;
            //stop recording
            //authB.onStop();
            //PassMaster.selectMode(1);
            // PassMaster.printPassword();
            //PassMaster.confirmAuthPassword();
        }

        if (startRecAuth && buttonID == 2)
        {
           // startRecAuth = false; //begin record off
            //isRecAuth = true; //enter record mode
        }


        if (isRecAuthError)//sets prompt again after initial select Mode change
        {

           // PassMaster.recError();
            //isRecAuthError = false;
            //startRecAuth = false; //reset due to mode change
            //isRecAuth = false;

        }
    }

    public static void changeMode(int mode)
    {
        ButtonMover.s.TransitionMenu(currentMode, mode);
        currentMode = mode;


    }
    public static int getCurrentMode()
    {
        return currentMode;
    }

    public static void setHasPassword(bool b) { hasPassword = b; }

    public static bool getHasPassword() { return hasPassword; }

    //color change code

    public void keyUp()
    {
        StartColorTween(button.colors.normalColor, false);
    }

    public void keyDown()
    {
        StartColorTween(button.colors.pressedColor, false);
       
    }

    void StartColorTween(Color targetColor, bool instant)
    {
        if (targetGraphic == null)
            return;

        targetGraphic.CrossFadeColor(targetColor, instant ? 0f : button.colors.fadeDuration, true, true);
    }
}
