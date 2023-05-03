using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using NAudio;
using NAudio.Wave;
using UnityEngine.Networking;
using TMPro;
using System;
using SimpleFileBrowser;
using MinMax_Slider;
//NOTE: CHANGE DEFAULT PATH OF MUSIC BANKS IN musicBankPath variable, line 29

//Code Modified from post by  DanielSRRosky1999 at: https://answers.unity.com/questions/652919/music-player-get-songs-from-directory.html
//Uses Runtime File Browser by yasirkula from Unity Asset Store: https://assetstore.unity.com/packages/tools/gui/runtime-file-browser-113006#description

[RequireComponent(typeof(AudioSource))]
public class SongLoader : MonoBehaviour
{
    public string filePath;

    private bool isPreviewing = false;

    [SerializeField] private string musicBankPath = null;   //set to null to give no preferred beginning path

 
    private AudioSource audioSource;
    private float clipStartTime;
    private float clipEndTime;

    [SerializeField]
    private TMP_Text t_selectedClip; //set in inspector
    [SerializeField]
    private TMP_Text t_songName;
    [SerializeField]
    private MinMaxSlider slider;
    [SerializeField]
    private Metronome beatCounter;
    
    [Header("Recording Variables")]
    [SerializeField]
    private AudioClip beatSound;
    [SerializeField]
    private Text playButtonText;        //this is the text of the play button in the clip beat menu
    private bool isBeatMenuActive = false;

    [Header("Password Creation Variables")]
    [SerializeField]
    private Text creationPlayButtonText;
    [SerializeField]
    private Text freeCountText;        //used on Freeplay and Record modes to count down
    [SerializeField]
    private Text countDownText;       //used on Practice and Log in modes to count down in the center of the screen
    private bool isCreationMenuActive = false;
    bool isPlayingCreation = false;
    bool countDownFinished = false;

    [Header("Login Variables")]
    [SerializeField]
    private Text loginCountDownText;
    [SerializeField]
    private Text loginPlayButtonText;
    [SerializeField]
    private bool isLoginMenuActive = false;


