using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private Image[] _images;
    public Image[] Images
    {
        get
        {
            return _images;
        }
    }
    public int NumberOfColors { get; private set; }
    public Color[] CurrentColors { get; private set; }

    void Awake()
    {
        if(CurrentColors == null)
        {
            CurrentColors = new Color[Images.Length];
        }
    }

    public Color[] ChooseRandomColors(Color[] colors, int numberOfColors = -1)
    {
        Color[] result = new Color[Images.Length];

        if (numberOfColors == -1)
        {
            numberOfColors = Random.Range(1, 4);
        }

        //// clear out current color so we can make sure 
        //// no two of the new colors are the same
        //for (int i = 0; i < CurrentColors.Length; i++)
        //{
        //    CurrentColors[i] = default;
        //}

        //NumberOfColors = numberOfColors;

        // choose random colors, all different
        Color col;
        for (int i = 0; i < result.Length; i++)
        {
            if (i < numberOfColors)
            {
                do
                {
                    col = colors[Random.Range(0, colors.Length)];
                }
                while (result.Contains(col));
                result[i] = col;
            }
            else
            {
                result[i] = default;
            }
        }
        return result;
    }

    public Color[] ChooseRandomColors(Palette[] palettes, int numberOfColors = -1)
    {
        Color[] result = new Color[Images.Length];
        if (numberOfColors == -1)
        {
            numberOfColors = Random.Range(1, 4);
        }
        
        // choose random colors, all different
        Palette palette = palettes[Random.Range(0, palettes.Length)];
        for (int i = 0; i < result.Length; i++)
        {
            if (i < numberOfColors)
            {
                result[i] = palette.cols[i];
            }
            else
            {
                result[i] = default;
            }
        }
        return result;
    }

    public void ChangeColors(Color[] colors)
    {
        NumberOfColors = colors.Length;
        CurrentColors = colors;
        for(int i = 0; i < Images.Length; i++)
        {
            if(colors[i] == default)
            {
                Images[i].gameObject.SetActive(false);
            }
            else
            {
                Images[i].color = colors[i];
            }
        }
    }
}