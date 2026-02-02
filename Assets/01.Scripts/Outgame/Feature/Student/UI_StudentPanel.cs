using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StudentPanel : MonoBehaviour
{
    [SerializeField] List<UI_Student_Item> _studentUIItem;

    private void Start()
    {
        StudentManager.Instance.OnDataChange += Refresh;

        Refresh();
    }

    private void Refresh()
    {
        List<IReadonlyStudent> students = StudentManager.Instance.GetAll();

        for (int i = 0; i < students.Count; ++i)
        {
            _studentUIItem[i].Refresh(students[i]);
        }
    }
}
