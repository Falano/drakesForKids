using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindow : WindowManager, IWindowManager
{
    List<EggGraphics> _activeEggsInPool = new List<EggGraphics>();
    List<EggGraphics> _inactiveEggsInPool = new List<EggGraphics>();
    [SerializeField] GameObject eggPrefab;
    [SerializeField] Transform eggParent;

    public void SpawnEgg(Egg egg)
    {
        EggGraphics graphics;
        if(_inactiveEggsInPool.Count > 0)
        {
            graphics = _inactiveEggsInPool[0];
            _inactiveEggsInPool.Remove(graphics);
            graphics.gameObject.SetActive(true);
            graphics.transform.SetAsLastSibling();
        }
        else
        {
            GameObject newEgg = GameObject.Instantiate(eggPrefab, eggParent);
            graphics = newEgg.GetComponent<EggGraphics>();
        }
        //Debug.Log("images length: " + graphics.Images.Length);
        _activeEggsInPool.Add(graphics);
        graphics.Initialize();
        graphics.ReinitializeSpritesToEgg(egg);
        egg.Graphics = graphics;
    }

    public void DespawnEgg(Egg egg)
    {
        // it could use the EggGraphics script directly 
        // and bypass the need for the Egg to know their EggGraphics
        // but I like the symmetry
        // that Spawn and Despawn use the same type of parameter
        egg.Graphics.gameObject.SetActive(false);
        _activeEggsInPool.Remove(egg.Graphics);
        _inactiveEggsInPool.Add(egg.Graphics);
    }
}
