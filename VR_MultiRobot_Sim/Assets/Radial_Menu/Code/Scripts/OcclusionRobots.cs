using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionRobots : MonoBehaviour
{
    [Header("Occluded Parts")]
    public List<GameObject> RobotParts;
    public List<float> Regions;

    [Header("Occlusion Indicator")]
    public RectTransform Indicator;

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < RobotParts.Count; i++){
            if(Regions.Count > i){
                if(Regions[i] <= Indicator.localPosition.y  && RobotParts[i].activeSelf)
                    RobotParts[i].SetActive(false);
                else if (Regions[i] > Indicator.localPosition.y  && !RobotParts[i].activeSelf)
                    RobotParts[i].SetActive(true);
            }
        }
        
    }
}
