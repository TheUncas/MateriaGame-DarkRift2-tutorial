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

    #endregion

    #region Unity Callbacks
    private void Start()
    {
        //////////////////
        /// Properties initialization
        clientsId = new List<int>();
        serverReference = GetComponent<XmlUnityServer>();

        //////////////////
        /// Events subscription
        serverReference.Server.ClientManager.ClientConnected += ClientConnected;
        serverReference.Server.ClientManager.ClientDisconnected += ClientDisconnected;

        //////////////////
        /// Load the game scene
        SceneManager.LoadScene("MainGameScene", LoadSceneMode.Additive);

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
    }

    /// <summary>
    /// When a client disconnects to the DarkRift server
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
    {
        throw new NotImplementedException();
    }
    #endregion

}
