using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tencent
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputMap InputMap { get; private set; }
        public PlayerInputMap.PlayerActions PlayerActions { get; private set; }

        private Vector2 m_LastFrameMoveInput;

        private Dictionary<InputEvent, InputAction> eventActionDict;

        private void Awake()
        {
            InputMap = new PlayerInputMap();
            PlayerActions = InputMap.Player;
            foreach (InputEvent e in System.Enum.GetValues(typeof(InputEvent)))
            {
                InputData.InitDict(e, new ChargeData());
            }

            eventActionDict = new()
            {
                { InputEvent.Attack, PlayerActions.Attack },
                { InputEvent.Jump, PlayerActions.Jump },
                { InputEvent.Dash, PlayerActions.Dash },
                { InputEvent.Defend ,PlayerActions.Defend},
                { InputEvent.Interact, PlayerActions.Interact }
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
            if (m_LastFrameMoveInput == Vector2.zero && PlayerActions.Move.ReadValue<Vector2>() != Vector2.zero)
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
            if (PlayerActions.Move.ReadValue<Vector2>() != Vector2.zero)
            {
                InputData.AddEvent(InputEvent.Move);
                InputData.MoveInput = PlayerActions.Move.ReadValue<Vector2>();
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