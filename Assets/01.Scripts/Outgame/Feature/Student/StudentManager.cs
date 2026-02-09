using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StudentManager : MonoBehaviour
{
    public static StudentManager Instance { get; private set; }

    private List<Student> _students = new();

    [SerializeField] private StudentSpecTable _SpecTable;

    public event Action OnDataChange;

    private IStudentRepository _repository;
    private void Awake()
    {
        Instance = this;

#if UNITY_WEBGL && !UNITY_EDITOR
        _repository = new PlayerPrefsStudentRepository();
#else
        _repository = new FireBaseStudentRepository();
#endif

        foreach (StudentSpecData data in _SpecTable.SpecDatas)
        {
            //불러오기
            StudentSaveData saveData = _repository.Load(data.Name);

            bool isAttendance = false;
            if (saveData != null)
            {
                isAttendance = saveData.Attendance;
            }

            Student student = new Student(data, isAttendance);


            _students.Add(student);
        }
    }

    public List<IReadonlyStudent> GetAll()
    {
        return _students.ToList<IReadonlyStudent>();
    }

    public bool TryAttendance(string studentName)
    {
        Student student = _students.Find(student => student.Name == studentName);
        if (student == null)
        {
            return false;
        }

        if(student.IsAttendance)
        {
            return false;
        }

        student.CheckAttendance(true);


        OnDataChange?.Invoke();

        StudentSaveData savedata = new StudentSaveData();
        savedata.Attendance = true;

        _repository.Save(studentName, savedata);

        return true;
    }


}