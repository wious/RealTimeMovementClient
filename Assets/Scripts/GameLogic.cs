using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    GameObject character;

    Vector2 characterPositionInPercent;

    Vector2 characterVelocityInPercent;

    //const float CharacterSpeed = 0.25f;

    //float HalfCharacterSpeed = Mathf.Sqrt(CharacterSpeed * CharacterSpeed + CharacterSpeed * CharacterSpeed/ 2f); //.0.125f;

    void Start()
    {
        //Debug.Log("HalfCharacterSpeed : ->" + HalfCharacterSpeed.ToString());

        NetworkedClientProcessing.SetGameLogic(this);

        Sprite circleTexture = Resources.Load<Sprite>("Circle");

        character = new GameObject("Character");

        character.AddComponent<SpriteRenderer>();
        character.GetComponent<SpriteRenderer>().sprite = circleTexture;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)
            || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            characterVelocityInPercent = Vector2.zero;

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                NetworkedClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyBoardInputUpdate + "," + KeyboardInputUpdate.Upright);

            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                NetworkedClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyBoardInputUpdate + "," + KeyboardInputUpdate.Upleft);

            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                NetworkedClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyBoardInputUpdate + "," + KeyboardInputUpdate.DownRight);

            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                NetworkedClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyBoardInputUpdate + "," + KeyboardInputUpdate.Downleft);

            }
            else if (Input.GetKey(KeyCode.D))

                NetworkedClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyBoardInputUpdate + "," + KeyboardInputUpdate.right);


            else if (Input.GetKey(KeyCode.A))
                NetworkedClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyBoardInputUpdate + "," + KeyboardInputUpdate.left);


            else if (Input.GetKey(KeyCode.W))
                NetworkedClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyBoardInputUpdate + "," + KeyboardInputUpdate.Up);


            else if (Input.GetKey(KeyCode.S))
                NetworkedClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyBoardInputUpdate + "," + KeyboardInputUpdate.Down);
            else 
            {
                NetworkedClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyBoardInputUpdate + "," + KeyboardInputUpdate.Nothing);
            }
        }

        characterPositionInPercent += (characterVelocityInPercent * Time.deltaTime);

        Vector2 screenPos = new Vector2(characterPositionInPercent.x * (float)Screen.width, characterPositionInPercent.y * (float)Screen.height);
        Vector3 characterPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0));
        characterPos.z = 0;
        character.transform.position = characterPos;

    }

    public void SetVelocityAndPosition(Vector2 vel, Vector2 pos)
    {
        characterVelocityInPercent = vel;
        characterPositionInPercent = pos;
    }
}
