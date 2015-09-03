using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_SyncPosition : NetworkBehaviour
{
    [SyncVar] //automatically transmit this value to all clients when it changes
    private Vector3 syncPos;

    [SerializeField]  Transform player_Transform;
    [SerializeField]  float lerpRate = 15;

    private Vector3 lastPos;
    private float threshold = 0.1f;


    void Update()
    {
        LerpPosition();
    }


	void FixedUpdate () 
	{
        TransmitPosition();
	}

    void LerpPosition()
    {
        if(!isLocalPlayer)
        {
            player_Transform.position = Vector3.Lerp(player_Transform.position, syncPos, Time.deltaTime * lerpRate); //if others players in game we wont their positions to smooth
        }
    }

     [Command] //this will only run on the server but it will call it from the client
     void CmdProvidePositionToServer(Vector3 pos) //retruns a vector3 position
    {
        syncPos = pos;
    }

    [ClientCallback]
    void TransmitPosition() //supplys the server with the position of the player
    {
        if(isLocalPlayer && Vector3.Distance(player_Transform.position,lastPos) > threshold) // only sends movement data if the player has actually moved
        {
            CmdProvidePositionToServer(player_Transform.position);
            lastPos = player_Transform.position;
        }
    }
}