using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirMirror;

    namespace MirMirrorEditor
    {
        [System.Serializable]
        public class GraghStartNode : MyGraghNode
        {


            public Rect m_OutRect { get { return new Rect(m_Rect.x + m_Rect.width - 20, m_Rect.y, 20, 20); } }
 
            public GraghStartNode(Vector2 startPos, Vector2 size, NodeID_Type _NodeID_Type, MM_DialogueNodeEditor _Editor) : base(startPos, size, _NodeID_Type,_Editor)
            {

                m_InPointsID = new List<int>();
                m_OutPointsID = new List<int>();
                ConnectionPoint tempPoint = new ConnectionPoint(this, ConnectionPointType.Out, 0,_Editor);
                m_OutPointsID.Add(tempPoint.m_PointID);

                m_Editor.CurMMDialogue_Gragh.m_DialoguePartGraghDatas[m_Editor.m_CurtPartID].m_GraghStartNode = this;
                m_Editor.CurMMDialogue_Gragh.m_MMDialogue_Data.m_DialoguePartDatas[m_Editor.m_CurtPartID].m_StartNode = new DataStartNode();
            }
            public GraghStartNode(GraghStartNode _GraghStartNode, MM_DialogueNodeEditor _Editor) : base(_GraghStartNode,_Editor)
            {

            }
            public NodeID_Type GetDataNode()
            {
                return m_NodeID_Type;
            }
        }

    }
