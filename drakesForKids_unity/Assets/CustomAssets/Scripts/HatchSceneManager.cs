using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchSceneManager : WindowManager, IWindowManager
{
    [SerializeField] GameObject EmberPrefab;
    [SerializeField] Transform EmberParent;
    [SerializeField] GameObject IcePrefab;
    [SerializeField] Transform IceParent;

    private int lastHatchingMove;
    private float _additionMultiplier = .5f;
    private int _additionSteps { get
        {
            return Mathf.CeilToInt((EggLevel + 1) * _additionMultiplier);
        } }

    // minimum temperature is always 0
    public int MaxTemperature
    {
        get
        {
            if (CurrentEgg == null)
            {
                Debug.LogWarning("Current Egg is null");
                return 0;
            }
            else
            {
                return (CurrentEgg.Type.difficulties[CurrentEgg.NumberOfColors-1].maxNumber * 10);
            }
        }
    }
    public int CurrentTemperature;// { get; private set; }
    public int GoalTemperature;// { get; private set; }
    public bool IsRightTemperature { get { return CurrentTemperature == GoalTemperature; } }
    public int[] EmbersAndIce { get; private set; }
    public TemperatureModifier[] SpawnedEmbersAndIce { get; private set; }
    public int NumberOfEmbers;// { get; private set; }

    [SerializeField] int MaxEggLevel;
    public Egg CurrentEgg;
    int _eggLevel;

    public int EggLevel
    {
        get
        {
            return _eggLevel;
        }

        private set
        {
            if (_eggLevel >= MaxEggLevel)
            {
                CurrentEgg.Hatch();
                Goer.GoToWindow(WindowsManager.instance.DragonsInventory);
                WindowsManager.instance.MarketManager.RefreshEggs();
                CurrentEgg = null;
            }
            _eggLevel = value;
            Debug.Log("current egg level: " + _eggLevel);
        }
    }

    // TODO: set the scene
    // TODO: set the coals and icecubes

    private new void Start()
    {
        EmbersAndIce = new int[NumberOfEmbers*2];
        SpawnedEmbersAndIce = new TemperatureModifier[NumberOfEmbers*2];
        base.Start();
    }

    public new void Activate()
    {
        base.Activate();
        if(CurrentEgg != null)
        {
            SetEmbersAndIce(CurrentEgg.NumberOfColors, CurrentEgg.Operation);
            CurrentTemperature = Mathf.RoundToInt(MaxTemperature * .5f);
            GoalTemperature = NewGoalTemperature(EggLevel, operation: CurrentEgg.Operation);
        }
    }

    void SetEmbersAndIce(int eggColorDifficulty, OperationTypes operation)
    {
        if(CurrentEgg == null)
        {
            Debug.LogError("No Current Egg! How can this be?");
        }

        Debug.Log("setting embers ice");
        // populate the Embers and Ice array
        int min = CurrentEgg.Type.difficulties[EggLevel].minNumber;
        int max = CurrentEgg.Type.difficulties[EggLevel].maxNumber;
        int interval = max - min;
        int step = interval/NumberOfEmbers;

        Random.InitState(System.DateTime.Now.Millisecond);
        for (int i = 0; i < NumberOfEmbers; i++)
        {
            EmbersAndIce[i] = Random.Range(min + (step * i), min + (step * (i + 1))) * -1;
        }

        Random.InitState(Time.frameCount);
        for (int i = 0; i < NumberOfEmbers; i++)
        {
            EmbersAndIce[i + NumberOfEmbers] = Random.Range(min + (step * i), min + (step * (i + 1)));
        }

        // spawn stuff
        for (int i = 0; i < EmbersAndIce.Length; i++)
        {
            Debug.Log("spawning ember/ice: " + EmbersAndIce[i] + " between (-)" + min + " and (-)" + max);
            SpawnEmberAndIce(index:i, value:EmbersAndIce[i]);
        }
    }

    void SpawnEmberAndIce(int index, int value)
    {
        GameObject obj;
        if (SpawnedEmbersAndIce[index] == null || SpawnedEmbersAndIce[index] == default)
        {
            if(value < 0)
            {
                obj = Instantiate(IcePrefab, IceParent);
            }
            else
            {
                obj = Instantiate(EmberPrefab, EmberParent);
            }
            SpawnedEmbersAndIce[index] = obj.GetComponent<TemperatureModifier>();
            SpawnedEmbersAndIce[index].value = value;
        }
        else
        {
            SpawnedEmbersAndIce[index].value = value;
        }
    }

    public void ChangeTemperature(int temp)
    {
        CurrentTemperature += temp;
        lastHatchingMove++;
        CheckTemperature();
        if(lastHatchingMove > _additionSteps*2)
        {
            ChangeTemperatureFailed();
        }
    }

    void ChangeTemperatureFailed()
    {
        // TODO: this
        Debug.Log("You took too many steps to change the temperature! Fail!");
    }

    void CheckTemperature()
    {
        if (IsRightTemperature)
        {
            EggLevel +=1;
            // TODO: do some nice level up animation
            GoalTemperature = NewGoalTemperature(growthLevel: EggLevel, operation: CurrentEgg.Operation);
            lastHatchingMove = 0;
        }
        // some anim or smth bc we changed the temperature
    }

    int NewGoalTemperature(int growthLevel, float multiplier = .5f, OperationTypes operation = OperationTypes.Addition)
    {
        int result;
        switch (operation)
        {
            case OperationTypes.Addition:
                result = NewGoalTemperatureSimple(operation);
                break;
            default:
                result = NewGoalTemperatureComplex(growthLevel, multiplier, operation);
                break;
        }
        return result;
    }

    /// <summary>
    /// Generates a new goal temperature for Addition.
    /// A new Goal Temperature is at most level*multiplier step away
    /// from the Current Temperature
    /// </summary>
    /// <param name="growthLevel">difficulty based on the egg growth level</param>
    /// <param name="multiplier">difficulty multiplier</param>
    /// <returns></returns>
    int NewGoalTemperatureSimple(OperationTypes operation = OperationTypes.Addition)
    {
        int tmpGoalTemperature = CurrentTemperature;
        int tmpChange;
        int tmpChangeIndex;

        // get random ember or ice to add to current temperature,
        // and do it a few times depending on the level
        for(int x = 0; x < _additionSteps; x+=1)
        {
            do
            {
                tmpChangeIndex = Random.Range(0, EmbersAndIce.Length);
                tmpChange = EmbersAndIce[tmpChangeIndex];
                tmpGoalTemperature += tmpChange;
            }
            // we check that it's within bounds
            while (tmpGoalTemperature <= 0 || tmpGoalTemperature >= MaxTemperature || tmpGoalTemperature == GoalTemperature);
        }
        return tmpGoalTemperature;
    }


    /// <summary>
    /// Generates a new goal temperature for Multiplication or Division.
    /// A new Goal Temperature is at most x ember/ice 
    /// (of one type) away from the Current Temperature
    /// or a piece x times smaller (for division)
    /// </summary>
    /// <param name="growthLevel">difficulty based on the egg growth level</param>
    /// <param name="multiplier">difficulty multiplier</param>
    /// <returns></returns>
    int NewGoalTemperatureComplex(int growthLevel, float multiplier = .5f, OperationTypes operation = OperationTypes.Multiplication)
    {
        int tmpGoalTemperature = CurrentTemperature;
        int tmpChange;
        int tmpChangeIndex;
        int tmpChangeMultiplier;
        int baseMultiplier = Mathf.CeilToInt(growthLevel * multiplier + 1);

        // get random ember or ice to add to current temperature,
        // and multiply it a few times depending on the level
        do
        {
            tmpChangeIndex = Random.Range(0, EmbersAndIce.Length);
            tmpChange = EmbersAndIce[tmpChangeIndex];
            switch (operation)
            {
                case OperationTypes.Multiplication:
                    tmpChangeMultiplier = Random.Range(
                        baseMultiplier,
                        baseMultiplier * 2 + Mathf.FloorToInt(growthLevel * multiplier));
                    break;
                case OperationTypes.Division:
                    // TODO: wait we need to be sure that we'll end up with whole numbers
                    // check it.
                    tmpChangeMultiplier = Random.Range(
                        baseMultiplier,
                        (int)((baseMultiplier ^ 2) + baseMultiplier * multiplier));
                    tmpChangeMultiplier = 1 / tmpChangeMultiplier;
                    break;
                default:
                    Debug.LogError(operation.ToString() + "is not a complex operation. Try division or multiplication");
                    tmpChangeMultiplier = 0;
                    break;
            }
            tmpGoalTemperature += tmpChange * tmpChangeMultiplier;
        }
        // we check that it's within bounds
        while (tmpGoalTemperature <= 0 && tmpGoalTemperature >= MaxTemperature);
        return tmpGoalTemperature;
    }
}