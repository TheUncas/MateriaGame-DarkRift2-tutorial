using DarkRift;
using DarkRift.Server;
using UnityEngine;

public class NetworkBouncyBall : NetworkObject
{

    #region Properties
    public Rigidbody rigidbodyReference;
    #endregion

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        ////////////////////////////////////
        // Get references
        rigidbodyReference = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        //If we are on server side
        if (!Equals(GameServerManager.instance, null))
        {
            if (GameServerManager.instance.currentTick % 10 == 0)
                SendBallPositionToClients();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Send ball server position to all clients
    /// </summary>
    private void SendBallPositionToClients()
    {
        //Create the message
        BouncyBallSyncMessageModel bouncyBallPositionMessageData = new BouncyBallSyncMessageModel(
            base.id,
            GameServerManager.instance.currentTick,
            rigidbodyReference.transform.position,
            rigidbodyReference.velocity);

        //create the message 
        using (Message m = Message.Create(
            NetworkTags.InGame.BOUNCY_BALL_SYNC_POS,        //Tag
            bouncyBallPositionMessageData)                  //Data
        )
        {
            foreach (IClient client in GameServerManager.instance.serverReference.Server.ClientManager.GetAllClients())
            {
                client.SendMessage(m, SendMode.Reliable);
            }
        }
    }
}