    Color originalSelectionColor;
    bool isColorFound = false;

    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
       
    }

    //Called in OnClick() Event for "Upload Button"
    //Opens a file explorer window and allows users to select a song to load into the program for password analysis
    public void ReadSongs(bool clipEdittable)
    {
        if (isColorFound == false)
        {
            originalSelectionColor = t_selectedClip.color;
            isColorFound = true;
        }
            
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Songs", ".mp3", ".wav"));
       // FileBrowser.SetDefaultFilter(".mp3");
       
       StartCoroutine(ShowLoadDialogCoroutine(clipEdittable));
    }

    //IMPORTANT: Change default path from D://Music to null on final build
    IEnumerator ShowLoadDialogCoroutine(bool isEdittable)
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, false, musicBankPath, null, "Select Sound", "Select"); //Change D://Music to null on final build

        if (FileBrowser.Success)
        {
            byte[] SoundFile = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            yield return SoundFile;
            audioSource.clip = NAudioPlayer.FromMp3Data(SoundFile);
            t_selectedClip.color = originalSelectionColor;
            t_selectedClip.text = "Selected Song: " + FileBrowserHelpers.GetFilename(FileBrowser.Result[0]);

            if (isEdittable)
            {
                slider.SetLimits(0, audioSource.clip.length);
                slider.SetValues(0, audioSource.clip.length);
                //makes a persistent data file Path if needed
                //string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
                //FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
            }



            //audioSource.Play();
        }
    }

    public void initilizeClipSelectionElements(string songName)
    {
        slider.SetLimits(0, audioSource.clip.length);
        slider.SetValues(0, audioSource.clip.length);
        t_songName.text = songName;
        //t_selectedClip = slider.gameObject.GetComponent<TMP_Text>();
        //t_selectedClip.text = "Selected Song: " + audioSource.clip.name;
    }

    public void PreviewClip()
    {
        
        if (audioSource.clip != null)
        {
            if (isPreviewing)
            {
                audioSource.Stop();
                isPreviewing = false;
            }
            else
            {
                clipStartTime = slider.GetMinValue();
                clipEndTime = slider.GetMaxValue();

                audioSource.time = clipStartTime;
                audioSource.Play();
                isPreviewing = true;
               
            }
        }
        else 
        {
            t_selectedClip.text = "Please upload a song first";
        }
      
    }

    public void PlaySong()
    {
        if (audioSource.clip != null)
        {
            if (audioSource.isPlaying)
            {
                if(beatCounter.isActiveAndEnabled)
                    beatCounter.resetPlayStatus();
                
                audioSource.Stop();

            }
            else
                audioSource.Play();
        }
        else
        {
            t_selectedClip.text = "Please upload a song first";
        }
           
    }

    public void playClip()
    {
   
        if (audioSource.clip != null)
        {
            if (isPreviewing)
            {
                audioSource.Stop();
                isPreviewing = false;
            }
            else
            {
                clipStartTime = slider.GetMinValue();
                clipEndTime = slider.GetMaxValue();
                updateClipEndpoints();
                audioSource.time = clipStartTime;
                audioSource.Play();
                isPreviewing = true;

            }
        }
    }

    //current bugs: loop doesn't start back to the beginning of the selected clip, but continues in the song
    //clip endpoints are only updated when preview clip is clicked...
    public void PlayClipWithEndpts(float begin,  float end)   
    {
        if (audioSource.clip != null)
        {
            if (isPreviewing)
            {
                audioSource.Stop();
                audioSource.loop = false;           //set back to false just in case for later
                isPreviewing = false;
            }
            else
            {
               
                clipStartTime = begin;
                clipEndTime = end;
                audioSource.time = clipStartTime;
                //audioSource.loop = true;
                audioSource.Play();
                isPreviewing = true;

            }
        }
    }

    public void playClipCountdown()
    {
        int mode = ModeButton.getCurrentMode();
        if (mode == 0 || mode == 1)
        {
            if (isPlayingCreation)
            {
                freeCountText.text = "";
                audioSource.Stop();
                RecordPasword.stopRecord();
                playButtonText.text = "Play Clip";
                isPlayingCreation = false;
                isPreviewing = false;
                countDownFinished = false;
            }
            else
            {
                isPlayingCreation = true;
                playButtonText.text = "Stop";
                StartCoroutine(countDownPlay());
            }
        }
        else if (mode == 2)
        {
            
            if (isPlayingCreation)
            {
                countDownText.text = "";
                audioSource.Stop();
                PracticeMgr.s.setPlayState(false, false);
                playButtonText.text = "Play Clip";
                isPlayingCreation = false;
                isPreviewing = false;
                countDownFinished = false;
            }
            else
            {
                PracticeMgr.s.loadPracticeElements();
                isPlayingCreation = true;
                playButtonText.text = "Stop";
                StartCoroutine(countDownPlay());
            }
        }
        else
        {
            print("Error, Illegal mode for play button");
        }
      
    }

    public void playClipLogin()
    {
        if (isPlayingCreation)
        {
            loginCountDownText.text = "";
            audioSource.Stop();
            LogInMenu.s.setPlayState(false, false);
            loginPlayButtonText.text = "Play Clip";
            isPlayingCreation = false;
            isPreviewing = false;
            countDownFinished = false;
        }
        else
        {
            LogInMenu.s.loadPracticeElements();
            isPlayingCreation = true;
            loginPlayButtonText.text = "Stop";
            StartCoroutine(countDownLogin());
        }
    }

    IEnumerator countDownLogin()
    {
        //get all necessary clip information, including interval between beats

        int count = 0;
        AccountDetails account = UIDisplayManager.s.getAccount();
        float beatInterval = (float)account.getBeatInterval();

        AudioClip clip = account.getSelectedClip();
        float[] endpts = account.getClipEndpoints();

        audioSource.clip = beatSound;
        audioSource.time = 0;

      
        loginCountDownText.text = "4";
        
       
        LogInMenu.s.setPlayState(true, false);  //count with the bars moving
        
        while (count < 4)
        {
            audioSource.Stop();
            audioSource.time = 0;
            audioSource.Play();
            yield return new WaitForSeconds(beatInterval);
            count++;

            int currentCountdown = 4 - count;
          
            if (currentCountdown == 0)
            {
                loginCountDownText.text = "Go!";
            }

            else
                loginCountDownText.text = (4 - count).ToString();
            

        }
        audioSource.Stop();
        audioSource.time = 0;
        countDownFinished = true;
        audioSource.clip = clip;

      
        loginCountDownText.text = "";
        LogInMenu.s.stertPracticeTime();
        

        PlayClipWithEndpts(endpts[0], endpts[1]);
    }

    IEnumerator countDownPlay()
    {
        //get all necessary clip information, including interval between beats
       
        int count = 0;
        AccountDetails account = UIDisplayManager.s.getAccount();
        float beatInterval = (float)account.getBeatInterval();
        
        AudioClip clip = account.getSelectedClip();
        float[] endpts = account.getClipEndpoints();

        audioSource.clip = beatSound;
        audioSource.time = 0;

        int currentMode = ModeButton.getCurrentMode();
        if (currentMode == 0 || currentMode == 1)
        {
            freeCountText.text = "4";
        }
        else //practice or log in mode
        {
            countDownText.text = "4";
        }
        if (currentMode == 2 || currentMode == 4)  //practice mode is running
        {
            PracticeMgr.s.setPlayState(true, false);  //count with the bars moving
        }
        while (count < 4)
        {
            audioSource.Stop();
            audioSource.time = 0;
            audioSource.Play();
            yield return new WaitForSeconds(beatInterval);     
            count++;
            
            int currentCountdown = 4 - count;
            if (currentMode == 0 || currentMode == 1)
            {
                if (currentCountdown == 0)
                {
                    freeCountText.text = "Go!";
                }

                else
                    freeCountText.text = (4 - count).ToString();
            }
            else if (currentMode == 2 || currentMode == 4)
            {
                if (currentCountdown == 0)
                {
                    countDownText.text = "Go!";
                }

                else
                    countDownText.text = (4 - count).ToString();
            }
           
        }
        audioSource.Stop();
        audioSource.time = 0;
        countDownFinished = true;
        audioSource.clip = clip;

        //modecheck before playback
        if (currentMode == 0)  // record mode is running
        {
            RecordPasword.beginRecord();
        }
        else if (currentMode == 2 || currentMode == 4)   //start practice time after the countDown, super important
        {
            countDownText.text = "";
            PracticeMgr.s.stertPracticeTime();
        }
       
            PlayClipWithEndpts(endpts[0], endpts[1]);
        
    }

    public void updateClipEndpoints()
    {
        UIDisplayManager.s.setClipInfo(audioSource.clip, clipStartTime, clipEndTime);
        
    }

 



    private void Update()
    {
     
        if (isPreviewing && audioSource.time >= clipEndTime)
        {
           
            
            audioSource.Stop();
            isPreviewing = false;

            if (isCreationMenuActive && creationPlayButtonText.isActiveAndEnabled && isPlayingCreation && countDownFinished) //case for preview menu
            {
                int mode = ModeButton.getCurrentMode();
                if (mode == 0)
                    RecordPasword.stopRecord();
                else if (mode == 2)
                    PracticeMgr.s.setPlayState(false, true); //when clip ends, set second flag to true to signify we can check for password

                //turn off count down text if clip is done playing
                if (mode == 0 || mode == 1)
                {
                    freeCountText.text = "";
                }
                else if (mode == 2 || mode == 4)
                {
                    countDownText.text = "";
                }
                creationPlayButtonText.text = "Play Clip";
                isPreviewing = false;
                isPlayingCreation = false;

                countDownFinished = false;
            }
            else if (isLoginMenuActive && isPlayingCreation && countDownFinished) //case for log in menu
            {
                LogInMenu.s.setPlayState(false, true);
                loginCountDownText.text = "";
                loginPlayButtonText.text = "Play Clip";
                isPreviewing = false;
                isPlayingCreation = false;

                countDownFinished = false;
            }


        }



        if (isBeatMenuActive && playButtonText.isActiveAndEnabled && isPreviewing && !audioSource.isPlaying)
        {
            playButtonText.text = "Play Clip";
            isPreviewing = false;
        }

       

       /* if (Input.GetKeyDown("a"))
        {
            ReadSongs();
        }*/
    }

    public void setBeatMenuActive(bool active)
    {
        isBeatMenuActive = active;
    }

    public void setCreationMenuActive(bool active)
    {
        isCreationMenuActive = active;
    }

    public void setLoginMenuAcitve(bool active)
    {
        isLoginMenuActive = active;
    }

    public bool checkPlayStatus()
    {
        return audioSource.isPlaying;
    }

}