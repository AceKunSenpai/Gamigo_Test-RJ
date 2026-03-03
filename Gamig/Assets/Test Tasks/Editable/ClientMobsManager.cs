using System.Threading.Tasks.Dataflow;
using TestTask.NonEditable;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TestTask.Editable
{
    public class ClientMobsManager : MonoBehaviour
    {
        ServerMobsManager serverMobsManager;
        public MonsterData CurrentMonster { get; private set;}

        // Prefab for the monster (assign in inspector)
        [SerializeField] private GameObject monsterPrefab;
        // Parent transform for the monster (assign in Inspector)
        [SerializeField] private Transform MonsterContainer;
        // UI Text to display monster info (assign in inspector)
        [SerializeField] TextMeshProUGUI monsterInfoText;
        // UI Slider to display monster health (assign in inspector)
        [SerializeField] Sprite ActiveMonsterSprite; // Assigned Sprite
        [SerializeField] private UnityEngine.UI.Slider monsterHealthBar; 
        // UI Image to display the monster sprite (assign in inspector)
        [SerializeField] Image monsterImage; 
        // List of sprites for different monster types (assign in inspector)
        [SerializeField] List<Sprite> monsterSprites; 

        private void Start() 
        {
            // Get the ServerMobsManager instance from the ServerMock
            serverMobsManager = ServerMock.Instance.ServerMobsManager; 
            // Initialize the ClientLoginResponse to undefined
            ServerPacketsHandler.ClientLoginResponse = LoginResponse.undefined; 
        }

        public void SpawnMob(string mobType)
        {
            serverMobsManager.SpawnMonster();
        }

        // Instruction: The Client Side should Update the UI/visuals to show the monster and its health.
        public void UpdateMonsterData()
        {
            
        }
    }
}
