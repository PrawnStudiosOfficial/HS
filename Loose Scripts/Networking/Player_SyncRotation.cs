using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_SyncRotation : NetworkBehaviour
{
    [SyncVar] private Quaternion syncPlayerRotation;  // [syncVar] syncs the variable across the network
    [SyncVar] private Quaternion syncCamRotation; //syncs the rotation of the palyers camera

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform camTransform;
    [SerializeField] private float lerpRate = 15;

    private Quaternion lastPlayerRot;
    private Quaternion lastCamRot;
    private float threshold = 5f;

    void Start () 
	{	
	
	}

	void FixedUpdate () 
	{
        TransmitRotations();
        LerpRotations();
	}

    void LerpRotations() //smooths rotations
    {
        if(!isLocalPlayer)
        {
            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
            camTransform.rotation = Quaternion.Lerp(camTransform.rotation, syncCamRotation, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void CmdProvideRotationsToServer(Quaternion playerRot, Quaternion camRot) //sends info to server
    {
        syncPlayerRotation = playerRot;
        syncCamRotation = camRot;
    }

    [Client]
    void TransmitRotations()
    {
        if (isLocalPlayer)
        {
            if(Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > threshold || Quaternion.Angle(camTransform.rotation, lastCamRot) > threshold) // if the player has rotated more than the threshold then send the command to update position
            {
                CmdProvideRotationsToServer(playerTransform.rotation, camTransform.rotation);
                lastPlayerRot = playerTransform.rotation;
                lastCamRot = camTransform.rotation;
            }
        }
    }
}