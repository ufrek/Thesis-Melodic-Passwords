using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Used for displaying multiple explanation windows 
//Not user friendly so dropped
public class ExplanationMgr : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject[] explanations;
    int activeMenu = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void displayExplanation()
    {
        int hintID = ModeButton.getCurrentMode();
        mainMenu.SetActive(false);
            switch (hintID)
            {
                case 0:  //record mode
                    activeMenu = 0;
                    explanations[0].SetActive(true);
                    break;
                case 1:    //freeplay mode
                    activeMenu = 1;
                    explanations[1].SetActive(true);
                    break;
                case 2:   //practice mode
                    activeMenu = 2;
                    explanations[2].SetActive(true);
                    break;
                case -1:   //unselected mode, returns genearl explanation of modes
                    activeMenu = 3;
                    explanations[3].SetActive(true);
                    break;
               
                default:
                    print("Invalid hintID set");
                    break;
            }
    }

    public void backToMain()
    {
        
        mainMenu.SetActive(true);
        explanations[activeMenu].SetActive(false);
        activeMenu = -1;   //set back to invalid if accidental trigger
    }


    public void clearExpMenu()
    {
        foreach (GameObject g in explanations)
        {
            g.SetActive(false);
        }
    }

    
}
