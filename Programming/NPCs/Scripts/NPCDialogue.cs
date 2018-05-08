using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SoundManager))]
[RequireComponent(typeof(NPC))]
public class NPCDialogue : MonoBehaviour
{
    protected int currentClip = 0;

    //References
    protected SoundManager soundManager;
    protected NPCMovement npcMovement;

    public GameObject chatIcon;

    [SerializeField] private bool canBeInteractedWith = true;
    public bool CanOnlyInteractOnce = false;
    private int iconShownAmount = 0;

    private bool looked = false;
    private bool interacted = false;

    //References
    private NPC npc;
    private PlayerInput pInput;

    [SerializeField] private float dialogueTriggerRadius = 5f;
    public bool CanBeInteractedWith { get { return canBeInteractedWith; } set { canBeInteractedWith = value; } }


    public UnityEvent OnInteractionActions;

    protected virtual void Awake()
    {
        pInput = GameObject.FindObjectOfType<PlayerInput>();
        npcMovement = this.GetComponent<NPCMovement>();
        soundManager = this.GetComponent<SoundManager>();
        ShowChatIcon(false);
    }

    protected virtual void Start()
    {
        BoxCollider[] colliders = this.gameObject.GetComponents<BoxCollider>();
        foreach (BoxCollider c in colliders)
        {
            if (c.isTrigger)
            {
                c.size = new Vector3(dialogueTriggerRadius, c.size.y, dialogueTriggerRadius);
            }
        }
    }

    protected virtual void Update()
    {
        
    }

    public void ShowChatIcon(bool show)
    {
        if(show && !chatIcon.activeInHierarchy)
        { 
            if (CanOnlyInteractOnce)
            {
                if (iconShownAmount >= 1)
                {
                    CanBeInteractedWith = false;
                    return;
                }
            }
        }
      
        chatIcon.SetActive(show);

    }

    public virtual void PlayDialogue()
    {
        //TODO: play dialogue on timer or one long thing
        if (npcMovement != null) { npcMovement.PauseMovement(true); }
    }

    public virtual void StopDialogue()
    {
        //TODO: stop that dialogue
        npcMovement.PauseMovement(false);
    }

    public virtual void PlayInteractionActions()
    {
        OnInteractionActions.Invoke();
    }

    private void StartDialogueTimer()
    {
        //TODO: Start timer
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null)
        {
            Bigfoot bigfoot = other.gameObject.GetComponent<Bigfoot>();
            if (bigfoot.DisguiseState == BigfootDisguiseState.Naked) 
            {
                looked = true;
            }
            else
            {
                if (CanBeInteractedWith)
                {
                    if (!looked) { this.ShowChatIcon(true); }
                    //this.ShowChatIcon(true);

                    if (pInput.InteractionInputPushed)
                    {
                        interacted = true;
                        iconShownAmount++;
                        this.ShowChatIcon(false);
                        this.PlayDialogue();
                        this.PlayInteractionActions();
                        looked = true;
                    }
                }
            }

            if (looked) { this.LookAtPlayer(); }
        }
    }


    protected virtual void LookAtPlayer()
    {
        var lookPos = pInput.gameObject.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3);
    }


    protected virtual void OnTriggerExit(Collider other)
    {
        if(interacted)
        {
            this.StopDialogue();
            interacted = false;
        }

        this.ShowChatIcon(false);
        looked = false;
    }

}
