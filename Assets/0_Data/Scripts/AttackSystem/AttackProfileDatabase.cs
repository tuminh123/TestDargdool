using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackProfileDB", menuName = "Attack/AttackProfileDatabase", order = 1)]
public class AttackProfileDatabase : ScriptableObject
{
    public List<AttackProfile> profiles = new List<AttackProfile>();
}
