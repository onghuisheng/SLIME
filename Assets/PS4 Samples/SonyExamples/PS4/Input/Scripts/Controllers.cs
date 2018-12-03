using UnityEngine;
using System.Collections;
using UnityEngine.PS4;
using System;

public class Controllers : MonoBehaviour
{
    int[] handles;
    int numHandles;
    int maxControllers = 4;

    int sectionPadding = 30; // seperates player sections from one another
    int internalPadding = 10; // adds a bit of padding around the contents of the player section
    public Texture2D boxTexture;
    public Texture2D fingerTexture;
    Vector2 fingerTextureOrigin;

    void Start()
    {
        handles = new int[maxControllers];

        for (int i = 0; i < maxControllers; i++)
        {
            PS4Input.PadResetOrientation(i);
        }

        fingerTextureOrigin = new Vector2(fingerTexture.width * 0.5f, fingerTexture.height * 0.5f);

        #region Hide joystick on-screen representations, they're enabed if a player is connected or connects
        for (int i = 0; i < 4; i++)
        {
            GameObject.Find("joystick" + i).GetComponent<MeshRenderer>().enabled = false;
        } 
        #endregion

    }

    void Update()
    {
        numHandles = PS4Input.PadGetUsersHandles2(maxControllers, handles); // get details of connected controllers
        UpdateJoystickRepresentations();
    }

    void OnGUI()
    {
        for (int i = 0; i <= handles.Length; i++)
        {
            #region Get screen position an display area for this player
            Vector2 screenPosition = GetScreenPosition(i);
            Rect displayArea = new Rect(screenPosition.x, screenPosition.y, Screen.width / 2, Screen.height / 2); 
            #endregion

            if (PS4Input.PadIsConnected(i) == true)
            {
                DrawPlayerSection(i, displayArea);                
            }
            else
            {                
                GUI.Label(new Rect(screenPosition.x + 200, screenPosition.y + 200, 200, 200), "Connect Controller");
            }
        }

    }

