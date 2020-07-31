using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorGrabber
{
    // careful: hasn't been updated from when colormaps were
    // wide as one palette length and high as the number of different palettes
    // TODO: update it to work with the new grey-bordered colormap format
    public static Color[,] GrabColorsFromFile(Texture2D map)
    {
        Color[,] Colors = new Color[map.width, map.height];
        Color[] ColorsUnsorted = map.GetPixels();
        int currentColor = 0;
        for (int h = map.height -1; h >= 0; h--)
        {
            for(int w = 0; w < map.width; w++)
            {
                Colors[w, h] = ColorsUnsorted[currentColor];
                currentColor++;
            }
        }
        return Colors;
    }

    public static Palette[] GrabPalettesFromFile(Texture2D map, int paletteLength)
    {
        //TODO: add some verification that the map width is (divisible by the (palette length + 1)) - 1
        Palette[] Palette = new Palette[((map.height+1)/2) * ((map.width+1) / (paletteLength + 1))];
        Color[] ColorsUnsorted = map.GetPixels();
        List<Color> PaletteColors = new List<Color>();
        int currentPalette = 0;
        for (int h = map.height - 1; h >= 0; h -=2)
        {
            for (int w = 0; w < map.width; w++)
            {
                if(w % (paletteLength + 1) == 0)
                {
                    // TODO: find a way to initialize this at each new palette (begining)
                    PaletteColors = new List<Color>();
                }
                PaletteColors.Add(ColorsUnsorted[w + h * map.width]);
                // TODO: loop on palette length somehow; check that it works
                if((w + 2) % (paletteLength + 1) == 0)
                {
                    Palette[currentPalette] = new Palette(PaletteColors);
                    currentPalette++;
                    // we skip the grey palette borders
                    w++;
                }
            }
        }
        return Palette;
    }
}
