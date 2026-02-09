using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance {  get; private set; }
    public static event Action OnDataChanged;

    [SerializeField] private UpgradeSpecTableSO _specTable;
    private IUpgradeRepository _repository;

    private Dictionary<EUpgradeType, Upgrade> _upgrade = new();
    private void Awake()
    {
        Instance = this;

        //_repository = new JsonUpgradeRepository(AccountManager.Instance.Email);
        _repository = new FirebaseUpgradeRepository();
        InitializeAsync().Forget();
    }

    private async UniTaskVoid InitializeAsync()
    {
        var saveData = await _repository.Load();

        foreach (var specData in _specTable.Datas)
        {
            if (_upgrade.ContainsKey(specData.Type))
            {
                throw new Exception($"There is already an upgrade with type {specData.Type}");
            }

            int savedLevel = 0;
            int index = (int)specData.Type;
            if (saveData.Levels != null && index < saveData.Levels.Length)
            {
                Debug.Log(saveData.Levels[index]);
                savedLevel = saveData.Levels[index];
            }
            else
            {
                Debug.Log("없");
            }

            Debug.Log(specData.Name);
            _upgrade.Add(specData.Type, new Upgrade(specData, savedLevel));
        }

        OnDataChanged?.Invoke();
    }

    public Upgrade Get(EUpgradeType type) => _upgrade[type] ?? null;
    public List<Upgrade> GetAll() => _upgrade.Values.ToList();

    public bool CanLevelUp(EUpgradeType type)
    {
        if (!_upgrade.TryGetValue(type, out Upgrade upgrade))
        {
            return false;
        }

        if (!upgrade.CanLevelUp())
        {
            return false;
        }

        return CurrencyManager.Instance.CanAfford(ECurrencyType.Gold, upgrade.Cost);
    }
    public bool TryLevelUp(EUpgradeType type)
    {
        if (!_upgrade.TryGetValue(type, out Upgrade upgrade))
        {
            return false;
        }

        if (!CurrencyManager.Instance.TrySpend(ECurrencyType.Gold, upgrade.Cost))
        {
            return false;
        }

        if (!upgrade.TryLevelUp())
        {
            return false;
        }

        Save();

        OnDataChanged?.Invoke();

        return true;
    }
    private void Save()
    {
        var data = new UpgradeSaveData
        {
            Levels = new int[(int)EUpgradeType.Count]
        };

        foreach (var pair in _upgrade)
        {
            data.Levels[(int)pair.Key] = pair.Value.Level;
        }

        _repository.Save(data).Forget();
    }

}
