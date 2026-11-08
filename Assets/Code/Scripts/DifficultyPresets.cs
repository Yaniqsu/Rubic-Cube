using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects /Difficulty Presets", fileName = "Difficulty Presets")]
public class DifficultyPresets : ScriptableObject
{
    [Serializable]
    private struct Difficulty
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int RandomizedTurns { get; private set; }
    }

    [SerializeField] private Difficulty[] presets;

    public int GetTurns(int index) => index >= presets.Length ? 0 : presets[index].RandomizedTurns;

    public string[] GetNames() => presets.Select(p => p.Name).ToArray();
}
