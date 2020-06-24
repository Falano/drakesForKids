using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EggsManager : MonoBehaviour
{
    public static EggsManager instance;

    [SerializeField] Texture2D colorsMap;
    public Color[] Colors;

    [SerializeField] Texture2D palettesMap;
    public Palette[] Palettes;

    // TODO: add some verification that this isn't null
    public int maxColorsNumber;
    [SerializeField] EggType[] _eggTypes;
    public EggType[] EggTypes { get { return _eggTypes; } }

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        GetColors();
    }

    void GetColors()
    {
        Colors = colorsMap.GetPixels();
        Palettes = ColorGrabber.GrabPalettesFromFile(palettesMap, maxColorsNumber);
    }
}