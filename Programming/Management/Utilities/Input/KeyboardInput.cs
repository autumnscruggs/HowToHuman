using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class KeyboardInput
{
    //custom input handler holding various functions
    //just in case we ever want to change it, we don't have to change it everywhere

    public static bool IsAnyKeyDown()
    {
        return Input.anyKeyDown;
    }
    public static bool IsHoldingAnyKey()
    {
        return Input.anyKey;
    }

    //------------STRING OVERLOADS-----------------//
    public static bool IsKeyDown(string key)
    {
        return Input.GetKeyDown(key);
    }

    public static bool HasReleasedKey(string key)
    {
        return Input.GetKeyUp(key);
    }

    public static bool IsHoldingKey(string key)
    {
        return Input.GetKey(key);
    }

    public static bool IsKeyDownFromList(List<string> keys)
    {
        foreach (string key in keys)
        {
            if (IsKeyDown(key))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsHoldingKeyFromList(List<string> keys)
    {
        foreach (string key in keys)
        {
            if (IsHoldingKey(key))
            {
                return true;
            }
        }

        return false;
    }

    //-------------KEYCODE OVERLOADS-----------------//
    public static bool IsKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    public static bool HasReleasedKey(KeyCode key)
    {
        return Input.GetKeyUp(key);
    }

    public static bool IsHoldingKey(KeyCode key)
    {
        return Input.GetKey(key);
    }

    public static bool IsKeyDownFromList(List<KeyCode> keys)
    {
        foreach (KeyCode key in keys)
        {
            if (IsKeyDown(key))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsHoldingKeyFromList(List<KeyCode> keys)
    {
        foreach(KeyCode key in keys)
        {
            if(IsHoldingKey(key))
            {
                return true;
            }
        }

        return false;
    }
}
