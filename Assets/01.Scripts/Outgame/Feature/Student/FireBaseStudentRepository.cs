#if !UNITY_WEBGL || UNITY_EDITOR
using UnityEngine;

public class FireBaseStudentRepository : IStudentRepository
{
    public void Save(string name, StudentSaveData saveData)
    {
        //�˾Ƽ� ����

    }
    public StudentSaveData Load(string name)
    {
        //�˾Ƽ� ����
        return null;
    }
}
#endif
