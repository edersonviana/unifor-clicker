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

    [Header("Profissionais Contratados (RH)")]
    public int totalSegurancas = 0;
    public int totalBibliotecarios = 0;
    public int totalProfessores = 0;
    public int totalCozinheiros = 0;

    [Header("Custos de Contratação")]
    public double custoSeguranca = 500;
    public double custoBibliotecario = 800;
    public double custoProfessor = 1000;
    public double custoCozinheiro = 600;
    public double custoClique = 100;

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

    public void ComprarClique()
    {
        if (verba >= custoClique)
        {
            verba -= custoClique;
            verbaPorClique += 1;
            Debug.Log("Upgrade de clique comprado! Valor: " + verbaPorClique);
        }
    }

    // --- FUNÇÕES DE CONTRATAÇÃO (Adicionadas sem quebrar o resto) ---

    public void ContratarSeguranca()
    {
        if (verba >= custoSeguranca)
        {
            verba -= custoSeguranca;
            totalSegurancas++;
            Debug.Log("Segurança contratado! Total: " + totalSegurancas);
        }
    }

    public void ContratarBibliotecario()
    {
        if (verba >= custoBibliotecario)
        {
            verba -= custoBibliotecario;
            totalBibliotecarios++;
            Debug.Log("Bibliotecário contratado! Total: " + totalBibliotecarios);
        }
    }

    public void ContratarProfessor()
    {
        if (verba >= custoProfessor)
        {
            verba -= custoProfessor;
            totalProfessores++;
            Debug.Log("Professor contratado! Total: " + totalProfessores);
        }
    }

    public void ContratarCozinheiro()
    {
        if (verba >= custoCozinheiro)
        {
            verba -= custoCozinheiro;
            totalCozinheiros++;
            Debug.Log("Cozinheiro contratado! Total: " + totalCozinheiros);
        }
    }
}