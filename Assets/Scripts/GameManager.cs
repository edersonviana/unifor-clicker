using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Recursos")]
    public double verba = 0;
    public double verbaPorClique = 1;
    public double verbaPorSegundo = 0;

    [Header("Progresso")]
    public float notaMEC = 1.0f;
    public int setoresRecuperados = 0;
    public int totalSetores = 8;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        verba += verbaPorSegundo * Time.deltaTime;
    }

    public void Clicar()
    {
        verba += verbaPorClique;
    }
}