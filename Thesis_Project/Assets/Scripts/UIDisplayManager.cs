using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(SongLoader))]
public class UIDisplayManager : MonoBehaviour
{
    public static UIDisplayManager s;               //Singleton variable if needed

    [SerializeField] private GameObject introScreen;


    [Header("Sign In Menu Elements")]
    [SerializeField] private GameObject signInUserNameScreen;
    [SerializeField] private InputField inputUserName;
    [SerializeField] private Text signInPrompt;


    [SerializeField] private GameObject newAccountScreen;


    [Header("New User Menu")]
    [SerializeField] private InputField inputNewUserName;
    [SerializeField] private InputField inputConfirmNewUserName;
    [SerializeField] private Text newUserPrompt;
    [SerializeField] private Text confirmUserPrompt;

    [Header("Clip Upload Menus")]
    [SerializeField] private GameObject songUploadMenu;
    [SerializeField] private Text uploadWelcomePrompt;
    [SerializeField] private TMP_Text selectedClipPrompt;
    [SerializeField] private GameObject clipSelectionMenu;
    [SerializeField] private TextMeshProUGUI clipDurationText;
    [SerializeField] private float clipDuration;
    private float durationMin = 5;
    private float durationMax = 15;
    private float clipStart;
    private float clipEnd;

    [Header("Clip Tempo Selection")]
    [SerializeField] private GameObject clipBeatMenu;

    [Header("Password Creation Menu")]
    [SerializeField] private GameObject passCreationMenu;
    [SerializeField] private GameObject accountConfirmationMenu;
    [SerializeField] private GameObject accountCreatedMenu;


    [Header("Created Account SignIn Menu")]
    [SerializeField] private GameObject passwordLoginMenu;
    [SerializeField] private InputField createdUserInputField;
    [SerializeField] private Text createdUserSignInPrompt;

    [Header("Login Success Menu")]
    [SerializeField] private GameObject loginSuccessMenu;
    [SerializeField] private Text userWelcomText;

    [SerializeField]
    private AccountDetails account;
    private GameObject currentMenu;
    private InputField currentUserInputField;
    private Text currentSignInPrompt;

    void Start()
    {
        s = this;
        currentMenu = introScreen;
        //account = this.gameObject.GetComponent<AccountDetails>();
    }

    public void changeMenu(GameObject currMenu, GameObject newMenu)
    {
        currMenu.SetActive(false);
        newMenu.SetActive(true);
        currentMenu = newMenu;
        currentSignInPrompt = signInPrompt;
    }

    //Navigates to a menu where the username is asked for
    public void SignInMenu()
    {
        currentUserInputField = inputUserName;
        currentSignInPrompt = signInPrompt;
        changeMenu(currentMenu, signInUserNameScreen);

    }

    public void newAccountMenu()
    {
        changeMenu(currentMenu, newAccountScreen);
        //signInUserNameScreen.SetActive(false);

    }

    public void signInToPasswordLogin()
    {
        string username = account.getUserName();
      
        if (username != "" && username.Equals(currentUserInputField.text))
        {
            changeMenu(currentMenu, passwordLoginMenu);
            GetComponent<SongLoader>().setLoginMenuAcitve(true);
            LogInMenu.s.initialize();
        }
        else 
        {
            currentSignInPrompt.text = "Invalid Username.";
        }
    }


    private bool validateUserName(InputField input, Text prompt)
    {
        bool validUserName = false;
        if (input.text.Length >= 6)
        {
            validUserName = true;
            prompt.text = "Please Enter New Username";
            prompt.color = Color.white;
        }
        else
        {
            prompt.color = Color.red;
            prompt.text = "Must be at least 6 characters";
        }

        if (input.text.ToLower().Equals("username"))
        {
            validUserName = false;
            prompt.color = Color.red;
            prompt.text = "Please type in a valid username.";
        }


        return validUserName;
    }

    public void EnterUserName()
    {
        if (validateUserName(inputUserName, signInPrompt))
        {
            account.setUserName(inputUserName.text);
            newAccountScreen.SetActive(false);
            //AnalysisMenu.SetActive(true);
        }
    }

