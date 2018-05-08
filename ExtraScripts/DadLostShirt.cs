using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadLostShirt : MonoBehaviour {

    private SubtitleManager sManager_ref;

	// Use this for initialization
	void Start () {
        sManager_ref = FindObjectOfType<SubtitleManager>();
	}
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<DadResponse>() != null)
        {
            sManager_ref.PlaySubtitle(8);
            Destroy(this.gameObject);
        }
    }
}
