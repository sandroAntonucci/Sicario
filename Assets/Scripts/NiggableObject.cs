using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NiggableObject", order = 1)]
public class NiggableObject : ScriptableObject
{
    public List<AnimationClip> idle;
    public AnimationClip walking;
    public AnimationClip shooting;
    public AnimationClip attacking;
    public AnimationClip dance;
}