﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    private static GameManager instance;

    public Transform[] spawnPositions;
    public GameObject playerPrefab;

    private int[] playerScores;

    private void Start()
    {
        playerScores = new[] {0, 0};
        SpawnPlayer(); // 각각 플레이어가 한 번 실행

        if(PhotonNetwork.IsMasterClient)
        {
            // 원래는 ball spawn
        }
    }

    private void SpawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // 내부적으로는 0부터 셈
        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
    }

    // 만약 방 나가는 걸 구현한다면 버튼 하나 만들고
    // PhotonNetwork.leaveRoom()

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void AddScore(int playerNumber, int score)
    {
        if(PhotonNetwork.IsMasterClient) // host가 아니면 점수 변화X
        {
            return;
        }

        playerScores[playerNumber - 1] += score;

        photonView.RPC("RPCUpdateScoreText", RpcTarget.All, 
            playerScores[0].ToString(), playerScores[1].ToString());
    }
    
    [PunRPC]
    private void RPCUpdateScoreText(string player1ScoreText, string player2ScoreText)
    {
        
    }
}