using DarkRift.Client.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientManager : MonoBehaviour
{

    #region Properties

    /// <summary>
    /// Reference to the DarkRift2 client
    /// </summary>
    public UnityClient clientReference;

    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        //////////////////
        /// Properties initialization
        clientReference = GetComponent<UnityClient>();

        //////////////////
        /// Load the game scene
        SceneManager.LoadScene("MainGameScene", LoadSceneMode.Additive);
    }
    #endregion
}
