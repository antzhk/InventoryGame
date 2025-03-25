using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); 
        PlayerPrefs.Save();
        
        BackPack.ClearAll();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}