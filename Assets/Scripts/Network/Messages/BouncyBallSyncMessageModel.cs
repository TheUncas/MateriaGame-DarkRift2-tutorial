using DarkRift;
using UnityEngine;

public class BouncyBallSyncMessageModel : IDarkRiftSerializable
{
    #region Properties
    public int networkID;
    public int serverTick;
    public Vector3 position;
    public Vector3 velocity;
    #endregion

    #region IDarkRiftSerializable implementation
    public void Deserialize(DeserializeEvent e)
    {
        networkID = e.Reader.ReadInt32();
        serverTick = e.Reader.ReadInt32();
        position = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
        velocity = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
    }

    public void Serialize(SerializeEvent e)
    {
        //Write id of the network object
        e.Writer.Write(networkID);

        //Write id of the network object
        e.Writer.Write(serverTick);

        //Write position
        e.Writer.Write(position.x);
        e.Writer.Write(position.y);
        e.Writer.Write(position.z);

        //Write velocity
        e.Writer.Write(velocity.x);
        e.Writer.Write(velocity.y);
        e.Writer.Write(velocity.z);
    }
    #endregion
}
