using System;
using System.Collections.Generic;
using MyStateMachine;
using UnityEngine;
using UnityEngine.Assertions;

namespace Framework
{
    public class ProcedureFsm
    {
        public ProcedureBase CurrentProcedure;
        public bool HasStart = false;
        private Dictionary<int, ProcedureBase> _stateDict = new();
        private Dictionary<string, object> _valueDictionary = new();
        private int _startHash=-1;
        public void Start()
        {
            if (_startHash == -1)
            {
                Debug.LogError("Hasn't set start state");
                return;
            }

            HasStart = true;
            CurrentProcedure = _stateDict[_startHash];
            CurrentProcedure.OnEnter();
        }

        public void OnLogic(float deltaTime)
        {
            if (!HasStart) return;
            CurrentProcedure.OnLogic(deltaTime);
        }

        public void ChangeState<T>()
        {
            CurrentProcedure.OnExit();
            CurrentProcedure = _stateDict[typeof(T).GetHashCode()];
            CurrentProcedure.OnEnter();
        }
        
        public void SetStartProcedure(ProcedureBase instance)  
        {
            _startHash = instance.GetType().GetHashCode();
        }

        public void AddProcedure(ProcedureBase instance)
        {
            int hash = instance.GetType().GetHashCode();
            instance.Owner = this;
            if (!_stateDict.ContainsKey(hash))
            {
                Debug.Log(instance.GetType().ToString());
                _stateDict.Add(hash,instance);
            }
            else
            {
                Debug.LogError($"重复添加State {instance.GetType()}");
            }
        }
        

        public void SetValue(string name, object value)
        {
            if (!_valueDictionary.ContainsKey(name))
            {
                _valueDictionary.Add(name, value);
            }
            else _valueDictionary[name] = value;
        }

        public T GetValue<T>(string name)
        {
            Assert.IsTrue(_valueDictionary.ContainsKey(name));
            return (T)_valueDictionary[name];
        }

        public bool TryGetValue<T>(string name, out T value)
        {
            if (!_valueDictionary.ContainsKey(name))
            {
                value = default;
                return false;
            }
            
            value=(T)_valueDictionary[name];
            return true;
        }
    }
}