    /// <summary>
    /// Display information on the current  player/controller
    /// Joypad buttons do no need to be mapped in the editor, they can be accessed using Input.GetKey(KeyCode) EG. Input.GetKey(KeyCode.Joystick1Button0), Input.GetKey(KeyCode.Joystick2Button0)
    /// </summary>
    internal void DrawPlayerSection(int playerId, Rect displayArea)
    {
        int playerColor = PS4Input.GetUsersDetails(playerId).color;
        int stickMappedPlayerId = playerId + 1;

        #region Define the display area for this player
        displayArea = new Rect(displayArea.x + sectionPadding + 10, displayArea.y + sectionPadding + 10, displayArea.width - (sectionPadding * 2), displayArea.height - (sectionPadding * 2));
        // draw the players' colour bar
        DrawQuad(new Rect(displayArea.x + 2, displayArea.y + 2, displayArea.width - 4, 20), GetPlayerColor(playerColor));
        // set the content position with internal padding
        Rect internalDisplayArea = new Rect(displayArea.x + internalPadding, displayArea.y + internalPadding, displayArea.width - (internalPadding * 2), displayArea.height - (internalPadding * 2));
        #endregion

        #region Display the results of button presses and stick movements for the current player
        GUI.Label(internalDisplayArea,
            // player details
            "\nPlayer ID: " + playerId +
            "\nColor: " + playerColor +
            "\nUser ID: " + PS4Input.GetUsersDetails(playerId).userId +
            "\nUser Name: " + PS4Input.GetUsersDetails(playerId).userName +
            "\nStatus: " + PS4Input.GetUsersDetails(playerId).status +
            "\nLast Orientation: " + PS4Input.PadGetLastOrientation(playerId) +
            "\nLast Gyro: " + PS4Input.PadGetLastGyro(playerId) +
            "\nLast Acceleration: " + PS4Input.PadGetLastAcceleration(playerId) +
            "\n" +
            // buttons
            "\nJoystick" + stickMappedPlayerId + "Button0 (cross): " + Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickMappedPlayerId + "Button0", true)).ToString() +
            "\nJoystick" + stickMappedPlayerId + "Button1 (circle): " + Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickMappedPlayerId + "Button1", true)).ToString() +
            "\nJoystick" + stickMappedPlayerId + "Button2 (square): " + Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickMappedPlayerId + "Button2", true)).ToString() +
            "\nJoystick" + stickMappedPlayerId + "Button3 (triangle): " + Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickMappedPlayerId + "Button3", true)).ToString() +
            "\nJoystick" + stickMappedPlayerId + "Button4 (left shoulder): " + Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickMappedPlayerId + "Button4", true)).ToString() +
            "\nJoystick" + stickMappedPlayerId + "Button5 (right shoulder): " + Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickMappedPlayerId + "Button5", true)).ToString() +
            "\nJoystick" + stickMappedPlayerId + "Button6 (touch pad button): " + Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickMappedPlayerId + "Button6", true)).ToString() +
            "\nJoystick" + stickMappedPlayerId + "Button7 (options): " + Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickMappedPlayerId + "Button7", true)).ToString() +
            "\nJoystick" + stickMappedPlayerId + "Button8 (L3): " + Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickMappedPlayerId + "Button8", true)).ToString() +
            "\nJoystick" + stickMappedPlayerId + "Button9 (R3): " + Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickMappedPlayerId + "Button9", true)).ToString() +
            "\n" +
            // sticks & D-pad - mapped in editor
            "\ndpad" + stickMappedPlayerId + "_horizontal: " + Input.GetAxis("dpad" + stickMappedPlayerId + "_horizontal").ToString() +
            "\ndpad" + stickMappedPlayerId + "_vertical: " + Input.GetAxis("dpad" + stickMappedPlayerId + "_vertical").ToString() +
            "\nleftstick" + stickMappedPlayerId + "horizontal (left stick): " + Input.GetAxis("leftstick" + stickMappedPlayerId + "horizontal").ToString() +
            "\nleftstick" + stickMappedPlayerId + "vertical (left stick): " + Input.GetAxis("leftstick" + stickMappedPlayerId + "vertical").ToString() +
            "\nrightstick" + stickMappedPlayerId + "horizontal (right stick): " + Input.GetAxis("rightstick" + stickMappedPlayerId + "horizontal").ToString() +
            "\nrightstick" + stickMappedPlayerId + "vertical (right stick): " + Input.GetAxis("rightstick" + stickMappedPlayerId + "vertical").ToString() +
            "\njoystick" + stickMappedPlayerId + "_left_trigger (left trigger): " + Input.GetAxis("joystick" + stickMappedPlayerId + "_left_trigger").ToString() +
            "\njoystick" + stickMappedPlayerId + "_right_trigger (right trigger): " + Input.GetAxis("joystick" + stickMappedPlayerId + "_right_trigger").ToString()
            ); 
        #endregion

        DrawPadTouches(displayArea, playerId);

        #region Draw a box around this player section
        GUI.skin.box.normal.background = boxTexture;
        GUI.Box(displayArea, ""); 
        #endregion

    }

    private void DrawPadTouches(Rect displayArea, int playerId)
    {
        int touchNum, touch0x, touch0y, touch0id, touch1x, touch1y, touch1id;
        int touchResolutionX, touchResolutionY, analogDeadZoneLeft, analogDeadZoneRight;
        float touchPixelDensity;
        PS4Input.ConnectionType connectionType;

        // touchPixelDensity (dots per millimeter) 
        // touchResolutionX (x resolution in pixels) 
        // touchResolutionY (y resolution in pixels) 
        PS4Input.GetPadControllerInformation(playerId, out touchPixelDensity, out touchResolutionX, out touchResolutionY, out analogDeadZoneLeft, out analogDeadZoneRight, out connectionType);

        PS4Input.GetLastTouchData(playerId, out touchNum, out touch0x, out touch0y, out touch0id, out touch1x, out touch1y, out touch1id);

        GUI.Label(new Rect(displayArea.x + 400, displayArea.y, 300, 60), "Connection type: " + connectionType.ToString());

        GUI.Label(new Rect(displayArea.x + 400, displayArea.y + 40, 300, 60), "Number of Active Touches: " + touchNum);
        GUI.Label(new Rect(displayArea.x + 400, displayArea.y + 60, 300, 60), "Pad Touch 0: " + " touchId: " + touch0id + ", x:" + touch0x + ", y:" + touch0y);
        GUI.Label(new Rect(displayArea.x + 400, displayArea.y + 80, 300, 60), "Pad Touch 1: " + " touchId: " + touch1id + ", x:" + touch1x + ", y:" + touch1y);

        #region Draw touches to the player display area
        if (touchNum > 0)
        {
            int xDraw = 0;
            int yDraw = 0;

            if (touch0x > 0 || touch0y > 0)
            {
                xDraw = (int)(displayArea.x + (displayArea.width / touchResolutionX) * touch0x - fingerTextureOrigin.x);
                yDraw = (int)(displayArea.y + (displayArea.height / touchResolutionY) * touch0y - fingerTextureOrigin.y);

                GUI.DrawTexture(new Rect(xDraw, yDraw, 128, 128), fingerTexture);
            }

            if (touchNum > 1 && (touch1x > 0 || touch1y > 0))
            {
                xDraw = (int)(displayArea.x + (displayArea.width / touchResolutionX) * touch1x - fingerTextureOrigin.x);
                yDraw = (int)(displayArea.y + (displayArea.height / touchResolutionY) * touch1y - fingerTextureOrigin.y);

                GUI.DrawTexture(new Rect(xDraw, yDraw, 128, 128), fingerTexture);
            }

        } 
        #endregion

    }

    private void UpdateJoystickRepresentations()
    {
        for (int i = 0; i < numHandles; i++)
        {
            GameObject currentJoystick = GameObject.Find("joystick" + i);
            MeshRenderer meshrenderer = currentJoystick.GetComponent<MeshRenderer>();

            if (PS4Input.PadIsConnected(i) == true)
            {
                if (meshrenderer.enabled == false)
                {
                    meshrenderer.enabled = true;
                }

                currentJoystick.transform.eulerAngles = new Vector3(-PS4Input.PadGetLastOrientation(i).x, PS4Input.PadGetLastOrientation(i).y, PS4Input.PadGetLastOrientation(i).z) * 100;
            }
            else if (meshrenderer.enabled == true)
            {
                meshrenderer.enabled = false;
            }

        }

    }

    void DrawQuad(Rect position, Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.skin.box.normal.background = texture;
        GUI.Box(position, GUIContent.none);
    }

    internal Vector2 GetScreenPosition(int connectedControllerId)
    {
        Vector2 position;

        if (connectedControllerId < 2) // top row of screen
        {
            position.x = (Screen.width / 2) * connectedControllerId;
            position.y = 0;
        }
        else // bottom row of screen
        {
            position.x = (Screen.width / 2) * (connectedControllerId - 2);
            position.y = Screen.height / 2;
        }

        return position;
    }

    Color GetPlayerColor(int colorId)
    {
        switch (colorId)
        {
            case 0:
                return Color.blue;
            case 1:
                return Color.red;
            case 2:
                return Color.green;
            case 3:
                return Color.magenta;
            default:
                return Color.black;
        }
    }

}

