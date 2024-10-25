using Framework;
using UnityEngine;

namespace Tencent
{
    public class ChangeSpawnPoint : MonoBehaviour
    {
        public int SpawnIndex;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerTrigger>(out var player))
            {
                return;
            }

            if (SpawnIndex > (GameEntry.Procedure as ProcedureManager).GetValue<int>("PlayerSpawnPoint"))
            {
                Debug.Log("set spawn point to "+SpawnIndex);
                (GameEntry.Procedure as ProcedureManager).SetValue("PlayerSpawnPoint", SpawnIndex);
            }
        }
    }
}