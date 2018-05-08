using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSpriteManual : MonoBehaviour {

    public Sprite kbSprite;
    public Sprite gpSprite;
    private Image image;

    void Start ()
    {
        image = this.GetComponent<Image>();
    }
    
    void Update ()
    {
        image.sprite = InputManager.ControllerType == ControlType.Keyboard ? kbSprite : gpSprite;
    }
}
