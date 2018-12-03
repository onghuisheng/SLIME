using UnityEngine;
using System.Collections;
using UnityEngine.PS4;

// Trivial PS4 UDPP2P example, that has 2 options, start as server and connect to server that is at a fixed supplied IP address 

public class NetworkManager : MonoBehaviour {

    public GameObject playerPrefab;
    public Transform spawnObject;
    private string gameName = "MudGrape";
    bool refreshing = false;
    float btnX;
    float btnY;
    float btnW;
    float btnH;
	bool startingServer = false;
	bool startingHost = false;
	int udpp2pPort=25001;	// what port shall we communicate through (translates to a UDPP2P vport number )
	
	public bool udpp2pNetworkingEnabled=true;

    HostData[] hostData;

    string messages;
    string serversMessage = "";

    void Start()
    {
        btnX = Screen.width * 0.05f;
        btnY = Screen.width * 0.05f;
        btnW = Screen.width * 0.2f;
        btnH = Screen.width * 0.1f;
        hostData = new HostData[] { };
    }

    void StartServer()
    {        
		startingServer = true;
		
		UnityEngine.PS4.Networking.enableUDPP2P = udpp2pNetworkingEnabled;		
		
		
        NetworkConnectionError err = Network.InitializeServer(32, udpp2pPort, false); //!Network.HavePublicAddress
		if(err == NetworkConnectionError.NoError)
		{
// no master servers for udpp2p test        	MasterServer.RegisterHost(gameName, "Tutorial Game Name", "Testing");
		}
		else
		{
			print (err);
			startingServer = false;
		}
    }
	
	float refreshTime = 0.0f;
	
    void Update()
    {
 		
        if (refreshing == true)
        {
			refreshTime -= Time.deltaTime;
			if(refreshTime <= 0.0f)
			{
				refreshing = false;
			}
			else if (MasterServer.PollHostList().Length > 0)
            {
                hostData = MasterServer.PollHostList();
                refreshing = false;
                print("Found " + MasterServer.PollHostList().Length + " Games");
            }
        }
    }

    void SpawnPlayer()
    {
        Network.Instantiate(playerPrefab, spawnObject.position, Quaternion.identity, 0);
    }

    // Messages
    void OnConnectedToServer()
    {
        SpawnPlayer();
    }

    void OnServerInitialized(NetworkPlayer player)
    {
 		Debug.Log("Server Initialized: " + player.ipAddress + ":" + player.port + ", " + Network.peerType);
		SpawnPlayer();
    }

    void OnMasterServerEvent(MasterServerEvent mse)
    {
        if (mse == MasterServerEvent.RegistrationSucceeded)
        {
            print("Registered Server");
        }
    }       

    // GUI
    void OnGUI()
    {       
        if(Network.isServer)
            serversMessage = " Connections:" + Network.connections.Length;

        {
            GUI.TextArea(new Rect(0, 0, 300, 25), Application.platform.ToString() +
                " Servers:" + MasterServer.PollHostList().Length +
                serversMessage);

            if (!Network.isClient && !Network.isServer)
            {
                if (refreshing == true)
                {
					int intTime = System.Convert.ToInt32(refreshTime);
                    GUI.TextArea(new Rect(0, 26, 300, 25), " Refreshing... " + intTime);
                }
				
				if (startingServer == false && refreshing == false)
				{
	                if (GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Start Server"))
	                {
	                    print("Starting Server");
	                    StartServer();
	                }
				}

                int count = 0;
				HostData h;
				h = new HostData();
				h.gameName = "UDPP2P client test";
				string[] clientIP = new string[1];
				
				// This sample uses a hard coded server address, we would get this infomation from a master server, or PSN host data
				clientIP[0] = "192.168.60.45";
				h.ip = clientIP;
				h.port = udpp2pPort;
				h.guid = "";
				if (GUI.Button(new Rect(btnX * 2f + btnW, btnY * 1.2f + (btnH * count), btnW * 3f, btnH * .5f), h.ip[0]))
				{
					UnityEngine.PS4.Networking.enableUDPP2P = udpp2pNetworkingEnabled;						
					
					// connect to the server
					Network.Connect(h);
				}

            }
        }

    }

    
}


