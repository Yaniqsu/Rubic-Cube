using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Skybox Reference", menuName = "Scriptable Objects /Skybox Reference")]
public class SkyboxReference : ScriptableObject
{
    [Serializable]
    public struct SkyboxInfo
    {
        public string name;
        public Material material;
    }

    [SerializeField] private SkyboxInfo[] references;

    public SkyboxInfo[] Reference => references;
}
