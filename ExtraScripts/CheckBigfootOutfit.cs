using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBigfootOutfit : MonoBehaviour
{
    private Bigfoot bfoot_Ref;
    private SubtitleManager sManager;

	// Use this for initialization
	void Start ()
    {
        bfoot_Ref = FindObjectOfType<Bigfoot>();
        sManager = FindObjectOfType<SubtitleManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (bfoot_Ref.DisguiseState == BigfootDisguiseState.Disguised)
        {
            Destroy(this.gameObject);
        }
	}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null)
        {
            if (bfoot_Ref.DisguiseState == BigfootDisguiseState.Naked)
            {
                sManager.PlaySubtitle(1);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
