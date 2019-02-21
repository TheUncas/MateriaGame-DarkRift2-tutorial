using DarkRift;
using DarkRift.Server;
using DarkRift.Server.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class GameServerManager : MonoBehaviourSingletonPersistent<GameServerManager>
{

    #region Properties

    /// <summary>
    /// List of connected clients
    /// </summary>
    public List<int> clientsId;

    /// <summary>
    /// Reference to the DarkRift server
    /// </summary>
    public XmlUnityServer serverReference;

    /// <summary>
    /// List of objects handled by the server
    /// </summary>
    public List<NetworkObject> networkObjects;

    #endregion

    #region Unity Callbacks
    private void Start()
    {
        //////////////////
        /// Properties initialization
        clientsId = new List<int>();
        serverReference = GetComponent<XmlUnityServer>();
        networkObjects = new List<NetworkObject>();

        //////////////////
        /// Events subscription
        serverReference.Server.ClientManager.ClientConnected += ClientConnected;
        serverReference.Server.ClientManager.ClientDisconnected += ClientDisconnected;

        //////////////////
        /// Load the game scene
        SceneManager.LoadScene("MainGameScene", LoadSceneMode.Additive);

    }
    #endregion

    #region Implementation

    /// <summary>
    /// Use this function to add a network object that must be handle by the server
    /// </summary>
    /// <param name="pNetworkObject"></param>
    public void RegisterNetworkObject(NetworkObject pNetworkObject)
    {
        //Add the object to the list
        networkObjects.Add(pNetworkObject);
    }

    /// <summary>
    /// Send a message to the client to spawn an object into its scene
    /// </summary>
    /// <param name="pClient"></param>
    public void SendObjectToSpawnTo(NetworkObject pNetworkObject, IClient pClient)
    {
        //Spawn data to send
        SpawnMessageModel spawnMessageData = new SpawnMessageModel
        {
            networkID = pNetworkObject.id,
            resourceID = pNetworkObject.resourceId,
            x = pNetworkObject.gameObject.transform.position.x,
            y = pNetworkObject.gameObject.transform.position.y
        };

        //create the message 
        using (Message m = Message.Create(
            NetworkTags.InGame.SPAWN_OBJECT,                //Tag
            spawnMessageData)                               //Data
        ){
            //Send the message in TCP mode (Reliable)
            pClient.SendMessage(m, SendMode.Reliable);
        }
    }

    /// <summary>
    /// Send a message with all objects to spawn
    /// </summary>
    /// <param name="pClient"></param>
    public void SendAllObjectsToSpawnTo(IClient pClient)
    {
        foreach (NetworkObject networkObject in networkObjects)
            SendObjectToSpawnTo(networkObject, pClient);
    }

    #endregion


    #region Server events
    /// <summary>
    /// When a client connects to the DarkRift server
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ClientConnected(object sender, ClientConnectedEventArgs e)
    {
        clientsId.Add(e.Client.ID);

        //Send all objects to spawn
        SendAllObjectsToSpawnTo(e.Client);
    }

    /// <summary>
    /// When a client disconnects to the DarkRift server
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
    {
        clientsId.Remove(e.Client.ID);
    }
    #endregion

}
