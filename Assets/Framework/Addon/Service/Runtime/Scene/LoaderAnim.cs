using System;
using Framework;
using Framework.Develop;
using UnityEngine;

namespace Services.SceneManagement
{
    public class LoaderAnim : MonoBehaviour
    {
        public void OnLoaderAnimStartEnd()
        {
            GameEntry.Event.Fire(this,OnLoaderAnimStartArg.Create());
        }
    }
}