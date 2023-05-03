using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

//NOT USED: Check numericalKeys instead


[RequireComponent(typeof(AudioSource))]
/*Frequency List:
 * C4 = 262
 * C#4 = 277.18
 * D4 = 294
 * D#4 = 311
 * E4 = 330
 * F4 = 349
 * F#4 = 370
 * G4 = 392
 * G#4 = 415.3
 * A4 = 440
 * A#4 = 466
 * B4 = 494
 * C5 = 523
 * 
*/





public class PianoKeys : MonoBehaviour
{
    //musical variables
    public string keyLetter = "a";
    public float frequency = 262;  //set in inspector

    float downTime = 0;
    float downAuthTime = 0;
    int currentIndex;
    int currentAuthIndex;
    public AudioSource aS;
    AudioClip pianoKey;



    //button variables
    Graphic targetGraphic;
    Color normalColor;
    Button button;

    KeyStroke keyPress;

    bool isRecordingInput = false;

   static int currentMode = 1; //default to freePlay
    int recordingIndex;

    void Awake()
    {
        button = GetComponent<Button>();
        button.interactable = false;
        targetGraphic = GetComponent<Graphic>();

        ColorBlock cb = button.colors;
        cb.disabledColor = cb.normalColor;
        button.colors = cb;
    }

    // Start is called before the first frame update
    void Start()
    {
        aS = this.GetComponent<AudioSource>();

        int sampleFreq = 44000;
       

        float[] samples = new float[44000];
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = Mathf.Sin(Mathf.PI * 2 * i * frequency / sampleFreq);
        }
        pianoKey = AudioClip.Create("Test", samples.Length, 1, sampleFreq, false);
        pianoKey.SetData(samples, 0);
        aS.clip = pianoKey;
        aS.loop = true;

        button = GetComponent<Button>();
        button.targetGraphic = null;
        keyUp();

    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(keyLetter))
        {
            //aS.Play();
            keyDown();

            
            if (currentMode == 0) //recording mode
            {
                isRecordingInput = true;
                float impactTime = Time.time - RecordPasword.getRecordStartTime();  //impact time is in relation to the beginning of the clip
                keyPress = new KeyStroke(impactTime, keyLetter);
                
                // downTime = Time.time;
               // PassMaster.NotePlayed(this);   //old implementation
                
              
            }

            if (currentMode == 2) //practice mode
            {
               
                downAuthTime = Time.time;
                PassMaster.NoteAuthPlayed(this);


            }

            //controller.registerInput(keyLetter);

        }
        if (Input.GetKeyUp(keyLetter))
        {
            aS.Stop();
            keyUp();
            if (currentMode == 0)
            {
                if (RecordPasword.isRecording)
                {
                    isRecordingInput = false;
                    float duration = Time.time - keyPress.getImpactTime() - RecordPasword.getRecordStartTime();
                    keyPress.setDuration(duration);
                    RecordPasword.addNote(keyPress);
                    //PassMaster.NotePlayed(this);
                }

                
                //PassMaster.NoteReleased(currentIndex, Time.time - downTime);  //old implementation
                
            }
           

            if (currentMode == 2)
            {
                if (isRecordingInput && RecordPasword.isRecording == false)   //force keystroke ending
                {
                    //float duration = RecordPasword.getRecordEndTime() - keyPress.getImpactTime()  ;
                    isRecordingInput = false;

                }
                   
                
                //PassMaster.NoteAuthReleased(currentAuthIndex, Time.time - downAuthTime);

            }


        }

    }

    public void forceStopRecord()
    {
        if (isRecordingInput)
        {
            float duration = RecordPasword.getRecordEndTime() - keyPress.getImpactTime() - RecordPasword.getRecordStartTime();
            keyPress.setDuration(duration);
            RecordPasword.addNote(keyPress);
        }
        isRecordingInput = false;
    }

    public void setCurrentNoteIndex(int index)
    {
        currentIndex = index;
    }

    public void setCurrentNoteAuthIndex(int index)
    {
        currentAuthIndex = index;
    }

    public static  void changeMode(int mode)
    {
        currentMode = mode;
    }


    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool Beep(uint dwFreq, uint dwDuration);

    public static void PlayBeep(uint iFrequency, uint iDuration)
    {
        Beep(iFrequency, iDuration);
    }

    public void PlayKey(uint iDur)
    {
        PlayBeep((uint)frequency, iDur);
    }





    //color change code

    public void keyUp()
    {
        StartColorTween(button.colors.normalColor, false);
    }

    void keyDown()
    {
        StartColorTween(button.colors.pressedColor, false);
        button.onClick.Invoke();
    }

    void StartColorTween(Color targetColor, bool instant)
    {
        if (targetGraphic == null)
            return;

        targetGraphic.CrossFadeColor(targetColor, instant ? 0f : button.colors.fadeDuration, true, true);
    }

    public string getKeyID()
    {
        return keyLetter;
    }
}
