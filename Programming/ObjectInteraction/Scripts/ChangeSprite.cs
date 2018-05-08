using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour {

    private Sprite kbSprite;
    private Sprite gpSprite;
    private Image image;

    void Start ()
    {
        kbSprite = Resources.Load<Sprite>("KBSprite");
        gpSprite = Resources.Load<Sprite>("GPSprite");
        image = this.GetComponent<Image>();
    }
    
    void Update ()
    {
        image.sprite = InputManager.ControllerType == ControlType.Keyboard ? kbSprite : gpSprite;
    }
}
