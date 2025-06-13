using UnityEngine.SceneManagement;
using UnityEngine;

public class Scenes : MonoBehaviour
{
    void Update()
    {
        if(IsTitleScene() && Input.anyKeyDown)
        {
            LoadGameScene();
		}

		if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift))
        {
            if (IsGameScene())
            {
                LoadTitleScene();
            }
        }
        
        if ((Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.R)))
        {
            ReloadCurrentScene();
        }
    }
    
    private void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    
    private bool IsTitleScene()
    {
        return SceneManager.GetActiveScene().name == "TitleScene";
    }
    public bool IsGameScene()
    {
        return SceneManager.GetActiveScene().name == "GameScene";
    }
    
    public void LoadTitleScene()
    {
        Debug.Log("Loading Title Scene");
        SceneManager.LoadScene("TitleScene");
    }
    
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
