using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGGameManager : MonoBehaviour
{
    public static RPGGameManager sharedInstance = null;
    public SpawnPoint playerSpawnPoint;
    public RPGCameraManager cameraManager;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SetupScene();
    }

    public void SetupScene()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPlayer()
    {
        if (playerSpawnPoint != null)
        {
            GameObject player = playerSpawnPoint.SpawnObject();

            cameraManager.virtualCamera.Follow = player.transform;
        }
    }
}
