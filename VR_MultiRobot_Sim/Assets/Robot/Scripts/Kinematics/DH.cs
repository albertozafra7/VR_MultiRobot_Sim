using System;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

using UnityEngine;

public static class DH {

    [Serializable]		// Makes possible to convert the class Param to a bunch of data
	// The Denavit-Hartenberg main parameters (https://en.wikipedia.org/wiki/Denavit%E2%80%93Hartenberg_parameters)
    public class Param
    {
        public float D;
        public float Theta;
        public float A;
        public float Alpha;
        
        public JointType Type;	// Only available Revolute and Prismatic joints
    }

	// Defines the Forward Kinematics method for the current robot from a list with the dhParams
    public static Matrix4x4 FK(Vector<float> jointValues, List<Param> dhParams, bool firstDTheta = true, bool lastAAlpha = true)
    {
        Matrix4x4 forward = Matrix4x4.identity;
        for (int i = 0; i < dhParams.Count; i++)
            forward *= FK(jointValues[i], dhParams[i], i > 0 || firstDTheta, i < dhParams.Count - 1 || lastAAlpha);
			// The forward kinematics matrix will be an identity matrix with the data value of the calculation in the next method

        return forward;
    }

	// Returns the 
    public static Matrix4x4 FK(float jointValue, Param dhParam, bool dTheta = true, bool aAlpha = true)
    {
		// codition ? consequent : alternative
		/*
			if (condition)
				consequent;
			else
				alternative;
		*/
		
		// If dTheta == true ---> d = dhParam.D + (result); else ---> d = 0;
        float d = dTheta ? dhParam.D + (dhParam.Type == JointType.Prismatic ? jointValue : 0) : 0;
        float theta = dTheta ? dhParam.Theta + (dhParam.Type == JointType.Revolute ? jointValue : 0) : 0;
        float a = aAlpha ? dhParam.A : 0;
        float alpha = aAlpha ? dhParam.Alpha : 0;

        float st = Mathf.Sin(theta);
        float ct = Mathf.Cos(theta);
        float sa = Mathf.Sin(alpha);
        float ca = Mathf.Cos(alpha);

		// Normal solution Denavit-Hartenberg
        return new Matrix4x4()
        {
            m00 = ct, m01 = -st * ca, m02 = st * sa, m03 = a * ct,
            m10 = st, m11 = ct * ca, m12 = -ct * sa, m13 = a * st,
            m20 = 0, m21 = sa, m22 = ca, m23 = d,
            m30 = 0, m31 = 0, m32 = 0, m33 = 1
        };
    }
    
	// ¿Makes a vector of two points that allows the robot to move over this direction? ---> Investigate
    public static Vector<float> GetFreeMode(Param dhParam)
    {
        switch(dhParam.Type)
        {
            case JointType.Revolute: return new DenseVector(new float[] { 0, 0, 1, 0, 0, 0 });
            case JointType.Prismatic: return new DenseVector(new float[] { 0, 0, 0, 0, 0, 1 });
        }
        throw new ArgumentException($"DH-Parameters does not support JointType: {dhParam.Type}");
    }

	// Calculates the Jacobian of the robot
    public static Matrix<float> Jacobian(Vector<float> jointValues, List<Param> dhParams)
    {
        // Frame k is attached to the tool with global rotation
        Matrix4x4 k = FK(jointValues, dhParams);		// Uses the forward kinematics for its calculations
        k.m03 = 0; k.m13 = 0; k.m23 = 0;
        
        Matrix<float> J = new DenseMatrix(6, 6);
        Matrix<float> X = Kinematics.SpatialTransform(k);
        for (int i = dhParams.Count - 1; i >= 0; --i)
        {
            Param dhParam = dhParams[i];
            float jointValue = jointValues[i];
            X *= Kinematics.InverseSpatialTransform(FK(jointValue, dhParam));
            J.SetColumn(i, X * GetFreeMode(dhParam));
        }

        return J;
    }

	// Calculates the Jacobian of the robot, depending of the tool
    public static Matrix<float> JacobianWithTool(Vector<float> jointValues, List<Param> dhParams, Matrix4x4 tool)
    {
        // Frame k is attached to the tool with global rotation
        Matrix4x4 k = tool;							// Uses the tool values
        k.m03 = 0; k.m13 = 0; k.m23 = 0;
        Matrix4x4 dh2k = FK(jointValues, dhParams).inverse * tool;

        Matrix<float> J = new DenseMatrix(6, 6);
        Matrix<float> X = Kinematics.SpatialTransform(k);
        for (int i = dhParams.Count - 1; i >= 0; --i)
        {
            Param dhParam = dhParams[i];
            float jointValue = jointValues[i];
            if (i == dhParams.Count - 1)
                X *= Kinematics.InverseSpatialTransform(dh2k);
            X *= Kinematics.InverseSpatialTransform(FK(jointValue, dhParam));
            J.SetColumn(i, X * GetFreeMode(dhParam));
        }

        return J;
    }
}
