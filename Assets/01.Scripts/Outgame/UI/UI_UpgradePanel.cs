using System.Collections.Generic;
using UnityEngine;

public class UI_UpgradePanel : MonoBehaviour
{
    public List<UI_UpgradeItem> Items;


    private void Start()
    {
        CurrencyManager.OnDataChanged += Refresh;
        UpgradeManager.OnDataChanged += Refresh;
    }


    private void Refresh()
    {
        var upgrades = UpgradeManager.Instance.GetAll();

        if (upgrades == null || upgrades.Count == 0)
            return;

        int count = Mathf.Min(Items.Count, upgrades.Count);
        for (int i = 0; i < count; ++i)
        {
            Items[i].Refresh(upgrades[i]);
        }
    }
}
