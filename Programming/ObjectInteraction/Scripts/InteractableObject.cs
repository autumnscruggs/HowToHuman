using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum StarState { ShowOnTrigger, ShowAlways, ShowNever }

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
    [Header("Note: This object needs a trigger collider")]
    public string StarObjectPrefabName = "InteractableStar";
    public StarState StarState;
    public float yOffset = 2;
    private GameObject starObj;
    public List<UnityEvent> OnInteractionActions = new List<UnityEvent>();
    //Dependencies
    private PlayerInput input;

    private Sprite kbSprite;
    private Sprite gpSprite;
    private SpriteRenderer image;


    private void Awake()
    {
        //Throw error if it doesn't have a trigger collider
        List<Collider> colliders = this.gameObject.GetComponents<Collider>().ToList();
        if (colliders.Find(x => x.isTrigger) == null)
        {
            throw new Exception("The InteractableObject component on " + this.gameObject.name + " requires a TRIGGER collider to function properly.");
        }
        input = GameObject.FindObjectOfType<PlayerInput>();
    }

    private void Start()
    {
        kbSprite = Resources.Load<Sprite>("KBSprite");
        gpSprite = Resources.Load<Sprite>("GPSprite");

        if (this.transform.Find(StarObjectPrefabName) == null)
        {
            GameObject starPrefab = Resources.Load<GameObject>(StarObjectPrefabName);
            starObj = GameObject.Instantiate(starPrefab);
            starObj.transform.parent = this.transform;
            starObj.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y + yOffset, this.transform.position.z);
        }
        else
        {
            starObj = this.transform.Find(StarObjectPrefabName).gameObject;
        }

        image = starObj.GetComponent<SpriteRenderer>();

        if (StarState == StarState.ShowAlways) { ShowStar(true); }
        else { ShowStar(false); }
    }

    private void Update()
    {
        image.sprite = InputManager.ControllerType == ControlType.Keyboard ? kbSprite : gpSprite;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null)
        {
            if (StarState == StarState.ShowOnTrigger && starObj.gameObject.activeInHierarchy == false)
            {
                ShowStar(true);
            }

            if (input.InteractionInputPushed)
            {
                OnInteractionActions.ForEach(x => x.Invoke());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null)
        {
            if (StarState == StarState.ShowOnTrigger)
            {
                ShowStar(false);
            }
        }
    }

    public void ShowStar(bool show)
    {
        starObj.SetActive(show);
    }
}
