using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions 
{
    public static Vector3 ForcedNormalization(this Vector3 vector)
    {
        Vector3 vec = Vector3.zero;
        if(vector.x == 0) { vec.x = 0; }
        else { vec.x = vector.x > 0 ? 1 : -1;}
        if (vector.y == 0) { vec.y = 0; }
        else { vec.y = vector.y > 0 ? 1 : -1; }
        if (vector.z == 0) { vec.z = 0; }
        else { vec.z = vector.z > 0 ? 1 : -1; }
        return vec;
    }
}
