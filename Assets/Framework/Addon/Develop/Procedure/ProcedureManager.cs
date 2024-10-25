using System;
using System.Collections;
using System.Linq;
using Services;
using UnityEngine;

namespace Framework
{
    public class ProcedureManager : Service, IProcedureManager
    {
        public ProcedureBase CurrentProcedure => _core.CurrentProcedure;


        private ProcedureFsm _core;
        private ProcedureBase m_EntranceProcedure = null;
        private bool _hasStart = false;

        [SerializeField] private string[] m_AvailableProcedureTypeNames = null;

        [SerializeField] private string m_EntranceProcedureTypeName = null;

        protected override void Awake()
        {
            base.Awake();
            if (_core is null)
                _core = new ProcedureFsm();
        }

        protected override void Start()
        {
            base.Start();
            if (!_hasStart)
                StartCoroutine(nameof(StartProcedure));
        }

        private void Update()
        {
            if (!_hasStart) return;
            _core.OnLogic(Time.deltaTime);
        }

        private IEnumerator StartProcedure()
        {
            yield return new WaitForEndOfFrame();
            ProcedureBase[] procedures = new ProcedureBase[m_AvailableProcedureTypeNames.Length];

            for (int i = 0; i < m_AvailableProcedureTypeNames.Length; i++)
            {
                Type procedureType = TypeUtils.GetType(m_AvailableProcedureTypeNames[i]);
                if (procedureType == null)
                {
                    Debug.LogErrorFormat("Can not find procedure type '{0}'.", m_AvailableProcedureTypeNames[i]);
                    yield break;
                }

                procedures[i] = (ProcedureBase)Activator.CreateInstance(procedureType);
                if (procedures[i] == null)
                {
                    Debug.LogErrorFormat("Can not create procedure instance '{0}'.", m_AvailableProcedureTypeNames[i]);
                    yield break;
                }

                if (m_EntranceProcedureTypeName == m_AvailableProcedureTypeNames[i])
                {
                    m_EntranceProcedure = procedures[i];
                }

                _core.AddProcedure(procedures[i]);
            }

            if (m_EntranceProcedure == null)
            {
                Debug.LogErrorFormat("Entrance procedure is invalid.");
                yield break;
            }
            else
            {
                _core.SetStartProcedure(m_EntranceProcedure);
            }

            _hasStart = true;
            _core.Start();
        }

        public void StartImmediately()
        {
            ProcedureBase[] procedures = new ProcedureBase[m_AvailableProcedureTypeNames.Length];

            for (int i = 0; i < m_AvailableProcedureTypeNames.Length; i++)
            {
                Type procedureType = TypeUtils.GetType(m_AvailableProcedureTypeNames[i]);
                if (procedureType == null)
                {
                    Debug.LogErrorFormat("Can not find procedure type '{0}'.", m_AvailableProcedureTypeNames[i]);
                    return;
                }

                procedures[i] = (ProcedureBase)Activator.CreateInstance(procedureType);
                if (procedures[i] == null)
                {
                    Debug.LogErrorFormat("Can not create procedure instance '{0}'.", m_AvailableProcedureTypeNames[i]);
                    return;
                }

                if (m_EntranceProcedureTypeName == m_AvailableProcedureTypeNames[i])
                {
                    m_EntranceProcedure = procedures[i];
                }

                _core.AddProcedure(procedures[i]);
            }

            if (m_EntranceProcedure == null)
            {
                Debug.LogErrorFormat("Entrance procedure is invalid.");
                return;
            }
            else
            {
                _core.SetStartProcedure(m_EntranceProcedure);
            }

            _hasStart = true;
            _core.Start();
        }

        public void SetValue(string name, object value)
        {
            if (_core is null)
            {
                _core = new ProcedureFsm();
            }

            _core.SetValue(name, value);
        }

        public T GetValue<T>(string name)
            => _core.GetValue<T>(name);
        
        

        public void AddProcedure<T>() where T : ProcedureBase
        {
            if (m_AvailableProcedureTypeNames is null)
            {
                m_AvailableProcedureTypeNames = new string[1];
                m_AvailableProcedureTypeNames[0] = typeof(T).ToString();
                return;
            }

            int length = m_AvailableProcedureTypeNames.Length + 1;
            var copy = m_AvailableProcedureTypeNames.ToList();
            m_AvailableProcedureTypeNames = new string[length];
            for (int i = 0; i < copy.Count; i++)
            {
                m_AvailableProcedureTypeNames[i] = copy[i];
            }

            m_AvailableProcedureTypeNames[^1] = typeof(T).ToString();
        }

        public void SetStartProcedure<T>() where T : ProcedureBase
        {
            m_EntranceProcedureTypeName = typeof(T).ToString();
        }
    }
}