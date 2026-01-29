using System.Collections.Generic;
using UnityEngine;

public class UI_UpgradePanel : MonoBehaviour
{
    public List<UI_UpgradeItem> Items;


    private void Start()
    {
        Refresh();

        CurrencyManager.OnDataChanged += Refresh;
        UpgradeManager.OnDataChanged += Refresh;
    }


    private void Refresh()
    {
        var upgrades = UpgradeManager.Instance.GetAll();

        for (int i = 0; i < Items.Count; ++i)
        {
            Items[i].Refresh(upgrades[i]);
        }
    }
}
