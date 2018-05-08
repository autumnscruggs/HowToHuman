using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Characters { Bigfoot, Narrator, Dad, BusDriver, PhoneGirl }

[RequireComponent(typeof(AudioSource))]
public class SubtitleManager : MonoBehaviour
{
    [SerializeField] private Image subtitleBox;
    private Text character;
    private Text dialogue;

    [SerializeField] private bool startTimer = false;
    [SerializeField] private float subtitleTimer = 0;
    private int currentID = 0;
    private float currentSubtitleDelay = 0;
    private bool closeSubtitleChain = false;
    private int nextSubtitleID = 0;

    //[SerializeField] private float fadeDuration = 0.5f;
    [HideInInspector] public List<Subtitle> Subtitles = new List<Subtitle>();

    private AudioSource source;

    private static SubtitleManager singleton; // Singleton instance                                    
    public static SubtitleManager Instance // Return instance
    {
        get
        {
            if (singleton == null)
            {
                Debug.LogError("[SubtitleManager]: Instance does not exist!");
                return null;
            }

            return singleton;
        }
    }
    private void InitializeSingleton()
    {
        // Found a duplicate instance of this class, destroy it!
        if (singleton != null && singleton != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            singleton = this;
        }
    }

    private void Awake()
    {
        InitializeSingleton();

        subtitleBox.gameObject.SetActive(true);
        character = subtitleBox.transform.GetChild(0).gameObject.GetComponent<Text>();
        dialogue = subtitleBox.transform.GetChild(1).gameObject.GetComponent<Text>();
        ShowSubtitleBox(false, 0.01f);

        source = this.GetComponent<AudioSource>();
    }

    private void Start()
    {
        //PlaySubtitle(0);
    }

    private void OnEnable()
    {
        Pauser.OnPause += OnPause;
        Pauser.OnUnPause += OnUnPause;
    }

    private void OnDisable()
    {
        Pauser.OnPause -= OnPause;
        Pauser.OnUnPause -= OnUnPause;
    }

    private void OnPause(object sender, System.EventArgs e)
    {
        source.Pause();
    }
    private void OnUnPause(object sender, System.EventArgs e)
    {
        source.UnPause();
    }

    private void Update()
    {
        //if (currentID == 53) { Debug.Log("Start Timer - " + startTimer); }

        if (startTimer)
        {
            subtitleTimer += Time.deltaTime;
            if (subtitleTimer >= currentSubtitleDelay)
            {
                subtitleTimer = 0;
                startTimer = false;
                if (closeSubtitleChain) { ShowSubtitleBox(false); }
                else { PlaySubtitle(nextSubtitleID); }
            }
        }
    }

    public void TurnOffSubtitleImmediately()
    {
        ShowSubtitleBox(false, 0.001f);
        source.Stop();
        subtitleTimer = 0;
        startTimer = false;
    }

    private void ShowSubtitleBox(bool show, float duration = -1)
    {
        //if(duration == -1) { duration = fadeDuration; }
        //if(show) { subtitleBox.FadeIn(duration); character.FadeIn(duration); dialogue.FadeIn(duration); }
        //else { subtitleBox.FadeOut(duration); character.FadeOut(duration); dialogue.FadeOut(duration); }

        subtitleBox.gameObject.SetActive(show);
    }

    public void PlaySubtitle(int id)
    {
        TurnOffSubtitleImmediately();
        Subtitle subtitle = Subtitles.Find(x => x.ID == id);
        currentID = id;
        source.PlayOneShot(subtitle.AudioClip);
        currentSubtitleDelay = subtitle.UpTime;
        closeSubtitleChain = subtitle.CloseAfter;
        nextSubtitleID = subtitle.NextID;
        //if (currentID == 53) { Debug.Log("Subtitle - " + subtitle.ID + " // CloseChain - " + closeSubtitleChain + " // " + "NextID - " + nextSubtitleID); }
        startTimer = true;

        if (PauseData.ShowSubtitles)
        {
            ShowSubtitleBox(true);
            character.text = CharacterName(subtitle.Character);
            dialogue.text = subtitle.DialogueText;
            //source.clip = subtitle.AudioClip;
            // source.PlayDelayed(fadeDuration);
        }
    }

    private string CharacterName(Characters character)
    {
        switch (character)
        {
            case Characters.Bigfoot:
                return "Bigfoot";
                break;
            case Characters.Narrator:
                return "Narrator";
                break;
            case Characters.Dad:
                return "Dad";
                break;
            case Characters.BusDriver:
                return "Horace";
                break;
            case Characters.PhoneGirl:
                return "Val";
                break;
            default:
                break;
        }

        return "";
    }
}

[System.Serializable]
public class Subtitle
{
    public int ID;
    public AudioClip AudioClip;
    public Characters Character;
    public string DialogueText;
    public float UpTime;
    public bool CloseAfter;
    public int NextID;

    //public Subtitle()
    //{
    //    ID = SubtitleManager.SubtitleID++;
    //}
}
