﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using MirMirror;
using System.Text.RegularExpressions;


namespace MirMirrorEditor
{
    public class MM_DialogueNodeEditor : EditorWindow
    {
        public int m_CurtPartID;
        private MMDialogueRtfEditor m_DialogueSpeedEditor = null;
        //private EventNodeDetailsWindow m_EventNodeDetailsWindow = null;

        private MMDialogue_Gragh m_MMDialogue_Gragh;
        public MMDialogue_Gragh MMDialogue_Gragh { get { return m_MMDialogue_Gragh; }set { m_MMDialogue_Gragh = value; } }
        private MMDialogue_Gragh m_CurMMDialogue_Gragh;
        public MMDialogue_Gragh CurMMDialogue_Gragh { get { return m_CurMMDialogue_Gragh; } set { m_CurMMDialogue_Gragh = value; } }

        private ConnectionPoint m_CurSelectPoint;
        public ConnectionPoint CurSelectPoint{ get { return m_CurSelectPoint; }set { m_CurSelectPoint = value; } }

        private bool m_IsDeleteLineMode = false;
        public bool IsDeleteLineMode { get { return m_IsDeleteLineMode; } }
        private bool m_IsDraggingCanvas;
        //public GUIStyle m_StylePointIn;
        //public GUIStyle m_StylePointOut;
        //public GUIStyle m_NodeTitleStyle;
        //public GUIStyle m_NodeAttrStyle;
        //public GUIStyle m_NodeTextFieldStyle;
        //public GUIStyle m_NodeTextAreaStyle;
        //public GUIStyle m_NodeEditTextStyle;
        private Vector2 Gm_ridOffset;

        [MenuItem("Tools/MVDE")]
        private static void OpenWindow()
        {
            MM_DialogueNodeEditor nodeEditor = GetWindow<MM_DialogueNodeEditor>(false, "MMDialogueNodeEditor");


        }
        private void OnDestroy()
        {
            //m_EventNodeDetailsWindow?.Close();
            m_DialogueSpeedEditor?.Close();
        }
        private void OnGUI()
        {

            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            DrawNodes();
            DrawConnectionLines();
            ProcessNodesEvent();
            ProcessEvents(Event.current);
            DrawPendingLine(Event.current);
            DrawHeader();






            if (GUI.changed)
            {
                Repaint();
            }
        }

        private void OnEnable()
        {
            //初始化GraghMM_Dialogue
            m_CurtPartID = 0;

            MMDialogue_Gragh tempDialogue = (MMDialogue_Gragh)CreateInstance(typeof(MMDialogue_Gragh));
            CurMMDialogue_Gragh = tempDialogue;
            CurMMDialogue_Gragh.m_MMDialogue_Data = null;
            //初始化GraghMM_Dialogue.DataMM_Dialogue
            tempDialogue.m_DialoguePartGraghDatas = new List<DialoguePartGraghData>();
            tempDialogue.m_DialoguePartGraghDatas.Add(new DialoguePartGraghData());
            //tempDialogue.m_GraghDialogueNodes = new List<GraghDialogueNode>();
            //tempDialogue.m_GraghChoiceNodes = new List<GraghChoiceNode>();
            //tempDialogue.m_GraghEventNodes = new List<GraghEventNode>();
            //tempDialogue.m_Points = new List<ConnectionPoint>();
            tempDialogue.m_MMDialogue_Data = (MMDialogue_Data)CreateInstance(typeof(MMDialogue_Data));
            tempDialogue.m_MMDialogue_Data.m_DialoguePartDatas = new List<DialoguePartData>();
            tempDialogue.m_MMDialogue_Data.m_DialoguePartDatas.Add(new DialoguePartData());
            //tempDialogue.m_MMDialogue_Data.m_StartNode = new DataStartNode();
            //tempDialogue.m_MMDialogue_Data.m_DataDialogueNodes = new List<DataDialogueNode>();
            //tempDialogue.m_MMDialogue_Data.m_DataChoieceNodes = new List<DataChoieceNode>();
            //tempDialogue.m_MMDialogue_Data.m_DataEventNodes = new List<DataEventNode>();
            //tempDialogue.m_Lines = new List<ConnectionLine>();
            AddStartNode();

            //InitializeGUIStyle();

            CurSelectPoint = null;
            if (MMDialogue_Gragh!=null)
            {
                UpdateData();
            }



        }
        //private void InitializeGUIStyle()
        //{


        //    m_StylePointIn = new GUIStyle();
        //    //normal：       正常显示组件时的渲染设置Assets/MirMirror/Icons/characterBg_C.png
        //    m_StylePointIn.normal.background = EditorGUIUtility.Load("Assets/MirMirror/Icons/style_PointIn.png") as Texture2D;
        //    //active：       按下控件时的渲染设置。
        //    m_StylePointIn.active.background = EditorGUIUtility.Load("Assets/MirMirror/Icons/style_PointInActivate.png") as Texture2D;
        //    //hover：        鼠标悬停在控件上时的渲染设置。
        //    m_StylePointIn.hover.background = EditorGUIUtility.Load("Assets/MirMirror/Icons/style_PointInActivate.png") as Texture2D;
        //    m_StylePointOut = new GUIStyle();
        //    m_StylePointOut.normal.background = EditorGUIUtility.Load("Assets/MirMirror/Icons/style_PointOut.png") as Texture2D;
        //    //active：       按下控件时的渲染设置。
        //    m_StylePointOut.active.background = EditorGUIUtility.Load("Assets/MirMirror/Icons/style_PointOutActivate.png") as Texture2D;
        //    //hover：        鼠标悬停在控件上时的渲染设置。
        //    m_StylePointOut.hover.background = EditorGUIUtility.Load("Assets/MirMirror/Icons/style_PointOutActivate.png") as Texture2D;

        //    //Node Title
        //    m_NodeTitleStyle = new GUIStyle();
        //    m_NodeTitleStyle.alignment = TextAnchor.MiddleCenter;
        //    m_NodeTitleStyle.fontStyle = FontStyle.Bold;
        //    m_NodeTitleStyle.fontSize = 13;

        //    m_NodeTitleStyle.normal.textColor = Color.black;



        //    m_NodeAttrStyle = new GUIStyle();
        //    //m_NodeAttrStyle.alignment = TextAnchor.MiddleCenter;
        //    //m_NodeAttrStyle.fontStyle = FontStyle.Bold;
        //    m_NodeAttrStyle.fontSize = 13;

        //    m_NodeAttrStyle.normal.textColor = Color.black;

        //    m_NodeTextFieldStyle = new GUIStyle();
        //    m_NodeTextFieldStyle.fontSize = 15;

        //    m_NodeTextFieldStyle.alignment = TextAnchor.MiddleLeft;
        //    m_NodeTextFieldStyle.clipping = TextClipping.Clip;
        //    m_NodeTextFieldStyle.normal.textColor = Color.white;
        //    m_NodeTextFieldStyle.normal.background = EditorGUIUtility.Load("Assets/MirMirror/Icons/InputBG.png") as Texture2D;

