﻿using System;
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
        Crouch = 1 << 2,
        Fire = 1 << 3
    }

    public static class InputData
    {
        private static InputEvent m_CurEvents;
        private static InputEvent m_StartEvents;
        public static Vector2 MoveInput;
        public static Vector2 LookInput;

        public static void Clear()
        {
            m_CurEvents = InputEvent.None;
            m_StartEvents = InputEvent.None;
            MoveInput = Vector2.zero;
            LookInput = Vector2.zero;

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


    }

}