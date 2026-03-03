using TestTask.NonEditable;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
            if (CurrentMonster != null)
            {
                // monsterInfoText.text = $"Name: {CurrentMonster.MonsterName}\nType: {CurrentMonster.MonsterType}\nHealth: {CurrentMonster.MonsterCurrentHealth}/{CurrentMonster.MonsterMaxHealth}";
                monsterInfoText.text = $"Name: {CurrentMonster.MonsterName}";
                if (monsterHealthBar != null)
                {
                    monsterHealthBar.gameObject.SetActive(true);
                    monsterHealthBar.maxValue = CurrentMonster.MonsterMaxHealth;
                    monsterHealthBar.value = CurrentMonster.MonsterCurrentHealth;

                    if(CurrentMonster.MonsterCurrentHealth <= 0)
                    {
                        monsterHealthBar.gameObject.SetActive(false); // Hide health bar if monster is dead
                        // Spawn new Monster
                        if(ServerPacketsHandler.ClientLoginResponse == LoginResponse.Success)
                        {
                            ClientManager.Instance.ClientMobsManager.CreateNewMonster(ServerMock.Instance.ServerMobsManager.MonsterData);
                        }
                        else
                        {
                            Debug.Log("Cannot spawn new monster. Client is not logged in. ");
                            ActiveMonsterSprite = monsterSprites[0]; // Set to default sprite if client is not logged in
                            monsterImage.sprite = ActiveMonsterSprite; // Update the monster image to the default sprite
                            // monsterHealthBar.value = 0; // Set health bar to 0 if client is not logged in
                        }
                    }
                }

                // Lookup Sprite name based on MonsterType and assign it to ActiveMonsterSprite
                ActiveMonsterSprite = monsterSprites.Find(sprite => sprite.name == CurrentMonster.MonsterType.ToString());

                if (ActiveMonsterSprite == null && monsterSprites.Count > 0)
                {
                    ActiveMonsterSprite = monsterSprites[0]; // Fallback to the first sprite if no match is found
                }
                
                monsterImage.sprite = ActiveMonsterSprite; // Set the monster image to the active sprite
                monsterImage.color = Color.white; // Ensure the image is visible (in case it was hidden before)

                // ActiveMonsterSprite = monsterSprites[(int)CurrentMonster.MonsterType % monsterSprites.Count]; // Example of how to select a sprite based on monster type
                 }
            else
            {
                monsterInfoText.text = "No monster spawned.";
                monsterHealthBar.value = 0;
            }
        }

        public void CreateNewMonster(MonsterData monsterData) 
        {
            // Instruction: The Client Side should receive the monster data from the server.
            CurrentMonster = monsterData;   
            // Instruction: The Client Side should Create/Update its local representation of the monster.
            UpdateMonsterData();    
        }        

        // Instruction:  an action (a button in the scene) should: Trigger dealing damage to the current monster. 
        public void DamageMonster(float damage) 
        {
            if (CurrentMonster != null)
            {
                CurrentMonster.TakeDamage(damage);
                Debug.Log($"Dealt {damage} damage to {CurrentMonster.MonsterName}. Current Health: {CurrentMonster.MonsterCurrentHealth}/{CurrentMonster.MonsterMaxHealth}");
                // Instruction: an action (a button in the scene) should: Inform the server about which monster was hit. 
                UpdateMonsterData();
            }
        }
    }
}
