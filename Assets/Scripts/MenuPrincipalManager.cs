using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SairDoJogo()
    {
        Debug.Log("Fechando o jogo...");
        Application.Quit();
    }
}