using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tencent
{
    [Flags]
    public enum InputEvent
    {
        None = 0,
        Move = 1,
        Jump = 1 << 1,
        Attack = 1 << 2,
        Dash=1<<3,
        Defend=1<<4,
        Interact=1<<5,
    }

    public static class InputData
    {
        private static InputEvent m_CurEvents;
        private static InputEvent m_StartEvents;
        public static Vector2 MoveInput;
        private static Dictionary<InputEvent, ChargeData> m_ChargeDict = new();
        

        public static void Clear()
        {
            m_CurEvents = InputEvent.None;
            m_StartEvents = InputEvent.None;
            MoveInput = Vector2.zero;
            foreach (var pair in m_ChargeDict)
            {
                if (pair.Value.ChargeEndThisFrame)
                {
                    pair.Value.ChargeTime = 0;
                }
            }
        }

        public static bool HasEvent(InputEvent e)
        {
            if (e == InputEvent.Move) return MoveInput != Vector2.zero;
            return (m_CurEvents & e) != 0;
        }

        public static bool HasEventStart(InputEvent e)
        {
            return (m_StartEvents & e) != 0;
        }

        public static void AddEvent(InputEvent e)
        {
            m_CurEvents |= e;
        }

        public static void AddEventStart(InputEvent e)
        {
            m_StartEvents |= e;
        }

        public static void InitDict(InputEvent e, ChargeData d)
        {
            m_ChargeDict.Add(e, d);
        }

        public static ChargeData GetChargeData(InputEvent e)
        {
            return m_ChargeDict[e];
        }

        public static float GetChargeEndTime(InputEvent e)
        {
            var data = m_ChargeDict[e];
            if (data.ChargeEndThisFrame)
            {
                return data.ChargeTime;
            }
            else return 0;
        }

        public static bool IsCharging(InputEvent e)
        {
            return m_ChargeDict[e].Charging;
        }

        public static float GetChargingTime(InputEvent e)
        {
            if (!IsCharging(e)) return 0;
            else return GetChargeData(e).ChargeTime;
        }
    }

    public class ChargeData
    {
        public InputEvent Type = InputEvent.None;
        public float ChargeTime = 0;
        public bool Charging = false;
        public bool ChargeEndThisFrame => !Charging && ChargeTime > 0;
    }
}