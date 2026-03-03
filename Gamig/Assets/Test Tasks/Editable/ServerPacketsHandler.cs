using TestTask.NonEditable;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TestTask.Editable
{
    public static class ServerPacketsHandler
    {

        public static LoginResponse ClientLoginResponse {get; set;}
        public static int RelogAttemptsMax = 3, RelogAttempts;
        public static bool RelogSuccess = false;

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
            // Login Response Failure
            else if(clientLogInResponse == LoginResponse.Failure)
            {
                RelogSuccess = false;
                RelogAttempts = RelogAttemptsMax;
                // Attempt Relog-in
                CoroutineRunner.Instance.StartCoroutine(ReloginCoroutine());
            }
            else // clientLogInResponse == LoginResponse.undefined
            {
                
            }
        }

        public static bool ReLoginRequest(Packet packet)
        {
            bool LoginSuccess = false;
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

                LoginSuccess=true;
            }

            return LoginSuccess;
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

        public static IEnumerator ReloginCoroutine()
        {
            while(RelogAttempts>0)
            {
                Packet packet = new Packet(1);
                RelogSuccess = ReLoginRequest(packet);
                RelogAttempts--;
                if(!RelogSuccess)
                {
                    Debug.Log("Relog Failure, try again");
                    // Paused for 3 seconds
                    yield return new WaitForSeconds(3);
                }
                else
                {
                    Debug.Log("Relog Success");
                    yield return null;
                }
            }
            Debug.Log("Relog Attempts Exhausted");
            yield return null;
        }
    }
}

public enum LoginResponse
{
    Success = 0,
    Failure = 1,
    undefined = 2,
}

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                // Create a hidden GameObject to run coroutines
                GameObject runnerObj = new GameObject("CoroutineRunner");
                _instance = runnerObj.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(runnerObj);
            }
            return _instance;
        }
    }

    // Public method to start coroutines from anywhere
    public static Coroutine Run(IEnumerator routine)
    {
        return Instance.StartCoroutine(routine);
    }
}
