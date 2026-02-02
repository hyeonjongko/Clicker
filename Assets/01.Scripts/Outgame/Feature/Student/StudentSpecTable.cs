using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StudentSpecTable", menuName = "Scriptable Objects/StudentSpecTable")]
public class StudentSpecTable : ScriptableObject
{
    public List<StudentSpecData> SpecDatas;
}