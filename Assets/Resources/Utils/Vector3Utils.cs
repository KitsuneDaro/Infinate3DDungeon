using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Utils
{
    public static Vector3 Floor(Vector3 vector3)
    {
        return new Vector3(Mathf.Floor(vector3.x), Mathf.Floor(vector3.y), Mathf.Floor(vector3.z));
    }

    public static Vector3 GetFractionalPart(Vector3 vector3)
    {
        return vector3 - Vector3Utils.Floor(vector3);
    }

    public static Vector3 Repeat(Vector3 vector3, Vector3 repeatingVector3)
    {
        return new Vector3(Mathf.Repeat(vector3.x, repeatingVector3.x), Mathf.Repeat(vector3.y, repeatingVector3.y), Mathf.Repeat(vector3.z, repeatingVector3.z));
    }
}
