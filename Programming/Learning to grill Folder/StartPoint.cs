using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour {

    // Use this for initialization
    [HideInInspector]
    public TransitionCamera transitionCam;
    [HideInInspector]
    public GameObject bigfoot;
    [HideInInspector]
    public LearningGrillManager LGM;
	void Start ()
    {
       bigfoot = FindObjectOfType<Bigfoot>().gameObject;
       LGM = FindObjectOfType<LearningGrillManager>();
	}

    void Update()
    {
        if (transitionCam == null)
        {
            transitionCam = FindObjectOfType<TransitionCamera>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null)
        {
            if (!LGM.InStart)
            {
                LGM.InStart = true;
                LGM.StartMiniGame();
            }
            
            //this.gameObject.SetActive(false);
        }
    }
}
