using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LinearPath : MonoBehaviour
{

    public List<CurvePath> Paths = new List<CurvePath>();
    public Transform EE_Frame;
    public Transform FK_Frame;
    public bool Run = false;
    public bool circular = false;


    private int i = 1;
    private float t = 0;
    private float RedFactor = 0.0f;
    private float initDist = 0.0f;
    public float General_Speed = 1.0f;
    //private float time = 0;
    //private float initTime = 0.0f;

    void Start()
    {
        RedFactor = 1.0f;
        //initTime = Time.deltaTime;
        // time = initTime;
    }

    void Update()
    {
        if (Run)
        {
            if (i < Paths[0].TTargets.Length)
            {
                if (!circular)
                {
                    EE_Frame.position = Vector3.Lerp(Paths[0].TTargets[i - 1].position, Paths[0].TTargets[i].position, t);
                    EE_Frame.rotation = Quaternion.Lerp(Paths[0].TTargets[i - 1].rotation, Paths[0].TTargets[i].rotation, t);
                } else if(i < Paths[0].TTargets.Length-1)
                {
                    Vector3 L0 = Vector3.Lerp(Paths[0].TTargets[i - 1].position, Paths[0].TTargets[i].position, t);
                    Vector3 L1 = Vector3.Lerp(Paths[0].TTargets[i].position, Paths[0].TTargets[i+1].position, t);
                    EE_Frame.position = Vector3.Lerp(L0, L1, t);


                }

                if (t < 1.0f)
                {
                    if(Paths[0].Points[i].time == 0.0f)
                        t += (0.5f * General_Speed * Paths[0].Points[i].vel * Time.deltaTime / Vector3.Distance(Paths[0].TTargets[i - 1].position, Paths[0].TTargets[i].position)); // * (time - initTime);
                    else
                        t += (0.5f * General_Speed * Time.deltaTime / (Paths[0].Points[i].time/2));
                }
                else
                {
                    t = 0;
                    i++;
                    //initTime = time;
                }
                //Debug.Log(t);
                //time += Time.deltaTime;

            }
            else
            {
                if (Vector3.Distance(FK_Frame.position, EE_Frame.position) <= 0.001 && Vector3.Distance(FK_Frame.position, EE_Frame.position) >= -0.001)
                    i = 1;
                else
                {
                    EE_Frame.position = Paths[0].TTargets[0].position;
                    EE_Frame.rotation = Paths[0].TTargets[i - 1].rotation;
                }


            }
        }
    }

    void OnRenderObject()
    {
        Handles.color = Color.white;
        for (int i = 1; i < Paths[0].TTargets.Length; i++)
            Handles.DrawLine(Paths[0].TTargets[i - 1].position, Paths[0].TTargets[i].position);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 1; i < Paths[0].TTargets.Length; i++)
            Gizmos.DrawLine(Paths[0].TTargets[i - 1].position, Paths[0].TTargets[i].position);
    }
}
