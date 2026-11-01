using UnityEngine;
using R3;

public class CameraController : MonoBehaviour
{
    int playerCount=1;
    [SerializeField]Camera[] cameras;
    DeviceManager deviceManager;
    void Awake()
    {
        deviceManager=FindAnyObjectByType<DeviceManager>();
        playerCount=DeviceManager.playerCount;
        UpdateCameraLayout(playerCount);
        Debug.Log(playerCount);
        deviceManager.OnPlayerStateChanged.Subscribe(playerNum =>
        {
            int playerCount=deviceManager.GetPlayerCount();
        }).AddTo(this);
    }

    private void UpdateCameraLayout(int playerCount)
    {
        if(playerCount==1)cameras[0].rect=new Rect(0,0,1,1);

        else if(playerCount==2){
            cameras[0].rect=new Rect(0,0,0.5f,1);
            cameras[1].rect=new Rect(0.5f,0,0.5f,1);
            for(int i=playerCount;i<cameras.Length;i++)cameras[i].enabled=false;
        }
        else if (playerCount == 3)
        {
            cameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            cameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            cameras[2].rect = new Rect(0, 0, 0.5f, 0.5f);
            cameras[3].rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        }
        else if (playerCount == 4)
        {
            cameras[0].rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
            cameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            cameras[2].rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            cameras[3].rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        }
    }
}