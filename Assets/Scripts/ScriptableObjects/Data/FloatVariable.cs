using UnityEngine;

namespace ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "ScriptableObjects/Dat/Variables/Float", order = 0)]
    public class FloatVariable : ScriptableObject
    {
        public float Value;
    }
}