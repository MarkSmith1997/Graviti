using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {

    public static void SetPlayerRecursively(GameObject obj, int newLayer)
    {
        if(obj == null)
        {
            return;
        }
        obj.layer = newLayer;

        foreach(Transform child in obj.transform)
        {
            if(child == null)
            {
                continue;
            }
            SetPlayerRecursively(child.gameObject, newLayer);
        }
    }

    public static bool Toggle(bool input)
    {
        bool output = !input;
        return output;
    }
}
