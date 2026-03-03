using System.Collections.Generic;
using UnityEngine;

namespace TestTask.Editable
{
    public class ClientColors : MonoBehaviour
    {
        public int RequestHowManyColors;
        public List<Color> colors;
        public GameObject colorButtonPrefab; // Prefab for the color button (assign in inspector)
        public Transform buttonContainer; // Parent transform for the buttons (assign in inspector)
    

        void Start()
        {
            
        }

        // Instruction: The Client Side should be able to request a set of colors from the server (via a UI button).
        public void CreateColorButtons() 
        {
            if(ServerPacketsHandler.ClientLoginResponse == LoginResponse.Success)
            {
                ClearColorButtons(); // Clear existing buttons before creating new ones
                if(colors.Count<=0)
                {
                    colors = RequestColors(RequestHowManyColors);
                }
                
                Debug.Log("Requested "+colors.Count+" colors");

                foreach (Color color in colors)
                {
                    // Create a button and set its color to the current color in the list
                    GameObject button = Instantiate(colorButtonPrefab); // Instantiate a button from the prefab
                    button.transform.SetParent(buttonContainer, false); // Set the parent to the button container
                    button.GetComponent<UnityEngine.UI.Image>().color = new Color(color.r, color.g, color.b, color.a); // Set the button's color
                }
                
            }
            else
            {
                Debug.Log("Cannot create color buttons. Client is not logged in.");
                ClearColorButtons(); // Clear any existing buttons if client is not logged in
            }
        }

        public void ClearColorButtons()
        {
            // Example of how to clear all color buttons from the container
            foreach (Transform child in buttonContainer)
            {
                Destroy(child.gameObject); // Destroy each button game object
            }
        }

        // Instruction: The Client Side should be able to request a set of colors from the server (via a UI button).
        public List<Color> RequestColors(int howManyColors)
        {
            return ServerPacketsHandler.GetColorsFromServer(howManyColors); 
        }
    }
}
