using TestTask.NonEditable;
using UnityEngine;
using System.Collections.Generic;

namespace TestTask.Editable
{
    public static class ServerPacketsHandler
    {

        public static LoginResponse ClientLoginResponse {get; set;}

        #region Packet Handlers
        public static void LoginRequest(Packet packet)
        {
            var clientLogInResponse = ServerMock.Instance.TryConnectClient(out var clientId);
            ClientLoginResponse = clientLogInResponse;
            ServerMock.Instance.TryConnectClient(out var clientID);
            SendLoginResponse(clientLogInResponse, clientId);

            Debug.Log("Received login request from client. Response: " + clientLogInResponse + " Client ID " +clientID);

            // Additional Logic for successful login can be added here, e.g. Initializing client data, sending initial game state, etc.
            if(clientLogInResponse == LoginResponse.Success)
            {
                // Instruction: The Server Side should Spawn a Monster (with some ID, type, max HP, and current HP).
                var data = ServerMock.Instance.ServerMobsManager.MonsterData;

                //Instruction: The Server Side should Inform the client about this monster via packet.
                ClientManager.Instance.ClientMobsManager.CreateNewMonster(data);
            }
            // Login Response Failure or undefined (default)
            else if(clientLogInResponse == LoginResponse.Failure)
            {
                // Clear Color Buttons Here
            }
            else // clientLogInResponse == LoginResponse.undefined
            {
                
            }
        }

        #endregion

        #region Packet Senders
        public static void SendLoginResponse(LoginResponse response, int clientId)
        {
            using (Packet packet = new Packet(1))
            {
                packet.Write((int)response);
                packet.Write(clientId);

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }

        #endregion

        public static List<Color> GetColorsFromServer(int howManyColors)
        {
            List<Color> returnColors = new List<Color>();
            for(int i = 0; i<howManyColors; i++)
            {
                Vector3 ColorData;
                ColorData.x = Mathf.Clamp01(Random.value); // Example of generating random color data for demonstration purposes
                ColorData.y = Mathf.Clamp01(Random.value);
                ColorData.z = Mathf.Clamp01(Random.value);
                float alpha = 1.0f; // Example alpha value, can be modified as needed
                returnColors.Add(new Color(ColorData.x, ColorData.y, ColorData.z, alpha));
            }
            
            // This method would contain logic to request color data from the server and return it as a list of Color objects.
            // For example, it could send a packet to the server requesting color data, and then wait for a response packet containing the color information, which it would then parse and return as a list of Color objects.
            return returnColors;
        }   
    }
}

public enum LoginResponse
{
    undefined = 0,
    Success = 1,
    Failure = 2,
}