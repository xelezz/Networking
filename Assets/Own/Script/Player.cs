using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnMessageCountChanged))]
    int messageCount = 0;
    void HandleMovement()
    {
        if (isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal * .1f, moveVertical *.1f, 0);
            transform.position = transform.position + movement;
        }
    }

    private void Update()
    {
        HandleMovement();
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Message to server");
            Message();
        }
        if (isServer && transform.position.y > 50)
        {
            PlayerHeight();
        }
    }

    public override void OnStartServer()
    {
        Debug.Log("Player has been spawned on the server");
    }

    [Command]
    void Message()
    {
        Debug.Log("Message from Client!");
        messageCount += 1;
        ReplyMessage();
    }

    [TargetRpc]
    void ReplyMessage()
    {
        Debug.Log("Message from Server!");
    }

    [ClientRpc]
    void PlayerHeight()
    {
        Debug.Log("Player is to high up!");
    }

    public void OnMessageCountChanged(int oldCount, int newCount)
    {
        Debug.Log($"We had {oldCount} messages, now we have {newCount} messages");
    }
}
