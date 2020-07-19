using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

[System.Obsolete]
public class SceneController : MonoBehaviour
{
    public void LoadGameScene(){
        SceneManager.LoadScene("GameScene");
    }
    public void LoadMenuScene(){
        Destroy(NetworkManager.singleton.gameObject);
        NetworkManager.Shutdown();
        SceneManager.LoadScene("MenuScene");
    }
}
