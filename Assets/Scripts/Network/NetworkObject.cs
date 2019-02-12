using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Id of the network object
    /// </summary>
    [HideInInspector]
    public int id;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        //If we are not on the server, destroy the gameobject
        if(Equals(GameServerManager.instance, null))
        {
            Destroy(gameObject);
        }
        else
        {
            // Get the instance id of the gameobject on the server scene
            id = GetInstanceID();
            //Register with the server
            //GameServerManager.instance.RegisterNetworkObject(this);
        }
    }

    #endregion
}