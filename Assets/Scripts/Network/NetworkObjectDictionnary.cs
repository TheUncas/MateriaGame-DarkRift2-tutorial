using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkObjectDictionnary
{
    /// <summary>
    /// Dictionnary that contains all gameobjects spawnable
    /// </summary>
    private static readonly Dictionary<int, string> dictionnary = new Dictionary<int, string>
    {
        {1, "BouncyBall" } 
    };

    /// <summary>
    /// Returns the specified object name 
    /// </summary>
    /// <param name="pID"></param>
    /// <returns></returns>
    public static string GetResourcePathFor(int pID)
    {
        string objectName;
        dictionnary.TryGetValue(pID, out objectName);
        return objectName;
    }
}
