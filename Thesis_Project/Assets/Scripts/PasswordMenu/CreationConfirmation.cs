using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreationConfirmation : MonoBehaviour
{
    [SerializeField]
    UIDisplayManager menuMgr;
    [SerializeField]
    private Button[] creationMenuButtons;  //set all buttons in manager
    [SerializeField]
    private Toggle trackToggle;

    public void setButtonsInteractable(bool b)
    {
        foreach (Button button in creationMenuButtons)
        {
            button.interactable = b; 
        }
        trackToggle.interactable = b;
    }

    public void ContinueAnyways()
    {
        PracticeMgr.setHasPracticed(true);
        menuMgr.creationToAccountConfirmation();
    }

    public void Back()
    {
        setButtonsInteractable(true);
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