    public void NewUserToSongUpload()
    {
        if (validateUserName(inputNewUserName, newUserPrompt) && inputNewUserName.text.Equals(inputConfirmNewUserName.text))
        {
            account.setUserName(inputNewUserName.text);
            changeMenu(currentMenu, songUploadMenu);
            uploadWelcomePrompt.text = "Welcome, " + inputNewUserName.text + "!\n\nSelect a song to build your password upon.";

            //clipSelectionMenu.SetActive(true);
        }
        else if (!inputNewUserName.text.Equals(inputConfirmNewUserName.text))
        {
            confirmUserPrompt.color = Color.red;
            confirmUserPrompt.text = "Usernames do not match.";
        }
        else
        {
            confirmUserPrompt.color = Color.white;
            confirmUserPrompt.text = "Confirm Username";
        }

    }

    public void UploadConfirmToClipSelection()
    {
        AudioSource audioSource = this.GetComponent<AudioSource>();
        if (audioSource.clip != null)
        {

            if (audioSource.clip.length < durationMin)
            {
                print(" not working");
                selectedClipPrompt.color = Color.red;
                selectedClipPrompt.text = "File must be at least " + durationMin + " seconds long to make a password";
            }
            else
            {
                if (audioSource.isPlaying)
                    audioSource.Stop();

                changeMenu(currentMenu, clipSelectionMenu);
                GetComponent<SongLoader>().initilizeClipSelectionElements(selectedClipPrompt.text);

            }
        }
    }

    public void clipSelectionToClipUpload()
    {
        GetComponent<AudioSource>().Stop();
        changeMenu(currentMenu, songUploadMenu);
       
    }

    public void clipSelectionToClipBeat()
    {

        if (clipDuration >= durationMin && clipDuration <= durationMax)
        {
            //print("next menu");
            GetComponent<AudioSource>().Stop();
            GetComponent<SongLoader>().updateClipEndpoints();
            GetComponent<SongLoader>().setBeatMenuActive(true);
            changeMenu(currentMenu, clipBeatMenu);
            clipBeatMenu.GetComponentInChildren<Metronome>().resetBPM();
            clipBeatMenu.GetComponentInChildren<Metronome>().resetPrompt();
        }
        else
        {
            print("invalid length");
            clipDurationText.color = Color.red;
        }
           
    }

    public void setClipDuration(float dur, float durMin, float durMax)
    {
        clipDuration = dur;
        durationMin = durMin;
        durationMax = durMax;
    }

    public void clipBeatnToCreationMenu()
    {
        
        GetComponent<AudioSource>().Stop();
        GetComponent<SongLoader>().setBeatMenuActive(false);
        double interval = clipBeatMenu.GetComponentInChildren<Metronome>().getBeatIntervalM();
        if (interval != 0)
        {
            account.setBeatInterval(interval);
            GetComponent<SongLoader>().setCreationMenuActive(true);                    //REMINDER: Be Sure to turn this off when changing menu
            changeMenu(currentMenu, passCreationMenu);
        }
        else
        {
            clipBeatMenu.GetComponentInChildren<Metronome>().showWarning();
        }
    }

    public void creationToAccountConfirmation()
    {
        if(PracticeMgr.getHasPracticed() == false)
        {
            accountConfirmationMenu.SetActive(true);
            accountConfirmationMenu.GetComponent<CreationConfirmation>().setButtonsInteractable(false);
        }
        else 
        {
            //ToggleTrack.isTrackOn = false;  //turn off track for log in menu    //THE TIMING IS WAY HARDER WITH THIS OFF. 
            changeMenu(currentMenu, accountCreatedMenu);
            GetComponent<SongLoader>().setCreationMenuActive(false);
            currentUserInputField = createdUserInputField;
            currentSignInPrompt = createdUserSignInPrompt;
            
        }
    }

    public void loadLoginSuccessMenu()
    {
        changeMenu(currentMenu, loginSuccessMenu);
        string userName = account.getUserName();
        userWelcomText.text = "Welcome, " + userName + "!";
    }

   

    public void setClipInfo(AudioClip clip, float start, float end)
    {
        account.setClipDetails(clip, start, end);
        clipStart = start;
        clipEnd = end;
    }

    public float[] getClipEndPoints()
    {
        float[] endpoints = new float[2];
        endpoints[0] = clipStart;
        endpoints[1] = clipEnd;
        return endpoints;

    }

    public AccountDetails getAccount()
    {
        return account;
    }

}
