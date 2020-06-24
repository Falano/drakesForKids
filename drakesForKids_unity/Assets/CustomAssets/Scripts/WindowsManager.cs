using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsManager: MonoBehaviour
{
    public static WindowsManager instance { get; private set; }

    public InventoryWindow EggsInventory;
    public InventoryWindow DragonsInventory;
    public MarketManager MarketManager;
    public HatchSceneManager HatchSceneManager;
    public MenuWindow MenuWindow;

    public IWindowManager ActiveWindow;

    private Egg _currentEgg;
    public Egg CurrentEgg
    {
        get
        {
            return _currentEgg;
        }
        set
        {
            _currentEgg = value;
            HatchSceneManager.CurrentEgg = value;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        ActiveWindow = MenuWindow;
    }

    public void SetCurrentEgg(Egg egg)
    {
        CurrentEgg = egg;
    }
}