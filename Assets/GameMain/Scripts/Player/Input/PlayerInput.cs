using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tencent
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputMap InputMap { get; private set; }
        public PlayerInputMap.GroundMoveActions GroundMoveActions { get; private set; }
        
        public PlayerInputMap.CameraActions CameraActions { get; private set; }

        private Vector2 m_LastFrameMoveInput;

        private Dictionary<InputEvent, InputAction> eventActionDict;

        private void Awake()
        {
            InputMap = new PlayerInputMap();
            GroundMoveActions = InputMap.GroundMove;
            CameraActions = InputMap.Camera;
            foreach (InputEvent e in System.Enum.GetValues(typeof(InputEvent)))
            {
                InputData.InitDict(e, new ChargeData());
            }

            eventActionDict = new()
            {
                { InputEvent.Jump, GroundMoveActions.Jump },
            };
        }

        private void OnEnable()
        {
            InputMap.Enable();
        }

        private void OnDisable()
        {
            InputMap.Disable();
        }

        public void UpdateInput(bool clear=true)
        {
            m_LastFrameMoveInput = InputData.MoveInput;
            if(clear)
            {
                InputData.Clear();
            }
            CheckEventStart();
            CheckHasEvent();
            UpdateChargeData();
        }

        private void CheckEventStart()
        {
            if (m_LastFrameMoveInput == Vector2.zero && GroundMoveActions.Move.ReadValue<Vector2>() != Vector2.zero)
            {
                InputData.AddEventStart(InputEvent.Move);
            }

            foreach (var pair in eventActionDict)
            {
                if (pair.Value.triggered)
                {
                    InputData.AddEventStart(pair.Key);
                }
            }
        }

        private void CheckHasEvent()
        {
            //Move
            if (GroundMoveActions.Move.ReadValue<Vector2>() != Vector2.zero)
            {
                InputData.AddEvent(InputEvent.Move);
                InputData.MoveInput = GroundMoveActions.Move.ReadValue<Vector2>();
            }

            if (CameraActions.Look.ReadValue<Vector2>() != Vector2.zero)
            {
                InputData.LookInput = CameraActions.Look.ReadValue<Vector2>();
            }

            foreach (var pair in eventActionDict)
            {
                if (pair.Value.phase == InputActionPhase.Performed ||
                    pair.Value.phase == InputActionPhase.Started)
                {
                    InputData.AddEvent(pair.Key);
                }
            }
        }

        private void UpdateChargeData()
        {
            foreach (InputEvent e in System.Enum.GetValues(typeof(InputEvent)))
            {
                var data = InputData.GetChargeData(e);
                if (InputData.HasEvent(e))
                {
                    data.Charging = true;
                    data.ChargeTime += Time.deltaTime;
                }

                if (!InputData.HasEvent(e) && data.Charging)
                {
                    data.Charging = false;
                }
            }
        }
    }
}