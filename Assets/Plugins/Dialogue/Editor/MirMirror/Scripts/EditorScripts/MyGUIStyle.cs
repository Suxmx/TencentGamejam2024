using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace MirMirrorEditor{
    public static class MyGUIStyle
    {
        private static GUIStyle m_ParamName;
        public static GUIStyle ParamName
        {
            get
            {
                if (m_ParamName == null)
                {
                    m_ParamName = new GUIStyle();
                    m_ParamName.fontSize = 15;
                    m_ParamName.normal.textColor = Color.white;
                    m_ParamName.fontStyle = FontStyle.Bold;


                }
                return m_ParamName;
            }
        }
        private static GUIStyle m_LabelStyle;
        public static GUIStyle LabelStyle
        {
            get
            {
                if (m_LabelStyle == null)
                {
                    m_LabelStyle = new GUIStyle();
                    m_LabelStyle.fontSize = 16;
                    m_LabelStyle.normal.textColor = Color.white;


                }
                return m_LabelStyle;
            }
        }

        private static GUIStyle m_ReadOnlyParam;
        public static GUIStyle ReadOnlyParam
        {
            get
            {
                if (m_ReadOnlyParam == null)
                {
                    m_ReadOnlyParam = new GUIStyle();
                    m_ReadOnlyParam.fontSize = 14;
                    m_ReadOnlyParam.normal.textColor = Color.white;
                    //m_ParamContent.fontStyle = FontStyle.Bold;
                    m_ReadOnlyParam.normal.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/LbelBG.png") as Texture2D;


                }
                return m_ReadOnlyParam;
            }
        }

        public static GUIStyle m_TextAreaParam;
        public static GUIStyle TextAreaParam
        {
            get
            {
                if (m_TextAreaParam == null)
                {
                    m_TextAreaParam = new GUIStyle();
                    m_TextAreaParam.fontSize = 14;
                    m_TextAreaParam.normal.textColor = Color.white;
                    //m_ParamContent.fontStyle = FontStyle.Bold;
                    m_TextAreaParam.normal.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/InputBG.png") as Texture2D;
                    m_TextAreaParam.wordWrap = true;
                    m_TextAreaParam.clipping = TextClipping.Clip;
                    m_TextAreaParam.fixedHeight = 200;
                    m_TextAreaParam.fixedWidth = 280;
                    //m_TextAreaParam.stretchWidth = true;

                }
                return m_TextAreaParam;
            }
        }
        private static GUIStyle m_GpsIcon;
        public static GUIStyle GpsIcon
        {
            get
            {
                if (m_GpsIcon == null)
                {
                    m_GpsIcon = new GUIStyle();
                    m_GpsIcon.normal.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/gps.png") as Texture2D;
                    m_GpsIcon.active.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/gpsClick.png") as Texture2D;
                }
                return m_GpsIcon;
            }
        }
        private static GUIStyle m_StylePointIn;
        public static GUIStyle StylePointIn
        {
            get
            {
                if (m_StylePointIn == null)
                {
                    m_StylePointIn = new GUIStyle();
                    //normal：       正常显示组件时的渲染设置Assets/MirMirror/Icons/characterBg_C.png
                    m_StylePointIn.normal.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/style_PointIn.png") as Texture2D;
                    //active：       按下控件时的渲染设置。
                    m_StylePointIn.active.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/style_PointInActivate.png") as Texture2D;
                    //hover：        鼠标悬停在控件上时的渲染设置。
                    m_StylePointIn.hover.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/style_PointInActivate.png") as Texture2D;
                }
                return m_StylePointIn;
            }
        }
        private static GUIStyle m_StylePointOut;
        public static GUIStyle StylePointOut
        {
            get
            {
                if (m_StylePointOut == null)
                {
                    m_StylePointOut = new GUIStyle();
                    m_StylePointOut.normal.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/style_PointOut.png") as Texture2D;
                    //active：       按下控件时的渲染设置。
                    m_StylePointOut.active.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/style_PointOutActivate.png") as Texture2D;
                    //hover：        鼠标悬停在控件上时的渲染设置。
                    m_StylePointOut.hover.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/style_PointOutActivate.png") as Texture2D;
                }
                return m_StylePointOut;
            }
        }
        private static GUIStyle m_NodeEditTextStyle;
        public static GUIStyle NodeEditTextStyle
        {
            get
            {
                if (m_NodeEditTextStyle == null)
                {
                    m_NodeEditTextStyle = new GUIStyle();
                    m_NodeEditTextStyle.fontSize = 15;

                    m_NodeEditTextStyle.alignment = TextAnchor.UpperLeft;
                    m_NodeEditTextStyle.wordWrap = true;
                    m_NodeEditTextStyle.clipping = TextClipping.Clip;
                    m_NodeEditTextStyle.normal.textColor = Color.white;
                    m_NodeEditTextStyle.normal.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/LbelBG.png") as Texture2D;

                }
                return m_NodeEditTextStyle;
            }
        }
        private static GUIStyle m_NodeTextAreaStyle;
        public static GUIStyle NodeTextAreaStyle
        {
            get
            {
                if (m_NodeTextAreaStyle == null)
                {
                    m_NodeTextAreaStyle = new GUIStyle();
                    m_NodeTextAreaStyle.fontSize = 15;

                    m_NodeTextAreaStyle.alignment = TextAnchor.UpperLeft;
                    m_NodeTextAreaStyle.wordWrap = true;
                    m_NodeTextAreaStyle.clipping = TextClipping.Clip;
                    m_NodeTextAreaStyle.normal.textColor = Color.white;
                    m_NodeTextAreaStyle.normal.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/InputBG.png") as Texture2D;
                }
                return m_NodeTextAreaStyle;
            }
        }
        private static GUIStyle m_NodeTextFieldStyle;
        public static GUIStyle NodeTextFieldStyle
        {
            get
            {
                if (m_NodeTextFieldStyle == null)
                {

                    m_NodeTextFieldStyle = new GUIStyle();
                    m_NodeTextFieldStyle.fontSize = 15;

                    m_NodeTextFieldStyle.alignment = TextAnchor.MiddleLeft;
                    m_NodeTextFieldStyle.clipping = TextClipping.Clip;
                    m_NodeTextFieldStyle.normal.textColor = Color.white;
                    m_NodeTextFieldStyle.normal.background = EditorGUIUtility.Load("Assets/Plugins/Dialogue/Editor/Icons/InputBG.png") as Texture2D;

                }
                return m_NodeTextFieldStyle;
            }
        }
        private static GUIStyle m_NodeTitleStyle;
        public static GUIStyle NodeTitleStyle
        {
            get
            {
                if (m_NodeTitleStyle == null)
                {
                    m_NodeTitleStyle = new GUIStyle();
                    m_NodeTitleStyle.alignment = TextAnchor.MiddleCenter;
                    m_NodeTitleStyle.fontStyle = FontStyle.Bold;
                    m_NodeTitleStyle.fontSize = 13;

                    m_NodeTitleStyle.normal.textColor = Color.black;





                }
                return m_NodeTitleStyle;
            }
        }
    }
}