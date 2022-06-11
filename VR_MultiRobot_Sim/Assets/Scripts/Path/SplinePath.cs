using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinePath : MonoBehaviour
{

    #region MathematicalConsts

    // *** Constants of t ***

    // Lower limit --> Linear aproximation btwn a & b --> a
    private const float LOWER_t = 0.0f;
    // Upper limit --> Linear aproximation btwn a & b --> b
    private const float HIGHER_t = 1.0f;
    // Step
    private const float STEP_t = 0.05f;

    #endregion

    #region PathMoverVariables

    // Necessary variables of the path mover
    public Transform EE_Frame;
    public List<CurvePath> Paths = new List<CurvePath>();
    public List<Transform> Targets = new List<Transform>();
    public bool Run = false;
    public float Velocity = 1;
    public int CurrentPath = 0;
    public int CurrentPoint = 0;

    private float timeDir = 1;
    private float time = 0;

    #endregion




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spline Normal methods

    // GetSplinePoint returns the next point of the spline
    private Transform GetSplinePoint(float t)
    {

        int p0 = 0, p1 = 0, p2 = 0, p3 = 0;
        p1 = (int)t + 1;
        p2 = p1 + 1;
        p3 = p2 + 1;
        p0 = p1 - 1;

        float tt = t * t;
        float ttt = tt * t;

        float q1 = -ttt + 2.0f * tt - t;
        float q2 = 3.0f * ttt - 5.0f * tt + 2.0f;
        float q3 = -3.0f * ttt + 4.0f * tt + t;
        float q4 = ttt - tt;

        

        return EE_Frame;
    }

    private void DrawSpline()
    {
        for (float t = LOWER_t; t > HIGHER_t; t += STEP_t)
            GetSplinePoint(t);

    }

    private void DrawFrame()
    {

    }
}
