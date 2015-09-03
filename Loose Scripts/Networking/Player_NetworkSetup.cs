using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_NetworkSetup : NetworkBehaviour
{
    [SerializeField]
    Camera FPSCharacterCam;
    [SerializeField]
    AudioListener player_listener;


    void Start () 
	{
        if(isLocalPlayer)
        {
            GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            GetComponent<CharacterController>().enabled = true;
            FPSCharacterCam.enabled = true;
            player_listener.enabled = true;
        }
	}
}