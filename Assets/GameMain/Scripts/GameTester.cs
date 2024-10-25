using System;
using System.Collections;
using Framework;
using Framework.Develop;
using JSAM;
using MirMirror;
using Services.Asset;
using Services.SceneManagement;
using UnityEngine;

namespace GameMain
{
    [DefaultExecutionOrder(-1000)]
    public class GameTester : MonoBehaviour
    {
        [SerializeField] public MMDialogue_Data _dialogueData;
        public AudioLibrary L1, L2;

        private static bool _initAudio = false;

        private void Awake()
        {
            if (FindObjectsByType<GameTester>(FindObjectsSortMode.None).Length > 1)
                Destroy(gameObject);
            Debug.Log($"init state:{_initAudio}");
            if (!_initAudio)
            {
                _initAudio = true;
                AudioManager.InternalInstance.LoadAudioLibrary(L1);
                AudioManager.InternalInstance.LoadAudioLibrary(L2);
            }

            DontDestroyOnLoad(gameObject);
            if (GameEntry.Resource is null)
            {
                GameEntry.Resource = GetComponent<AssetLoader>();
            }
            else
            {
                GetComponent<AssetLoader>().enabled = false;
            }

            if (GameEntry.Event is null)
            {
                GameEntry.Event = GetComponent<ClassEventSystem>();
            }
            else
            {
                GetComponent<ClassEventSystem>().enabled = false;
            }

            if (GameEntry.UI is null)
            {
                GameEntry.UI = GetComponentInChildren<UIManager>();
                (GameEntry.UI as UIManager).InitImmediately();
            }
            else
            {
                GetComponentInChildren<UIManager>().enabled = false;
                transform.Find("UI").gameObject.SetActive(false);
            }

            if (GameEntry.Scene is null)
            {
                GameEntry.Scene = GetComponent<SceneControllerBase>();
            }

            if (GameEntry.Procedure is null)
            {
                GameEntry.Procedure = gameObject.AddComponent<ProcedureManager>();
                (GameEntry.Procedure as ProcedureManager).AddProcedure<ProcedureMain>();
                (GameEntry.Procedure as ProcedureManager).AddProcedure<ProcedureMenu>();
                (GameEntry.Procedure as ProcedureManager).AddProcedure<ProcedureChangeScene>();
                (GameEntry.Procedure as ProcedureManager).SetStartProcedure<ProcedureMain>();

                (GameEntry.Procedure as ProcedureManager).SetValue("Level", 1);
                (GameEntry.Procedure as ProcedureManager).SetValue("PlayerSpawnPoint", 1);
                (GameEntry.Procedure as ProcedureManager).StartImmediately();
            }
        }

        private void Start()
        {
            // AGameManager.Instance.OnEnter();
        }

        private void Update()
        {
        }
    }
}