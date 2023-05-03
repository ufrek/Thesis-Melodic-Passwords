using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonMover : MonoBehaviour
{
    public static ButtonMover s;

    private float slerpT = 0;    //used for values between 0 and 1 for slerp function when moving buttons around
    bool isTransitioning = false;
    bool transitionDirection = false;   //false to start from center, true to start from edge


    [SerializeField] private GameObject[] mainButtons;
    [SerializeField] private Vector3[] centerButtonPositions;
    [SerializeField] private Vector3[] endButtonPositions;
    [SerializeField] private GameObject playClipButton;
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private float slideSpeed = 2;
    [SerializeField] private float delayAmt = .15f;

    [Header("Mode Menu Elements")]
    [SerializeField] private GameObject FreePlayMenuElements;
    [SerializeField] private GameObject RecordMenuElements;
    [SerializeField] private GameObject PracticeMenuElements;

    private GameObject currentMenu = null;

    private void Awake()
    {
         s = this;
    }
    // Start is called before the first frame update
    void Start()
    {
          
    }

    //TODO: enable all other menu elements when mode changes
    public void TransitionMenu(int oldMode, int newMode)
    {
       
        float delay = 0;
        if (oldMode == -1)
        {
            isTransitioning = true;
            

        }
        else
        {
            if (currentMenu != null)
            {
                currentMenu.gameObject.SetActive(false);
                if (currentMenu == RecordMenuElements || currentMenu == PracticeMenuElements)
                {
                    playClipButton.SetActive(false);
                    confirmButton.SetActive(false);
                }
            }
            transitionDirection = true;
            isTransitioning = true;

        }

        //update currentMenu for turning on and off menus
        switch (newMode)
        {
            case 0:
                currentMenu = RecordMenuElements;
                break;
            case 1:
                currentMenu = FreePlayMenuElements;
                break;
            case 2:
                currentMenu = PracticeMenuElements;
                break;
            default:
                print("invalid Menu Option");
                break;
        }
    }

    IEnumerator slideButton(Vector3 beginPosition, Vector3 endPosition, GameObject button, float delay)
    {
        yield return new WaitForSeconds(delay);
        float t = 0;
        while (t < 1)
        {
            transform.position = Vector3.Slerp(beginPosition, endPosition, t);
            yield return null;
            t += (Time.deltaTime * slideSpeed);
        }
    }


    
    // Update is called once per frame
    void Update()
    {
        if (isTransitioning)
        {
            if (transitionDirection == false)  //from center to edge
            {
                if (slerpT < 1)
                {
                    for (int i = 0; i < mainButtons.Length; i++)
                    {
                        mainButtons[i].transform.localPosition = Vector3.Slerp(centerButtonPositions[i], endButtonPositions[i], slerpT);
                    }

                    slerpT += (Time.deltaTime * slideSpeed);
                }
                else
                {
                    slerpT = 0;
                    isTransitioning = false;
                    currentMenu.SetActive(true);
                    playClipButton.SetActive(true);

                    if (currentMenu == PracticeMenuElements)
                    {
                        if (PracticeMgr.s.getInitializationStatus() == false)
                        {
                            PracticeMgr.s.initialize();
                        }

                        PracticeMgr.s.loadPracticeElements();
                    }

                    if (currentMenu == RecordMenuElements || currentMenu == PracticeMenuElements)
                    {
                        confirmButton.SetActive(true);
                    }
                   
                }
            }
            else
            {
               
                    if (slerpT < 1)
                    {
                        for (int i = 0; i < mainButtons.Length; i++)
                        {
                            mainButtons[i].transform.localPosition = Vector3.Slerp( endButtonPositions[i], centerButtonPositions[i], slerpT);
                        }

                        slerpT += (Time.deltaTime * slideSpeed);
                    }
                    else
                    {
                        slerpT = 0;
                        transitionDirection = false;


                    }
                
            }

        }
    }
}
