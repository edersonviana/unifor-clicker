using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Áudio")]
    public AudioClip musicaFundo;
    private AudioSource audioSource;

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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (musicaFundo != null && audioSource != null)
        {
            audioSource.clip = musicaFundo;
            audioSource.loop = true;
            audioSource.playOnAwake = false; // Controlamos via código
            audioSource.volume = 0.5f; // Volume inicial médio
            audioSource.Play();
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

    public bool ComprarClique()
    {
        if (verba >= custoClique)
        {
            verba -= custoClique;
            verbaPorClique += 1;
            custoClique *= 1.5; // Aumenta o custo para o próximo
            Debug.Log("Upgrade de clique comprado! Valor: " + verbaPorClique);
            return true;
        }
        return false;
    }

    // --- FUNÇÕES DE CONTRATAÇÃO (Retornam bool para facilitar feedback de áudio) ---

    public bool ContratarSeguranca()
    {
        if (verba >= custoSeguranca)
        {
            verba -= custoSeguranca;
            totalSegurancas++;
            Debug.Log("Segurança contratado! Total: " + totalSegurancas);
            return true;
        }
        return false;
    }

    public bool ContratarBibliotecario()
    {
        if (verba >= custoBibliotecario)
        {
            verba -= custoBibliotecario;
            totalBibliotecarios++;
            Debug.Log("Bibliotecário contratado! Total: " + totalBibliotecarios);
            return true;
        }
        return false;
    }

    public bool ContratarProfessor()
    {
        if (verba >= custoProfessor)
        {
            verba -= custoProfessor;
            totalProfessores++;
            Debug.Log("Professor contratado! Total: " + totalProfessores);
            return true;
        }
        return false;
    }

    public bool ContratarCozinheiro()
    {
        if (verba >= custoCozinheiro)
        {
            verba -= custoCozinheiro;
            totalCozinheiros++;
            Debug.Log("Cozinheiro contratado! Total: " + totalCozinheiros);
            return true;
        }
        return false;
    }
}