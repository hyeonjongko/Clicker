using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance {  get; private set; }
    public static event Action OnDataChanged;

    [SerializeField] private UpgradeSpecTableSO _specTable;

    private Dictionary<EUpgradeType, Upgrade> _upgrade = new();
    private void Awake()
    {
        Instance = this;

        foreach(var specData in _specTable.Datas)
        {
            if(_upgrade.ContainsKey(specData.Type))
            {
                throw new Exception($"There is already an upgrade with type {specData.Type}");
            }

            _upgrade.Add(specData.Type, new Upgrade(specData));
        }

        OnDataChanged?.Invoke();
    }

    public Upgrade Get(EUpgradeType type) => _upgrade[type];
    public List<Upgrade> GetAll() => _upgrade.Values.ToList();

    public bool CanLevelUp(EUpgradeType type)
    {
        if(!_upgrade.TryGetValue(type, out Upgrade upgrade))
        {
            return false;
        }

        return CurrencyManager.Instance.TrySpend(ECurrencyType.Gold, upgrade.Cost);
    }
    public bool TryLevelUp(EUpgradeType type)
    {
        if (!_upgrade.TryGetValue(type, out Upgrade upgrade))
        {
            Debug.Log(1);

            return false;
        }

        if (!CurrencyManager.Instance.TrySpend(ECurrencyType.Gold, upgrade.Cost))
        {
            Debug.Log(2);

            return false;
        }

        if (!upgrade.LevelUp())
        {
            Debug.Log(3);
            return false;
        }

        OnDataChanged?.Invoke();

        return true;
    }

}
