using UnityEngine;

public class Egg
{
    public Color[] Colors { get; }
    public OperationTypes Operation { get; }
    public EggType Type { get; }
    public int NumberOfColors { get; }
    public bool hasHatched {get; private set;}
    // TODO: not sure whether this is useful; check
    public EggGraphics Graphics;


    public Egg(int numberOfColors, Color[] colors, EggType type)
    {
        hasHatched = false;
        Colors = colors;
        NumberOfColors = numberOfColors;
        Type = type;
    }

    public void Hatch()
    {
        this.hasHatched = true;
        if(Graphics == null)
        {
            Debug.LogError("Hatching Egg has no Graphics component! (it should)");
        }
        else
        {
            Debug.Log("Hatching egg");
            Graphics.gameObject.SetActive(false);
            WindowsManager.instance.DragonsInventory.SpawnEgg(this);
        }
    }
}
