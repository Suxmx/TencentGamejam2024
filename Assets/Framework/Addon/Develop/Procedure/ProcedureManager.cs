using System;
using System.Collections;
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
            _core = new ProcedureFsm();
        }

        protected override void Start()
        {
            base.Start();
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
    }
}