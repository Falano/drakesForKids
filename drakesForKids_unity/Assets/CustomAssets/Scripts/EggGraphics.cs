using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EggGraphics : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    WindowsManager man = WindowsManager.instance;

    ChangeColor _changeCol;
    private Image[] _images;
    [SerializeField] EggType _type;
    public EggType Type { get { return _type; } }
    public int numberOfColors { get { return _changeCol.NumberOfColors; } }


    // can be null if in the market
    private Egg _egg;


    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _changeCol = GetComponent<ChangeColor>();
        _images = _changeCol.Images;
        if (_images.Length == 0 || _images == null)
        {
            Debug.LogError("No images at all for the egg " + gameObject.name + "? That's just wrong.");
        }
    }

    public void ChangeSprites(EggType type, bool hasHatched = false)
    {
        List<Sprite> sprites = hasHatched ? 
            type.HatchedSprites : 
            type.EggSprites;

        _type = type;
        // update sprites according to operation type
        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].sprite = sprites[i];
        }
    }

    public void ChangeColorsRandom(int numberOfColors)
    {
        // still not sure whether I want my random to pick a palette
        // or individual colors
        Color[] colors = _changeCol.ChooseRandomColors(EggsManager.instance.Palettes, numberOfColors);
        _changeCol.ChangeColors(colors);
    }

    public void ReinitializeSpritesToEgg(Egg egg)
    {
        _egg = egg;
        ChangeSprites(egg.Type, egg.hasHatched);
        _changeCol.ChangeColors(egg.Colors);
    }

    #region pointerEvents
    public void OnPointerClick(PointerEventData eventData)
    {
        if (man.ActiveWindow == man.MarketManager)
        {
            // add egg to inventory
            Egg newEgg = new Egg(numberOfColors: _changeCol.NumberOfColors, colors: _changeCol.CurrentColors, type: Type);
            PlayerManager.Eggs.Add(newEgg);
            // add egg image to inventory window
            WindowsManager.instance.EggsInventory.SpawnEgg(newEgg);
            // remove egg from buyables
            gameObject.SetActive(false);
        }
        else if (man.ActiveWindow == man.EggsInventory)
        {
            if(_egg == null)
            {
                Debug.LogError("Missing egg! How come the eggs graphics don't know the egg object in the inventory egg window? Wrong, bad, shouldn't happen!");
            }
            man.SetCurrentEgg(_egg);
            Goer.GoToWindow(WindowsManager.instance.HatchSceneManager);
        }
        else if (man.ActiveWindow == man.DragonsInventory)
        {
                // TODO: dragony things
        }
        else
        {
            Debug.Log("egg clicked, nothing happens");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // start vibrating? or rotating with sin()
        _images[0].color = Color.magenta; //tmp
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerOut();
    }

    public void PointerOut()
    {
        _images[0].color = _changeCol.CurrentColors[0]; //tmp
        // stop rotating. Stop all coroutines?
        // or just stop this one. Seriously, overkill much?
    }

    public void OnDisable()
    {
        PointerOut();
    }
    #endregion pointerEvents
}
