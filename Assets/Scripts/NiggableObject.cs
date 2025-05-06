using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NiggableObject", order = 1)]
public class NiggableObject : ScriptableObject
{
    public Animator animator;
}