using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DisguiseTypes { None, Hunter }

public class DisguiseManager : MonoBehaviour
{
    public DisguiseTypes CurrentDisguiseType;

    private Bigfoot bigfoot;

    //SINGLETON
    #region Singleton
    private static DisguiseManager instance = null;
    public static DisguiseManager Instance
    {
        get
        {
            return instance;
        }
    }
    private void InitializeSingleton()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    private void Awake()
    {
        bigfoot = GameObject.FindObjectOfType<Bigfoot>();
        InitializeSingleton();
    }

    public void PickUpDisguise(DisguiseTypes disguiseTypes)
    {
        //bigfoot.ForceIdle();
        CurrentDisguiseType = disguiseTypes;
        bigfoot.DisguiseState = BigfootDisguiseState.Disguised;
        bigfoot.DisguiseStateChanged(CurrentDisguiseType);
    }

    public void LoseDisguise()
    {
        CurrentDisguiseType = DisguiseTypes.None;
        bigfoot.DisguiseState = BigfootDisguiseState.Naked;
        bigfoot.DisguiseStateChanged(CurrentDisguiseType);
    }
}
