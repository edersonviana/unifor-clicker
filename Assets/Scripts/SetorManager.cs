using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetorManager : MonoBehaviour
{
    [Header("Interface e Visual")]
    public string nomeDoSetor = "Biblioteca";
    public Image imagemCenario;
    public Sprite[] estadosSprite = new Sprite[3]; // 0 = Ruínas, 1 = Em Obras, 2 = Concluído
    public Button botaoUpgrade;
    public TextMeshProUGUI textoBotao;

    private int estadoAtual = 0; // Começa no 0 (Ruínas)

    [Header("Requisitos: Nível 1 (Ir para Obras)")]
    public double custoVerbaN1 = 1500;
    public int reqSegurancasN1 = 2;
    public int reqEspecificoN1 = 2; // Profissionais exigidos

    [Header("Requisitos: Nível 2 (Ir para Concluído)")]
    public double custoVerbaN2 = 5000;
    public int reqSegurancasN2 = 4;
    public int reqEspecificoN2 = 4;

    public enum TipoProfissional { Bibliotecario, Professor, Cozinheiro }
    [Header("Qual profissional este setor exige?")]
    public TipoProfissional tipoExigido;

    void Start()
    {
        AtualizarVisual();
    }

    void OnEnable()
    {
        // Quando este prédio for expandido/ativado, ele "toma o controle" do botão
        if (botaoUpgrade != null)
        {
            botaoUpgrade.onClick.RemoveAllListeners(); // Limpa as configurações antigas
            botaoUpgrade.onClick.AddListener(FazerUpgrade); // Liga a função DESTE prédio
        }
    }

    void Update()
    {
        VerificarRequisitos();
    }

    private void VerificarRequisitos()
    {
        if (estadoAtual >= 2) return; 

        bool podeComprar = false;
        GameManager gm = GameManager.Instance; // Puxando o Singleton do colega

        double verbaNecessaria = (estadoAtual == 0) ? custoVerbaN1 : custoVerbaN2;
        int segurancasNecessarios = (estadoAtual == 0) ? reqSegurancasN1 : reqSegurancasN2;
        int profNecessarios = (estadoAtual == 0) ? reqEspecificoN1 : reqEspecificoN2;

        int profAtuais = ObterQuantidadeProfissionalExigido(gm);

        // Verifica usando a variável "verba" do novo script
        if (gm.verba >= verbaNecessaria && 
            gm.totalSegurancas >= segurancasNecessarios && 
            profAtuais >= profNecessarios)
        {
            podeComprar = true;
        }

        botaoUpgrade.interactable = podeComprar;
        AtualizarTextoBotao(verbaNecessaria, segurancasNecessarios, profNecessarios);
    }

    public void FazerUpgrade()
    {
        if (estadoAtual >= 2) return;

        GameManager gm = GameManager.Instance;
        double verbaNecessaria = (estadoAtual == 0) ? custoVerbaN1 : custoVerbaN2;
        int segurancasNecessarios = (estadoAtual == 0) ? reqSegurancasN1 : reqSegurancasN2;
        int profNecessarios = (estadoAtual == 0) ? reqEspecificoN1 : reqEspecificoN2;
        int profAtuais = ObterQuantidadeProfissionalExigido(gm);

        // A TRAVA DE SEGURANÇA: Se não tiver TUDO que precisa, bloqueia o clique!
        if (gm.verba < verbaNecessaria || gm.totalSegurancas < segurancasNecessarios || profAtuais < profNecessarios)
        {
            Debug.Log("Faltam recursos para melhorar!");
            return; // Interrompe a função aqui, impedindo a verba de ficar negativa
        }

        // Se chegou aqui, é porque tem recursos suficientes. Pode cobrar!
        gm.verba -= verbaNecessaria;
        estadoAtual++;
        
        if (estadoAtual == 2) 
        {
            gm.setoresRecuperados++; 
        }

        AtualizarVisual();

        if (estadoAtual == 2)
        {
            botaoUpgrade.interactable = false;
            textoBotao.text = "Setor Concluído!";
        }
    }

    private void AtualizarVisual()
    {
        if (estadosSprite.Length > estadoAtual && estadosSprite[estadoAtual] != null)
        {
            imagemCenario.sprite = estadosSprite[estadoAtual];

            // Truque à prova de balas para forçar a Unity a redesenhar a imagem na mesma hora
            imagemCenario.enabled = false;
            imagemCenario.enabled = true;
        }
    }

    private int ObterQuantidadeProfissionalExigido(GameManager gm)
    {
        switch (tipoExigido)
        {
            case TipoProfissional.Bibliotecario: return gm.totalBibliotecarios;
            case TipoProfissional.Professor: return gm.totalProfessores;
            case TipoProfissional.Cozinheiro: return gm.totalCozinheiros;
            default: return 0;
        }
    }

    private void AtualizarTextoBotao(double vrb, int seg, int prof)
    {
        if (estadoAtual >= 2) return;
        textoBotao.text = $"Melhorar {nomeDoSetor}\nVerba: {vrb}\nSeguranças: {seg}\nProfissionais {tipoExigido}: {prof}";
    }
}