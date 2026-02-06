using Firebase.Firestore;
using System;

[Serializable]
[FirestoreData]
public class UpgradeSaveData
{
    // 레벨 배열 (EUpgradeType 순서대로 저장)
    [FirestoreProperty]
    public int[] Levels { get; set; }
    [FirestoreProperty]
    public string LastSaveTime { get; set; }

    /// <summary>기본값 (새 게임)</summary>
    public static UpgradeSaveData Default => new UpgradeSaveData
    {
        Levels = new int[(int)EUpgradeType.Count], // 모든 레벨 0
        LastSaveTime = DateTime.Now.ToString("o")
    };
}