        //    m_NodeTextAreaStyle = new GUIStyle();
        //    m_NodeTextAreaStyle.fontSize = 15;

        //    m_NodeTextAreaStyle.alignment = TextAnchor.UpperLeft;
        //    m_NodeTextAreaStyle.wordWrap = true;
        //    m_NodeTextAreaStyle.clipping = TextClipping.Clip;
        //    m_NodeTextAreaStyle.normal.textColor = Color.white;
        //    m_NodeTextAreaStyle.normal.background = EditorGUIUtility.Load("Assets/MirMirror/Icons/InputBG.png") as Texture2D;


        //    m_NodeEditTextStyle = new GUIStyle();
        //    m_NodeEditTextStyle.fontSize = 15;

        //    m_NodeEditTextStyle.alignment = TextAnchor.UpperLeft;
        //    m_NodeEditTextStyle.wordWrap = true;
        //    m_NodeEditTextStyle.clipping = TextClipping.Clip;
        //    m_NodeEditTextStyle.normal.textColor = Color.white;
        //    m_NodeEditTextStyle.normal.background = EditorGUIUtility.Load("Assets/MirMirror/Icons/LbelBG.png") as Texture2D;

        //}
        private void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        ClickMouseBtnRight(e.mousePosition);

                    }
                    else if (e.button == 0)
                    {

                        CurSelectPoint = null;


                    }
                    else if (e.button == 2)
                    {

                        m_IsDraggingCanvas = true;

                    }
                    break;
                case EventType.MouseUp:
                    if (e.button == 2)
                    {
                        m_IsDraggingCanvas = false;
                    }
                    break;
                case EventType.KeyDown:
                    if (e.keyCode == KeyCode.LeftShift)
                    {

                        m_IsDeleteLineMode = true;
                        GUI.changed = true;
                    }
                    break;
                case EventType.KeyUp:
                    if (e.keyCode == KeyCode.LeftShift)
                    {

                        m_IsDeleteLineMode = false;
                        GUI.changed = true;
                    }

                    break;
                case EventType.MouseDrag:
                    if (m_IsDraggingCanvas == true)
                    {
                        DragCanvas(e.delta);
                        Gm_ridOffset += e.delta;
                        e.Use();

                    }
                    break;
                case EventType.ScrollWheel:

