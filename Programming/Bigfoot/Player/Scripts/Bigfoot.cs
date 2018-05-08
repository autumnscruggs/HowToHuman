using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BigfootDisguiseState { Naked, Disguised }

[RequireComponent(typeof(PlayerMovement))]
public class Bigfoot : MonoBehaviour
{
    [SerializeField] private bool detectable = true;
    public bool IsDetectable
    {
        get { return detectable; }
        private set { detectable = value; }
    }

    //States
    [SerializeField] private BigfootDisguiseState disguiseState; //serialized for debugging
    public BigfootDisguiseState DisguiseState
    {
        get { return disguiseState; }
        set
        {
            if (disguiseState != value)
            {
                disguiseState = value;
            }
        }
    }

    public PlayerMovement Movement { get; private set; }
    private SkinnedMeshRenderer meshRenderer;
    public Texture OriginalBaseMesh;

    public List<DisguiseModelObject> disguises = new List<DisguiseModelObject>();
    #region Struct Class
    [System.Serializable]
    public class DisguiseModelObject
    {
        public DisguiseTypes Type;
        public GameObject Object;
        public Texture BaseMesh;
    }
    #endregion

    #region Singleton
    private static Bigfoot singleton; // Singleton instance                                    
    public static Bigfoot Instance // Return instance
    {
        get
        {
            if (singleton == null)
            {
                Debug.LogError("[Bigfoot]: Instance does not exist!");
                return null;
            }

            return singleton;
        }
    }
    #endregion

    private void Awake()
    {
        Movement = this.GetComponent<PlayerMovement>();
        meshRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();

        #region Singleton
        // Found a duplicate instance of this class, destroy it!
        if (singleton != null && singleton != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            // Create singleton instance
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
            #endregion

            //ChangeIntoDisguise(DisguiseTypes.None);
        }

    public void DisguiseStateChanged(DisguiseTypes disguiseType) //called in disguise manager
    {
        if (this.DisguiseState == BigfootDisguiseState.Disguised) { this.Movement.CanRun = false; }
        else { this.Movement.CanRun = true; }

        ChangeIntoDisguise(disguiseType);

    }

    public void Hide()
    {
        IsDetectable = false;
    }

    public void Unhide()
    {
        IsDetectable = false;
    }

    private void ChangeIntoDisguise(DisguiseTypes disguiseType)
    {
        disguises.ForEach(x => x.Object.SetActive(false));

        if (disguiseType != DisguiseTypes.None)
        {
            DisguiseModelObject dmo = disguises.Find(x => x.Type == disguiseType);
            if(dmo.BaseMesh != null) { meshRenderer.material.SetTexture("_MainTex", dmo.BaseMesh); }
            else { meshRenderer.material.SetTexture("_MainTex", OriginalBaseMesh); }
            dmo.Object.SetActive(true);
        }
        else
        {
            meshRenderer.material.SetTexture("_MainTex", OriginalBaseMesh);
        }
    }
}
