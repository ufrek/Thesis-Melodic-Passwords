using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTrack : MonoBehaviour
{
    public static bool isTrackOn = true;
    [SerializeField] private Image[] trackBackgroundElements; //track and threshold graphics
    [SerializeField] private GameObject beatMaster;
    // Start is called before the first frame update
    public void toggleTrack()
    {
        isTrackOn = !isTrackOn;
        
        foreach (Image i in trackBackgroundElements)
        {
            i.enabled = isTrackOn;
        }

        Image[] beats = beatMaster.GetComponentsInChildren<Image>();
        foreach (Image i in beats)
        {
            i.enabled = isTrackOn;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