                    break;
                default:
                    break;
            }
        }
        private void ClickMouseBtnRight(Vector2 mousePose)
        {
            bool isInNodeRect = false;
            bool isInNodeTitle = false;
            MyGraghNode targetNode = null;

            if (CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghStartNode.m_Rect.Contains(Event.current.mousePosition))
            {
                isInNodeRect = true;


            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghDialogueNodes.Count; i++)
            {
                if (CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghDialogueNodes[i].m_Rect.Contains(Event.current.mousePosition))
                {
                    isInNodeRect = true;

                    break;
                }
            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghChoiceNodes.Count; i++)
            {
                if (CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghChoiceNodes[i].m_Rect.Contains(Event.current.mousePosition))
                {
                    isInNodeRect = true;

                    break;
                }
            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghEventNodes.Count; i++)
            {
                if (CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghEventNodes[i].m_Rect.Contains(Event.current.mousePosition))
                {
                    isInNodeRect = true;

                    break;
                }
            }

            if (CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghStartNode.m_TitleRect.Contains(Event.current.mousePosition))
            {
                isInNodeTitle = true;
                targetNode = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghStartNode;

            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghDialogueNodes.Count; i++)
            {
                if (CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghDialogueNodes[i].m_TitleRect.Contains(Event.current.mousePosition))
                {
                    isInNodeTitle = true;
                    targetNode = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghDialogueNodes[i];
                    break;
                }
            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghChoiceNodes.Count; i++)
            {
                if (CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghChoiceNodes[i].m_TitleRect.Contains(Event.current.mousePosition))
                {
                    isInNodeTitle = true;
                    targetNode = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghChoiceNodes[i];
                    break;
                }
            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghEventNodes.Count; i++)
            {
                if (CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghEventNodes[i].m_TitleRect.Contains(Event.current.mousePosition))
                {
                    isInNodeTitle = true;
                    targetNode = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghEventNodes[i];
                    break;
                }
            }
            if (isInNodeRect == false)
            {
                GenericMenu genericMenu = new GenericMenu();
               
                genericMenu.AddItem(new GUIContent("AddDialogueNode"), false, () => AddDialogueNode(mousePose));
                genericMenu.AddItem(new GUIContent("AddChioceNode"), false, () => AddChioceNode(mousePose));
                genericMenu.AddItem(new GUIContent("AddEventNode"), false, () => AddEventNode(mousePose));
        
                genericMenu.ShowAsContext();
            }
            if (isInNodeTitle)

            {
                GenericMenu genericMenu = new GenericMenu();
                genericMenu.AddItem(new GUIContent("DeleteNode"), false, () => DeleteNode(targetNode));
                genericMenu.ShowAsContext();
            }

        }
        private void AddStartNode()
        {
            NodeID_Type nodeID_Type = new NodeID_Type(0, NODETYPE.START);
            CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghStartNode = new GraghStartNode(new Vector2(50, 100), new Vector2(180, 80), nodeID_Type,this);

        }
        private void AddDialogueNode(Vector2 mousePos)
        {
            NodeID_Type nodeID_Type = new NodeID_Type(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetNodeUniqueID(NODETYPE.DIALOGUE), NODETYPE.DIALOGUE);
            GraghDialogueNode tempNode = new GraghDialogueNode(mousePos, new Vector2(300, 130), nodeID_Type,this);

            //m_CurGraghMM_Dialogue.m_GraghDialogueNodes.Add(tempNode);
            //m_CurGraghMM_Dialogue.m_DataDialogue.m_DataDialogueNodes.Add(new DataDialogueNode(m_CurGraghMM_Dialogue.m_GraghDialogueNodes[m_CurGraghMM_Dialogue.m_GraghDialogueNodes.Count-1].DataNode.ID));


        }
        private void AddChioceNode(Vector2 mousePos)
        {
            NodeID_Type nodeID_Type = new NodeID_Type(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetNodeUniqueID(NODETYPE.CHOICE), NODETYPE.CHOICE);
            GraghChoiceNode tempNode = new GraghChoiceNode(mousePos, new Vector2(150, 95), nodeID_Type,this);

            //m_CurGraghMM_Dialogue.m_GraghChoiceNodes.Add(tempNode);
            //m_CurGraghMM_Dialogue.m_DataDialogue.m_DataChoieceNodes.Add(new DataChoieceNode(m_CurGraghMM_Dialogue.m_GraghChoiceNodes[m_CurGraghMM_Dialogue.m_GraghChoiceNodes.Count - 1].DataNode.ID));


        }
        private void AddEventNode(Vector2 mousePos)
        {
            NodeID_Type nodeID_Type = new NodeID_Type(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetNodeUniqueID(NODETYPE.EVENT), NODETYPE.EVENT);
            GraghEventNode tempNode = new GraghEventNode(mousePos, new Vector2(200, 80), nodeID_Type,this);

            //m_CurGraghMM_Dialogue.m_GraghEventNodes.Add(tempNode);
            //m_CurGraghMM_Dialogue.m_DataDialogue.m_DataEventNodes.Add(new DataEventNode(m_CurGraghMM_Dialogue.m_GraghEventNodes[m_CurGraghMM_Dialogue.m_GraghEventNodes.Count - 1].DataNode.ID));


        }
        private void DragCanvas(Vector2 delta)
        {
            CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghStartNode.ProcessDrag(delta);
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghDialogueNodes.Count; i++)
            {
                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghDialogueNodes[i].ProcessDrag(delta);
            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghChoiceNodes.Count; i++)
            {
                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghChoiceNodes[i].ProcessDrag(delta);
            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghEventNodes.Count; i++)
            {
                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghEventNodes[i].ProcessDrag(delta);
            }
        }
        private void DeleteNode(MyGraghNode myNode)
        {
            if (myNode.NodeType == NODETYPE.START)
            {
                Debug.LogWarning("You can not remove StartNode!");
                return;
            }
            List<ConnectionLine> connectionLines = new List<ConnectionLine>();
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines.Count; i++)
            {
                if (CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines[i].m_StartPoint.Owner == myNode)
                {
                    connectionLines.Add(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines[i]);
                }
                else if (CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines[i].m_EndPoint.Owner == myNode)
                {
                    connectionLines.Add(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines[i]);
                }
            }



            for (int i = 0; i < connectionLines.Count; i++)
            {
                //移除nextDataNode
                //DataMMNode targetNode = connectionLines[i].m_InPoint.m_Owner.DataNode;

                //DataMMNode nextNode= connectionLines[i].m_OutPoint.m_Owner.DataNode;
                NodeID_Type targetNode = connectionLines[i].m_StartPoint.Owner.DataNode;
                switch (targetNode.m_NODETYPE)

                {
                    case NODETYPE.START:
                        CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].m_StartNode.NextNodes()[0] = new NodeID_Type(-1, NODETYPE.START);
                        break;
                    case NODETYPE.DIALOGUE:
                        CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(targetNode.ID).NextNodes()[0] = new NodeID_Type(-1, NODETYPE.START);
                        break;
                    case NODETYPE.CHOICE:
                        CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(targetNode.ID).NextNodes()[connectionLines[i].m_StartPoint.m_NodePointNo] = new NodeID_Type(-1, NODETYPE.START);
                        break;
                    case NODETYPE.EVENT:
                        break;
                    default:
                        break;
                }

                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines.Remove(connectionLines[i]);
  
            }
            if (myNode != null)
            {
                //m_CurGraghMM_Dialogue.m_Nodes.Remove(myNode);
                switch (myNode.NodeType)

                {
                    case NODETYPE.START:
                        Debug.LogWarning("StartNode can not be removed!");

                        break;
                    case NODETYPE.DIALOGUE:

                        CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].RemoveGraghDialogueNode(myNode.DataNode.ID);
                        CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].RemoveDataDialogueNode(myNode.DataNode.ID);
                        break;
                    case NODETYPE.CHOICE:


                        CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].RemoveGraghChoieceNode(myNode.DataNode.ID);
                        CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].RemoveDataChoieceNode(myNode.DataNode.ID);
                        break;
                    case NODETYPE.EVENT:
                        CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].RemoveGraghEventNode(myNode.DataNode.ID);
                        CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].RemoveDataEventNode(myNode.DataNode.ID);
                        break;
                    default:
                        break;
                }

                for (int i = 0; i < myNode.m_InPointsID.Count; i++)
                {
                    //m_CurGraghMM_Dialogue.m_Points.Remove(myNode.m_InPoints[i]);
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].RemovePoint(myNode.m_InPointsID[i]);
                }
                for (int i = 0; i < myNode.m_OutPointsID.Count; i++)
                {
                    //m_CurGraghMM_Dialogue.m_Points.Remove(myNode.m_OutPoints[i]);
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].RemovePoint(myNode.m_OutPointsID[i]);
                }



                GUI.changed = true;
            }

        }

        private void DrawHeader()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, 50), Color.grey);
           
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Data",MyGUIStyle.LabelStyle);
            MMDialogue_Gragh = (MMDialogue_Gragh)EditorGUILayout.ObjectField(MMDialogue_Gragh, typeof(MMDialogue_Gragh), false);

            //EditorGUI.DrawRect(new Rect(260, 4, 1, 32), Color.white);
            if (GUILayout.Button("Reset"))
            {
                if (MMDialogue_Gragh != null)
                {

                    UpdateData();
                }
            }
            if (GUILayout.Button("Save"))
            {
                if (MMDialogue_Gragh == null)
                {
                    ExportData();
                }
                else
                {
                    SaveData();
                }
            }
            if (GUILayout.Button( "Clear"))
            {
                ClearData();
            }
            if (GUILayout.Button("EditMode"))
            {
                EditMode();
            }
            if (GUILayout.Button("Export Dialogue Data"))
            {

                ExportData();
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("链接NPC ID:", MyGUIStyle.LabelStyle);
            //EditorGUILayout.LabelField(CurMMDialogue_Gragh.m_MMDialogue_Data.CharacterID.ToString(),MyGUIStyle.ReadOnlyParam);
            CurMMDialogue_Gragh.m_MMDialogue_Data.CharacterID= EditorGUILayout.IntSlider(CurMMDialogue_Gragh.m_MMDialogue_Data.CharacterID,0,1000);
            EditorGUILayout.LabelField("当前PartID:", MyGUIStyle.LabelStyle);
            m_CurtPartID=EditorGUILayout.IntSlider(m_CurtPartID,0,CurMMDialogue_Gragh.m_DialoguePartGraghDatas.Count-1);
            if (GUILayout.Button("添加组"))
            {
                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas.Add(new DialoguePartData());
                CurMMDialogue_Gragh.m_DialoguePartGraghDatas.Add(new DialoguePartGraghData());
                m_CurtPartID = CurMMDialogue_Gragh.m_DialoguePartGraghDatas.Count - 1;
                AddStartNode();
            }
            if (GUILayout.Button("删除组"))
            {
                if (CurMMDialogue_Gragh.m_DialoguePartGraghDatas.Count==1)
                {
                    Debug.Log("<color=red>必须保留一组对话</color>");
                }
                else
                {
                    int tempPart = m_CurtPartID;
                    m_CurtPartID = m_CurtPartID - 1;
                    CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas.RemoveAt(tempPart);
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas.RemoveAt(tempPart);
                    if (m_CurtPartID<0)
                    {
                        m_CurtPartID = 0;
                    }
                }
            }

            if (GUILayout.Button("对话组上移"))
            {
                DialoguePartData dialoguePartData = CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID];
                DialoguePartGraghData dialoguePartGraghData = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID];
                int targetID = m_CurtPartID - 1;
                if (targetID<0)
                {
                    targetID = 0;
                }
                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID] = CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[targetID];
                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID] = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[targetID];

                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[targetID] = dialoguePartData;
                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[targetID] = dialoguePartGraghData;
                m_CurtPartID = targetID;
            }
            if (GUILayout.Button("对话组下移"))
            {
                DialoguePartData dialoguePartData = CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID];
                DialoguePartGraghData dialoguePartGraghData = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID];
                int targetID = m_CurtPartID + 1;
                if (targetID >= CurMMDialogue_Gragh.m_DialoguePartGraghDatas.Count)
                {
                    targetID = CurMMDialogue_Gragh.m_DialoguePartGraghDatas.Count-1;
                }
                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID] = CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[targetID];
                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID] = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[targetID];

                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[targetID] = dialoguePartData;
                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[targetID] = dialoguePartGraghData;
                m_CurtPartID = targetID;
            }
            GUILayout.EndHorizontal();
           

            GUILayout.EndVertical();


        }
        private bool m_EditMode = false;
        private void EditMode()
        {
            if (m_EditMode==false)
            {
                m_EditMode = true;
            }
            else
            {
                m_EditMode = false;
            }
        }
        private void EditShowContent(string _In,out string _Result)
        {
            string text = _In;
            string pattern = @"<(\d+)(\.\d+)?>|(</r>|(<r=.*?>))|(</b>|<b>)|(</i>|<i>)|(</size>|(<size=.*?>))|(</color>|(<color=.*?>))";
            _Result = Regex.Replace(text, pattern, "");
        }
        private void UpdateData()
        {
            Debug.Log("刷新数据");
            CurMMDialogue_Gragh = (MMDialogue_Gragh)CreateInstance(typeof(MMDialogue_Gragh));
            CurMMDialogue_Gragh.m_MMDialogue_Data = (MMDialogue_Data)CreateInstance(typeof(MMDialogue_Data));
            //Debug.Log("存档对话数量:"+ CurMMDialogue_Gragh.m_DialoguePartGraghDatas.Count);
            CurMMDialogue_Gragh.m_MMDialogue_Data.SetCharacterID(MMDialogue_Gragh.m_MMDialogue_Data.CharacterID);
            CurMMDialogue_Gragh.m_MMDialogue_Data.SetCharacterName(MMDialogue_Gragh.m_MMDialogue_Data.CharacterName);

            CurMMDialogue_Gragh.m_DialoguePartGraghDatas = new List<DialoguePartGraghData>();
            CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas = new List<DialoguePartData>();
            for (int i = 0; i < MMDialogue_Gragh.m_DialoguePartGraghDatas.Count; i++)
            {
                CurMMDialogue_Gragh.m_DialoguePartGraghDatas.Add(new DialoguePartGraghData());
                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas.Add(new DialoguePartData());

                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_StartNode = new DataStartNode(MMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_StartNode);
                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataDialogueNodes = new List<DataDialogueNode>();
                for (int j = 0; j < MMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataDialogueNodes.Count; j++)
                {
                    CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataDialogueNodes.Add(new DataDialogueNode(MMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataDialogueNodes[j]));
                }
                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataChoieceNodes = new List<DataChoieceNode>();
                for (int j = 0; j < MMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataChoieceNodes.Count; j++)
                {
                    CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataChoieceNodes.Add(new DataChoieceNode(MMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataChoieceNodes[j]));
                }
                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataEventNodes = new List<DataEventNode>();
                for (int j = 0; j < MMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataEventNodes.Count; j++)
                {
                    CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataEventNodes.Add(new DataEventNode(MMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataEventNodes[j]));
                }






                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghStartNode = new GraghStartNode(MMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghStartNode, this);

                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghDialogueNodes = new List<GraghDialogueNode>();
                for (int j = 0; j < MMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghDialogueNodes.Count; j++)
                {
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghDialogueNodes.Add(new GraghDialogueNode(MMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghDialogueNodes[j], this));
                }

                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghChoiceNodes = new List<GraghChoiceNode>();
                for (int j = 0; j < MMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghChoiceNodes.Count; j++)
                {
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghChoiceNodes.Add(new GraghChoiceNode(MMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghChoiceNodes[j], this));
                }

                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghEventNodes = new List<GraghEventNode>();
                for (int j = 0; j < MMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghEventNodes.Count; j++)
                {
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghEventNodes.Add(new GraghEventNode(MMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghEventNodes[j], this));
                }

                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_Lines = new List<ConnectionLine>();
                for (int j = 0; j < MMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_Lines.Count; j++)
                {
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_Lines.Add(new ConnectionLine(MMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_Lines[j], this));
                }

                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_Points = new List<ConnectionPoint>();
                for (int j = 0; j < MMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_Points.Count; j++)
                {
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_Points.Add(new ConnectionPoint(MMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_Points[j], this));
                }


            }






        }
        private void SaveData()
        {

            SaveDataData(ref m_MMDialogue_Gragh.m_MMDialogue_Data);
            
            EditorUtility.SetDirty(m_MMDialogue_Gragh.m_MMDialogue_Data);
            
            SaveGraghData(ref m_MMDialogue_Gragh);
            
            EditorUtility.SetDirty(m_MMDialogue_Gragh);
        }
        private void ClearData()
        {
            CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].ResetDataData();
            CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].ResetGraghData();
            AddStartNode();
        }
        private void ExportData()
        {

            MMDialogue_Data tempDataFile = (MMDialogue_Data)CreateInstance(typeof(MMDialogue_Data));
            
            string dataFilePath = EditorUtility.SaveFilePanelInProject("Save MM Data File", "", "asset", "Enter a file name to save!", EditorPrefs.GetString("MM_LastPath", "Assets"));
            if (dataFilePath.Length != 0)
            {
                EditorPrefs.SetString("MM_LastPath", dataFilePath);
                SaveDataData(ref tempDataFile);
                AssetDatabase.CreateAsset(tempDataFile, dataFilePath);
                EditorUtility.SetDirty(tempDataFile);

            }
            else
            {
                return;
            }

            MMDialogue_Gragh tempGraghFile = (MMDialogue_Gragh)CreateInstance(typeof(MMDialogue_Gragh));
            tempGraghFile.m_MMDialogue_Data = tempDataFile;
            string graghFilePath = EditorUtility.SaveFilePanelInProject("Save MM Gragh File", "", "asset", "Enter a file name to save!", EditorPrefs.GetString("MM_LastPath", "Assets"));
            if (graghFilePath.Length != 0)
            {
                EditorPrefs.SetString("MM_LastPath", graghFilePath);
                SaveGraghData(ref tempGraghFile);
                AssetDatabase.CreateAsset(tempGraghFile, graghFilePath);
                EditorUtility.SetDirty(tempGraghFile);
                MMDialogue_Gragh = tempGraghFile;
            }




        }
        private void SaveDataData(ref MMDialogue_Data _Dialogue)
        {
            _Dialogue.SetCharacterID(CurMMDialogue_Gragh.m_MMDialogue_Data.CharacterID);
            _Dialogue.SetCharacterName(CurMMDialogue_Gragh.m_MMDialogue_Data.CharacterName);
            _Dialogue.m_DialoguePartDatas = new List<DialoguePartData>();
            for (int i = 0; i < CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas.Count; i++)
            {
                _Dialogue.m_DialoguePartDatas.Add(new DialoguePartData());

                _Dialogue.m_DialoguePartDatas[i].m_StartNode = new DataStartNode(CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_StartNode);
                _Dialogue.m_DialoguePartDatas[i].m_DataDialogueNodes = new List<DataDialogueNode>();
                for (int j = 0; j < CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataDialogueNodes.Count; j++)
                {
                    _Dialogue.m_DialoguePartDatas[i].m_DataDialogueNodes.Add(new DataDialogueNode(CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataDialogueNodes[j]));
                }
                _Dialogue.m_DialoguePartDatas[i].m_DataChoieceNodes = new List<DataChoieceNode>();
                for (int j = 0; j < CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataChoieceNodes.Count; j++)
                {
                    _Dialogue.m_DialoguePartDatas[i].m_DataChoieceNodes.Add(new DataChoieceNode(CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataChoieceNodes[j]));
                }
                _Dialogue.m_DialoguePartDatas[i].m_DataEventNodes = new List<DataEventNode>();
                for (int j = 0; j < CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataEventNodes.Count; j++)
                {
                    _Dialogue.m_DialoguePartDatas[i].m_DataEventNodes.Add(new DataEventNode(CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[i].m_DataEventNodes[j]));
                }
            }





        }
        private void SaveGraghData(ref MMDialogue_Gragh _Dialogue)
        {

            _Dialogue.m_DialoguePartGraghDatas = new List<DialoguePartGraghData>();
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas.Count; i++)
            {
                _Dialogue.m_DialoguePartGraghDatas.Add(new DialoguePartGraghData());
                _Dialogue.m_DialoguePartGraghDatas[i].m_GraghStartNode = new GraghStartNode(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghStartNode, this);

                _Dialogue.m_DialoguePartGraghDatas[i].m_GraghDialogueNodes = new List<GraghDialogueNode>();
                for (int j = 0; j < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghDialogueNodes.Count; j++)
                {
                    _Dialogue.m_DialoguePartGraghDatas[i].m_GraghDialogueNodes.Add(new GraghDialogueNode(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghDialogueNodes[j], this));
                }

                _Dialogue.m_DialoguePartGraghDatas[i].m_GraghChoiceNodes = new List<GraghChoiceNode>();
                for (int j = 0; j < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghChoiceNodes.Count; j++)
                {
                    _Dialogue.m_DialoguePartGraghDatas[i].m_GraghChoiceNodes.Add(new GraghChoiceNode(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghChoiceNodes[j], this));
                }

                _Dialogue.m_DialoguePartGraghDatas[i].m_GraghEventNodes = new List<GraghEventNode>();
                for (int j = 0; j < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghEventNodes.Count; j++)
                {
                    _Dialogue.m_DialoguePartGraghDatas[i].m_GraghEventNodes.Add(new GraghEventNode(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_GraghEventNodes[j], this));
                }

                _Dialogue.m_DialoguePartGraghDatas[i].m_Lines = new List<ConnectionLine>();
                for (int j = 0; j < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_Lines.Count; j++)
                {
                    _Dialogue.m_DialoguePartGraghDatas[i].m_Lines.Add(new ConnectionLine(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_Lines[j], this));
                }

                _Dialogue.m_DialoguePartGraghDatas[i].m_Points = new List<ConnectionPoint>();
                for (int j = 0; j < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_Points.Count; j++)
                {
                    _Dialogue.m_DialoguePartGraghDatas[i].m_Points.Add(new ConnectionPoint(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[i].m_Points[j], this));
                }
            }








        }
        private void DrawConnectionLines()
        {
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines.Count; i++)
            {
                CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines[i].Draw();
            }
        }
        #region
        private void DrawNodes()
        {
            

            DrawStartNode(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghStartNode);


            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghDialogueNodes.Count; i++)
            {

                DrawDialogueNode(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghDialogueNodes[i]);

            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghChoiceNodes.Count; i++)
            {

                DrawChioceNode(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghChoiceNodes[i]);

            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghEventNodes.Count; i++)
            {

                DrawEventNode(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghEventNodes[i]);

            }

        }
        private void DrawDialogueNode(MyGraghNode myNode)
        {
         
            EditorGUI.DrawRect(myNode.m_Rect, new Color(84 / 255, 84 / 255, 84 / 255, 1));
                                                                                    
            #region 

            EditorGUI.DrawRect(myNode.m_TitleRect, Color.white);
            GUI.Label(myNode.m_TitleRect, "MM_DialogueNode", MyGUIStyle.NodeTitleStyle);
            #endregion




            //text
            #region
            Rect tempRect = new Rect(myNode.m_Rect.x, myNode.m_Rect.y + 50, myNode.m_Rect.width, myNode.m_Rect.height - 20 + (CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Words.Count - 1) * 55);
            EditorGUI.DrawRect(new Rect(myNode.m_Rect.x, myNode.m_Rect.y + 21, myNode.m_Rect.width, myNode.m_Rect.height - 21), new Color(1, 1, 0.5f));


            GUI.Label(new Rect(myNode.m_Rect.x + 5, myNode.m_Rect.y + 25, 50, 20), "Name:", MyGUIStyle.ReadOnlyParam);
            CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).ChrName = EditorGUI.TextField(new Rect(myNode.m_Rect.x + 60, myNode.m_Rect.y + 25, 175, 20), CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).ChrName, MyGUIStyle.NodeTextFieldStyle);
            CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).SpeakPos= (SpeakPos)EditorGUI.EnumPopup(new Rect(myNode.m_Rect.x + 240, myNode.m_Rect.y + 25, 30, 20), CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).SpeakPos);
            for (int i = 0; i < CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Words.Count; i++)
            {

                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).ChrImgs[i] = (Sprite)EditorGUI.ObjectField(new Rect(tempRect.x + 5, tempRect.y + 55 * i, 50, 50), CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).ChrImgs[i], typeof(Sprite), false);


                if (m_EditMode)
                {
                    EditShowContent(CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Words[i],out string result);
                    EditorGUI.LabelField(new Rect(tempRect.x + 60, tempRect.y + 55 * i, 175, 50), result, MyGUIStyle.NodeEditTextStyle);
                }
                else
                {

                    CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Words[i] = EditorGUI.TextArea(new Rect(tempRect.x + 60, tempRect.y + 55 * i, 175, 50), CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Words[i], MyGUIStyle.NodeTextAreaStyle);
                }


                if (GUI.Button(new Rect(tempRect.x+240,tempRect.y+55*i,tempRect.width-245,20),"RTF"))
                {
                    m_DialogueSpeedEditor = GetWindowWithRect<MMDialogueRtfEditor>(new Rect(Event.current.mousePosition,new Vector2(600,540)),false, "MMDialogueRtfEditor");
                    m_DialogueSpeedEditor.SetNode(CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID),i);
                }

                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Voices[i] = (AudioClip)EditorGUI.ObjectField(new Rect(tempRect.x + 240, tempRect.y + 55 * i+30, tempRect.width - 245, 20), CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Voices[i], typeof(AudioClip), false);
            }



    
            #region
            tempRect = new Rect(myNode.m_Rect.x + 5, myNode.m_Rect.y + myNode.m_Rect.height - 22, myNode.m_Rect.width / 2 - 10, 20);
            if (GUI.Button(tempRect, "+"))
            {
                tempRect = new Rect(myNode.m_Rect.x, myNode.m_Rect.y, myNode.m_Rect.width, myNode.m_Rect.height + 55);
                myNode.m_Rect = tempRect;
                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Words.Add("Content...");
                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).ChrImgs.Add(null);
                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Voices.Add(null);

                GUI.changed = true;


            }
            tempRect = new Rect(myNode.m_Rect.x + 5 + myNode.m_Rect.width / 2, myNode.m_Rect.y + myNode.m_Rect.height - 22, myNode.m_Rect.width / 2 - 10, 20);
            if (GUI.Button(tempRect, "-"))
            {
                if (CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Words.Count > 1)
                {
                    tempRect = new Rect(myNode.m_Rect.x, myNode.m_Rect.y, myNode.m_Rect.width, myNode.m_Rect.height - 55);
                    myNode.m_Rect = tempRect;
                    CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Words.RemoveAt(CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Words.Count - 1);
                    CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).ChrImgs.RemoveAt(CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).ChrImgs.Count - 1);
                    CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Voices.RemoveAt(CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataDialogueNode(myNode.DataNode.ID).Voices.Count - 1);
                    GUI.changed = true;
                }


            }
            #endregion

            #endregion

 
            #region
            DrawConnection(myNode);
            #endregion



        }
        private void DrawStartNode(MyGraghNode myNode)
        {
   
            EditorGUI.DrawRect(myNode.m_Rect, new Color(84 / 255, 84 / 255, 84 / 255, 1));
            #region 

            EditorGUI.DrawRect(myNode.m_TitleRect, Color.white);
            GUI.Label(myNode.m_TitleRect, "MM_StartNode", MyGUIStyle.NodeTitleStyle);
            #endregion

   
            #region
            DrawConnection(myNode);
            #endregion
            
            #region

            EditorGUI.DrawRect(new Rect(myNode.m_Rect.x, myNode.m_Rect.y + 21, myNode.m_Rect.width, myNode.m_Rect.height - 21), new Color(1, 0.5f, 0.5f));


  
            GUI.Label(new Rect(myNode.m_Rect.x + 5, myNode.m_Rect.y + 25, 50, 20), "Name:", MyGUIStyle.ReadOnlyParam);


            CurMMDialogue_Gragh.m_MMDialogue_Data.CharacterName=EditorGUI.TextField(new Rect(myNode.m_Rect.x + 60, myNode.m_Rect.y + 25, myNode.m_Rect.width - 65, 20), CurMMDialogue_Gragh.m_MMDialogue_Data.CharacterName);

            GUI.Label(new Rect(myNode.m_Rect.x + 5, myNode.m_Rect.y + 50, 50, 20), "Part ID:", MyGUIStyle.ReadOnlyParam);
            EditorGUI.LabelField(new Rect(myNode.m_Rect.x + 60, myNode.m_Rect.y + 50, myNode.m_Rect.width - 65, 20), m_CurtPartID.ToString(), MyGUIStyle.ReadOnlyParam);
            #endregion
        }
        private void DrawEventNode(MyGraghNode myNode)
        {

            EditorGUI.DrawRect(myNode.m_Rect, new Color(84 / 255, 84 / 255, 84 / 255, 1));
                                                                                       
            #region 

            EditorGUI.DrawRect(myNode.m_TitleRect, Color.white);
            GUI.Label(myNode.m_TitleRect, "MM_EventNode", MyGUIStyle.NodeTitleStyle);
            #endregion

 
            #region
            DrawConnection(myNode);
            #endregion

            #region

            EditorGUI.DrawRect(new Rect(myNode.m_Rect.x, myNode.m_Rect.y + 21, myNode.m_Rect.width, myNode.m_Rect.height - 21), new Color(0.5f, 1, 1));



            //GUI.Label(new Rect(myNode.m_Rect.x + 5, myNode.m_Rect.y + 25, 40, 20), "Type", MyGUIStyle.ReadOnlyParam);
            //EditorGUI.EnumPopup(new Rect(myNode.m_Rect.x + 5, myNode.m_Rect.y + 25, myNode.m_Rect.width - 10, 20), CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataEventNode(myNode.DataNode.ID).m_Event.m_GameEventType);
            CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataEventNode(myNode.DataNode.ID).EventKey = EditorGUI.TextField(new Rect(myNode.m_Rect.x + 5, myNode.m_Rect.y + 25, myNode.m_Rect.width - 10, 20), CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataEventNode(myNode.DataNode.ID).EventKey);
            //if (GUI.Button(new Rect(myNode.m_Rect.x + 5, myNode.m_Rect.y + 50, myNode.m_Rect.width - 10, 20), ">>"))
            //{
            //    OpenEventNodeDetailsWindow(CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataEventNode(myNode.DataNode.ID));
            //}
            #endregion
        }
        private void DrawChioceNode(MyGraghNode myNode)
        {

            EditorGUI.DrawRect(myNode.m_Rect, new Color(84 / 255, 84 / 255, 84 / 255, 1));
                                                                                
            #region 

            EditorGUI.DrawRect(myNode.m_TitleRect, Color.white);
            GUI.Label(myNode.m_TitleRect, "MM_ChoiceNode", MyGUIStyle.NodeTitleStyle);
            #endregion

    

            Rect tempRect = new Rect(myNode.m_Rect.x, myNode.m_Rect.y + 25, myNode.m_Rect.width, myNode.m_Rect.height - 20 + (CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(myNode.DataNode.ID).Choices().Count - 1) * 25);
            EditorGUI.DrawRect(new Rect(myNode.m_Rect.x, myNode.m_Rect.y + 21, myNode.m_Rect.width, myNode.m_Rect.height - 21), new Color(0.5f, 1, 0.5f));
            for (int i = 0; i < CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(myNode.DataNode.ID).Choices().Count; i++)
            {




                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(myNode.DataNode.ID).Choices()[i] = EditorGUI.TextField(new Rect(tempRect.x + 5, tempRect.y + 25 * i, tempRect.width - 30, 20), CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(myNode.DataNode.ID).Choices()[i], MyGUIStyle.NodeTextFieldStyle);
            }

            #region
            tempRect = new Rect(myNode.m_Rect.x + 5, myNode.m_Rect.y + myNode.m_Rect.height - 22, myNode.m_Rect.width / 2 - 10, 20);
            if (GUI.Button(tempRect, "+"))
            {
                tempRect = new Rect(myNode.m_Rect.x, myNode.m_Rect.y, myNode.m_Rect.width, myNode.m_Rect.height + 25);
                myNode.m_Rect = tempRect;
                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(myNode.DataNode.ID).Choices().Add("ABCD");
                CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(myNode.DataNode.ID).NextNodes().Add(new NodeID_Type(-1, NODETYPE.START));
                ConnectionPoint point = new ConnectionPoint(myNode, ConnectionPointType.Out, myNode.m_OutPointsID.Count,this);
                myNode.m_OutPointsID.Add(point.m_PointID);

                GUI.changed = true;

            }
            tempRect = new Rect(myNode.m_Rect.x + 5 + myNode.m_Rect.width / 2, myNode.m_Rect.y + myNode.m_Rect.height - 22, myNode.m_Rect.width / 2 - 10, 20);
            if (GUI.Button(tempRect, "-"))
            {
                if (CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(myNode.DataNode.ID).Choices().Count > 2)
                {
                    tempRect = new Rect(myNode.m_Rect.x, myNode.m_Rect.y, myNode.m_Rect.width, myNode.m_Rect.height - 25);
                    myNode.m_Rect = tempRect;

        
                    int tempPointID = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghChoiceNodes[myNode.DataNode.ID].m_OutPointsID[(CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(myNode.DataNode.ID)).Choices().Count - 1];
                    ConnectionLine tempConnectionLine = FindLineByStartPoint(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Points[tempPointID]);
                    if (tempConnectionLine != null)
                    {
                        CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines.Remove(tempConnectionLine);
                    }

                    CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(myNode.DataNode.ID).Choices().RemoveAt
                        ((CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(myNode.DataNode.ID)).Choices().Count - 1);

                    CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(myNode.DataNode.ID).NextNodes().RemoveAt((CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].GetDataChoiceNode(myNode.DataNode.ID)).Choices().Count - 1);
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].RemovePoint(myNode.m_OutPointsID[myNode.m_OutPointsID.Count - 1]);
                    myNode.m_OutPointsID.RemoveAt(myNode.m_OutPointsID.Count - 1);
                    GUI.changed = true;
                }

 
            }
            #endregion





            #region
            DrawConnection(myNode);
            #endregion
        }
        //private void OpenEventNodeDetailsWindow(DataEventNode dataEventNode)
        //{
        //    m_EventNodeDetailsWindow= GetWindow<EventNodeDetailsWindow>(false, "EventNodeDetailsWindow");
        //    m_EventNodeDetailsWindow.maxSize=new Vector2(300,600);
        //    m_EventNodeDetailsWindow.minSize = new Vector2(300, 600);
        //    m_EventNodeDetailsWindow.SetTarget(dataEventNode);
        //}
        private ConnectionLine FindLineByStartPoint(ConnectionPoint startPoint)
        {
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines.Count; i++)
            {
                if (startPoint == CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines[i].m_StartPoint)
                {
                    return CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines[i];
                }
            }
            return null;
        }
        private void DrawConnection(MyGraghNode myNode)
        {

            switch (myNode.DataNode.m_NODETYPE)

            {
                case NODETYPE.START:

                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_OutPointsID[0]).m_Rect = (myNode as GraghStartNode).m_OutRect;
                    if (GUI.Button((myNode as GraghStartNode).m_OutRect, "", MyGUIStyle.StylePointOut))
                    {
                        if (CurSelectPoint == null)
                        {
                            CurSelectPoint = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_OutPointsID[0]);

                        }
                        else
                        {
                            if (CurSelectPoint.m_ConnectionPointType == ConnectionPointType.In)
                            {
                                if (IsHasOutLine(myNode.DataNode, 0) == false && IsHasInLine(CurSelectPoint.Owner.DataNode, 0) == false)
                                {

                                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines.Add(new ConnectionLine(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_OutPointsID[0]).m_PointID, CurSelectPoint.m_PointID,this));
                                }

                            }
                            CurSelectPoint = null;
                        }
                        GUI.changed = true;

                    }
                    break;
                case NODETYPE.DIALOGUE:
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_InPointsID[0]).m_Rect = (myNode as GraghDialogueNode).m_InRect;
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_OutPointsID[0]).m_Rect = (myNode as GraghDialogueNode).m_OutRect;
                    if (GUI.Button((myNode as GraghDialogueNode).m_OutRect, "", MyGUIStyle.StylePointOut))
                    {
                        if (CurSelectPoint == null)
                        {
                            CurSelectPoint = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_OutPointsID[0]);
                        }
                        else
                        {
                            if (CurSelectPoint.m_ConnectionPointType == ConnectionPointType.In)
                            {
                                if (IsHasOutLine(myNode.DataNode, 0) == false && IsHasInLine(CurSelectPoint.Owner.DataNode, CurSelectPoint.m_NodePointNo) == false)
                                {

                                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines.Add(new ConnectionLine(myNode.m_OutPointsID[0], CurSelectPoint.m_PointID,this));
                                }

                            }
                            CurSelectPoint = null;
                        }
                        GUI.changed = true;
                    }
                    if (GUI.Button((myNode as GraghDialogueNode).m_InRect, "", MyGUIStyle.StylePointIn))
                    {
                        if (CurSelectPoint == null)
                        {
                            CurSelectPoint = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_InPointsID[0]);
                        }
                        else
                        {
                            if (CurSelectPoint.m_ConnectionPointType == ConnectionPointType.Out)
                            {
                                if (IsHasInLine(myNode.DataNode, 0) == false && IsHasOutLine(CurSelectPoint.Owner.DataNode, CurSelectPoint.m_NodePointNo) == false)
                                {

                                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines.Add(new ConnectionLine(CurSelectPoint.m_PointID, CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_InPointsID[0]).m_PointID,this));
                                }

                            }
                            CurSelectPoint = null;
                        }
                        GUI.changed = true;
                    }
                    break;
                case NODETYPE.CHOICE:
                  
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_InPointsID[0]).m_Rect = (myNode as GraghChoiceNode).m_InRect;

                    for (int i = 0; i < (myNode as GraghChoiceNode).m_OutPointsID.Count; i++)
                    {
                        CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_OutPointsID[i]).m_Rect = (myNode as GraghChoiceNode).GetOutRect(i);
                        if (GUI.Button(CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint((myNode as GraghChoiceNode).m_OutPointsID[i]).m_Rect, "", MyGUIStyle.StylePointOut))
                        {

                            if (CurSelectPoint == null)
                            {
                                CurSelectPoint = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_OutPointsID[i]); 
                            }
                            else
                            {
                                if (CurSelectPoint.m_ConnectionPointType == ConnectionPointType.In)
                                {
                                    if (IsHasOutLine(myNode.DataNode, i) == false && IsHasInLine(CurSelectPoint.Owner.DataNode, CurSelectPoint.m_NodePointNo) == false)
                                    {

                                        CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines.Add(new ConnectionLine(myNode.m_OutPointsID[i], CurSelectPoint.m_PointID,this));
                                    }

                                }
                                CurSelectPoint = null;
                            }
                            GUI.changed = true;
                        }
                    }
                    if (GUI.Button((myNode as GraghChoiceNode).m_InRect, "", MyGUIStyle.StylePointIn))
                    {
                        if (CurSelectPoint == null)
                        {
                            CurSelectPoint = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_InPointsID[0]);
                        }
                        else
                        {
                            if (CurSelectPoint.m_ConnectionPointType == ConnectionPointType.Out)
                            {
                                if (IsHasInLine(myNode.DataNode, 0) == false && IsHasOutLine(CurSelectPoint.Owner.DataNode, CurSelectPoint.m_NodePointNo) == false)
                                {

                                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines.Add(new ConnectionLine(CurSelectPoint.m_PointID, CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_InPointsID[0]).m_PointID,this));
                                }

                            }
                            CurSelectPoint = null;
                        }
                        GUI.changed = true;
                    }

                    break;
                case NODETYPE.EVENT:
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_InPointsID[0]).m_Rect = (myNode as GraghEventNode).m_InRect;
                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_OutPointsID[0]).m_Rect = (myNode as GraghEventNode).m_OutRect;
                    if (GUI.Button((myNode as GraghEventNode).m_OutRect, "", MyGUIStyle.StylePointOut))
                    {
                        if (CurSelectPoint == null)
                        {
                            CurSelectPoint = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_OutPointsID[0]);
                        }
                        else
                        {
                            if (CurSelectPoint.m_ConnectionPointType == ConnectionPointType.In)
                            {
                                if (IsHasOutLine(myNode.DataNode, 0) == false && IsHasInLine(CurSelectPoint.Owner.DataNode, CurSelectPoint.m_NodePointNo) == false)
                                {

                                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines.Add(new ConnectionLine(myNode.m_OutPointsID[0], CurSelectPoint.m_PointID,this));
                                }

                            }
                            CurSelectPoint = null;
                        }
                        GUI.changed = true;
                    }
                    if (GUI.Button((myNode as GraghEventNode).m_InRect, "", MyGUIStyle.StylePointIn))
                    {
                        if (CurSelectPoint == null)
                        {
                            CurSelectPoint = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_InPointsID[0]);
                        }
                        else
                        {
                            if (CurSelectPoint.m_ConnectionPointType == ConnectionPointType.Out)
                            {
                                if (IsHasInLine(myNode.DataNode, 0) == false && IsHasOutLine(CurSelectPoint.Owner.DataNode, CurSelectPoint.m_NodePointNo) == false)
                                {

                                    CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_Lines.Add(new ConnectionLine(CurSelectPoint.m_PointID, CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].GetPoint(myNode.m_InPointsID[0]).m_PointID,this));
                                }

                            }
                            CurSelectPoint = null;
                        }
                        GUI.changed = true;
                    }
                    break;
                default:
                    break;
            }



        }
        private bool IsHasOutLine(NodeID_Type _NodeID_Type, int _PointNo)
        {
     
            switch (_NodeID_Type.m_NODETYPE)

            {
                case NODETYPE.START:
                    if (CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].m_StartNode.NextNodes()[_PointNo].ID >= 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case NODETYPE.DIALOGUE:
                    if (CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].m_DataDialogueNodes[_NodeID_Type.ID].NextNodes()[_PointNo].ID >= 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case NODETYPE.CHOICE:
                    if (CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].m_DataChoieceNodes[_NodeID_Type.ID].NextNodes()[_PointNo].ID >= 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case NODETYPE.EVENT:
                    return false;

                default:
                    return false;
            }
        }
        private bool IsHasInLine(NodeID_Type _NodeID_Type, int _PointNo)
        {


            if (CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].m_StartNode.NextNodes()[0].m_NODETYPE == _NodeID_Type.m_NODETYPE && CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].m_StartNode.NextNodes()[0].ID == _NodeID_Type.ID)
            {
                return true;
            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].m_DataDialogueNodes.Count; i++)
            {
                if (CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].m_DataDialogueNodes[i].NextNodes()[0].m_NODETYPE == _NodeID_Type.m_NODETYPE && CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].m_DataDialogueNodes[i].NextNodes()[0].ID == _NodeID_Type.ID)
                {
                    return true;
                }
            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].m_DataChoieceNodes.Count; i++)
            {


                if (CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].m_DataChoieceNodes[i].NextNodes()[_PointNo].m_NODETYPE == _NodeID_Type.m_NODETYPE && CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_CurtPartID].m_DataChoieceNodes[i].NextNodes()[_PointNo].ID == _NodeID_Type.ID)
                {
                    return true;
                }

            }



            return false;
        }
        #endregion
        private void DrawPendingLine(Event e)
        {
            if (CurSelectPoint != null)
            {
 
                Vector3 startPosition;
                Vector3 endPosition;
                if (CurSelectPoint.m_ConnectionPointType == ConnectionPointType.Out)
                {
                    startPosition = CurSelectPoint.m_Rect.center;
                    endPosition = e.mousePosition;
                }
                else
                {
                    startPosition = e.mousePosition;
                    endPosition = CurSelectPoint.m_Rect.center;
                }
                Handles.DrawBezier(   
                startPosition,
                endPosition,
                startPosition - Vector3.left * 50f,
                endPosition + Vector3.left * 50f,   
                Color.green,        
                null,               
                4f               
                );

                GUI.changed = true;

            }
        }
        private void ProcessNodesEvent()
        {
            bool tempChanged = false;

            tempChanged = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghStartNode.ProcessEvents(Event.current);
            if (tempChanged)
            {
                GUI.changed = true;
            }

            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghDialogueNodes.Count; i++)
            {
                tempChanged = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghDialogueNodes[i].ProcessEvents(Event.current);
                if (tempChanged)
                {
                    GUI.changed = true;
                }
            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghChoiceNodes.Count; i++)
            {
                tempChanged = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghChoiceNodes[i].ProcessEvents(Event.current);
                if (tempChanged)
                {
                    GUI.changed = true;
                }
            }
            for (int i = 0; i < CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghEventNodes.Count; i++)
            {
                tempChanged = CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_CurtPartID].m_GraghEventNodes[i].ProcessEvents(Event.current);
                if (tempChanged)
                {
                    GUI.changed = true;
                }
            }
        }
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            {
                Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
                Vector3 gridOffset = new Vector3(Gm_ridOffset.x % gridSpacing, Gm_ridOffset.y % gridSpacing, 0);
                for (int i = 0; i < widthDivs; i++)
                {
                    Handles.DrawLine(
                        new Vector3(gridSpacing * i, 0 - gridSpacing, 0) + gridOffset,                  //起点
                        new Vector3(gridSpacing * i, position.height + gridSpacing, 0f) + gridOffset);  //终点
                }
                //绘制所有的横线
                for (int j = 0; j < heightDivs; j++)
                {
                    Handles.DrawLine(
                        new Vector3(0 - gridSpacing, gridSpacing * j, 0) + gridOffset,                  //起点
                        new Vector3(position.width + gridSpacing, gridSpacing * j, 0f) + gridOffset);   //终点
                }

                //重设颜色
                Handles.color = Color.white;
            }
            Handles.EndGUI(); //结束一个 2D GUI 块并返回到 3D Handle GUI。

        }
    }





}

