using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayerObject : MonoBehaviour
{
    private Bigfoot player;
    public bool flipRotation = false;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Bigfoot>();
    }

    void Update ()
    {
        var lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        if(flipRotation) { lookPos *= -1; }
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3);
    }
}
