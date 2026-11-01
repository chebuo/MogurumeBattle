using UnityEngine;
using UnityEngine.UI;
using R3;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]private GameObject[] playerImages;
    [SerializeField]private Sprite noJoinedSprites;
    [SerializeField]private Sprite joinedSprites;
    [SerializeField]private Sprite readySprites;

    [SerializeField]GameObject readyButton;

    DeviceManager deviceManager;

    void Start()
    {
        deviceManager=FindFirstObjectByType<DeviceManager>();
        readyButton.SetActive(false);

        for(int i=0;i<playerImages.Length;i++)
        {
            UpdatePlayerUI(i);
        }
        deviceManager.OnPlayerStateChanged.Subscribe(playerIndex =>
        {
            UpdatePlayerUI(playerIndex);
            UpdateReadyUI();
            DeviceManager.playerCount=deviceManager.GetPlayerCount();
        }).AddTo(this);

        deviceManager.OnAllReady.Subscribe(_ =>
        {
            AllReady();
            DeviceManager.playerCount=deviceManager.GetPlayerCount();
        }).AddTo(this);
    }
    private void UpdatePlayerUI(int playerIndex)
    {
        GameObject playerImage=playerImages[playerIndex];
        Transform child=playerImage.gameObject.transform.GetChild(0);
        PlayerJoinState state=deviceManager.GetPlayerState(playerIndex);
        switch (state)
        {
            case PlayerJoinState.NoJoined:
                playerImages[playerIndex].SetActive(true);
                child.gameObject.SetActive(false);
                child.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case PlayerJoinState.Joined:
                playerImages[playerIndex].SetActive(true);
                child.gameObject.SetActive(true);
                child.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case PlayerJoinState.Ready:
                playerImages[playerIndex].SetActive(true);
                child.gameObject.SetActive(true);
                child.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                break;
        }
    }

    private void UpdateReadyUI()
    {
        bool isAllReady=deviceManager.IsAllReady();

        readyButton.SetActive(isAllReady);
    }

    private void AllReady()
    {
        readyButton.SetActive(true);
    }
}