using System;
using UnityEngine;

namespace TestLab
{
    [CreateAssetMenu(fileName = "Achievement", menuName = "Achievement", order = 0)]
    public abstract class Achievement : ScriptableObject
    {
        [SerializeField] protected string title;
        [SerializeField] protected string description;
        
        public Action<string, string> OnTrigger;
        
        public abstract void Register();

        protected abstract void Check();

        protected abstract void Trigger();
    }
}