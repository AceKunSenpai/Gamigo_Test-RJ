using System.Diagnostics;
using TestTask.NonEditable;
using UnityEngine;

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
            ServerMock.Instance.TryConnectionClient(out var clientID);
            SendLoginResponse(clientLogInResponse, clientId);

            Debug.Log("Received login request from client. Response: " +clientLoginResponse + " Client ID " +clientID);

            // Additional Logic for successful login can be added here, e.g. Initializing client data, sending initial game state, etc.
            if(clientLogInResponse == LoginResponse.Success)
            {
                // Instruction: The Server Side should Spawn a Monster (with some ID, type, max HP, and current HP).
                var data = ServerMock.Instance.ServerMobsManager.MonsterData;

                //Instruction: The Server Side should Inform the client about this monster via packet.
                
            }
            else
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
    }
}

public enum LoginResponse
{
    Success = 0,
    Failure = 1,
}