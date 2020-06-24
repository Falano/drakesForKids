using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "Scriptables/EggTypes")]
public class EggType : ScriptableObject
{
    public OperationTypes type; // drake shape, aka operationType
    public List<Sprite> EggSprites; // the 3 images for each layer of the egg image
    public List<Sprite> HatchedSprites; // the 3 images for each layer of the egg image
    public List<Difficulty> difficulties; // nb colors, aka operationRange
    // egg growth difficulty, aka operationComplexity,
    // is procedurally generated, so not stocked here
}

[System.Serializable]
public struct Difficulty
{
    public int difficultyLevel;
    public int minNumber;
    public int maxNumber;
}

[System.Serializable]
public class Palette
{
    public List<Color> cols = new List<Color>();

    public Palette(List<Color> colors)
    {
        cols = colors;
    }
}