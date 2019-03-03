using DarkRift.Client;
using DarkRift.Client.Unity;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class ClientManager : MonoBehaviourSingletonPersistent<ClientManager>
{

    #region Properties

    /// <summary>
    /// Reference to the DarkRift2 client
    /// </summary>
    public UnityClient clientReference;



    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        base.Awake();

        //////////////////
        /// Properties initialization
        clientReference = GetComponent<UnityClient>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //////////////////
        /// Load the game scene
        SceneManager.LoadScene("MainGameScene", LoadSceneMode.Additive);

        //////////////////
        /// Suscribe to events
        clientReference.MessageReceived += SpawnGameObjects;

        //////////////////
        /// Connect to the server manually
        clientReference.ConnectInBackground(
            IPAddress.Parse("127.0.0.1"),
            4296,
            DarkRift.IPVersion.IPv4,
            null
            );

    }


    #endregion

    #region Implementation 

    /// <summary>
    /// Spawn object if message received is tagged as SPAWN_OBJECT
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SpawnGameObjects(object sender, MessageReceivedEventArgs e)
    {
        if (e.Tag == NetworkTags.InGame.SPAWN_OBJECT)
        {
            //Get message data
            SpawnMessageModel spawnMessage = e.GetMessage().Deserialize<SpawnMessageModel>();

            //Spawn the game object
            string resourcePath = NetworkObjectDictionnary.GetResourcePathFor(spawnMessage.resourceID);
            GameObject go = Resources.Load(resourcePath) as GameObject;
            go.GetComponent<NetworkObject>().id = spawnMessage.networkID;
            Instantiate(go, new Vector3(spawnMessage.x, spawnMessage.y, 0), Quaternion.identity);
        }
    }

    #endregion
}
