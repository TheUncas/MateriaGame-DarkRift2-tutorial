using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Id of the network object
    /// </summary>
    [HideInInspector]
    public int id = 0;

    /// <summary>
    /// Resource identifier for the spawner
    /// </summary>
    public int resourceId;


    #endregion

    #region Unity Callbacks

    private void Start()
    {
        //If we are not on the server and id is not set, destroy the gameobject
        if (Equals(GameServerManager.instance, null) && id == 0)
        {
            Destroy(gameObject);
        }
        else if(!Equals(GameServerManager.instance, null))
        {
            // Get the instance id of the gameobject on the server scene
            id = GetInstanceID();
            //Register with the server
            GameServerManager.instance.RegisterNetworkObject(this);
            //Test for resource ID
            if (resourceId == 0)
                throw new System.Exception(string.Format("There is no resource id for {0} gameobject", name));
        }
    }
    #endregion
}

