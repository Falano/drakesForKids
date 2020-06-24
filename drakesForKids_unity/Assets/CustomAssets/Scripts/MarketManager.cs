using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketManager : WindowManager, IWindowManager
{
    private List<EggGraphics> _eggs = new List<EggGraphics>();

    [SerializeField] GameObject _eggPrefab;
    [SerializeField] Transform _eggParent;
    [SerializeField] int _eggsNumber;

    Vector3 _position = new Vector3(200, -200, 0);
    int _stepX = 175;
    int _stepY = -150;

    // TODO: last played date

    private new void Start()
    {
        // TODO: only refresh eggs if last played date is different from today or a dragon hatched
        Initialize();
        base.Start();
    }

    public void Initialize()
    {
        foreach(EggType type in EggsManager.instance.EggTypes)
        {
            foreach(Difficulty difficulty in type.difficulties)
            {
                for(int i = 0; i < _eggsNumber; i++)
                {
                    SpawnEgg(type, difficulty.difficultyLevel+1, _position);
                    _position.x += _stepX;
                    _position.y += _stepY;
                    _stepY *= -1;
                }
            }
        }
    }

    public void SpawnEgg(EggType type, int level, Vector3 position)
    {
        EggGraphics egg = Instantiate(_eggPrefab, _eggParent).GetComponent<EggGraphics>();
        egg.transform.localPosition += position;
        egg.ChangeSprites(type);
        egg.ChangeColorsRandom(level);
        _eggs.Add(egg);

    }

    public void RefreshEggs()
    {
        if(_eggs.Count == 0 || _eggs == null)
        {
            Debug.LogError("No egg in the Market List; weird!/n Maybe you forgot " +
                "to give them ChangeColor scripts? Maybe your Market Manager " +
                "is in the wrong place? Something is wrong for sure.");
            return;
        }

        foreach(EggGraphics egg in _eggs)
        {
            egg.gameObject.SetActive(true);
            // TODO: implement Color List in Eggs Manager
            egg.ChangeColorsRandom(egg.numberOfColors);
        }
    }
}
