using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Rigidbody2D rbPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        GameReset();
    }

    void GameReset()
    {
        Player.position = new Vector2(0, 1.25f);
        rbPlayer.velocity = new Vector2(0, 0);
    }
}
