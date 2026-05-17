using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetorManager : MonoBehaviour
{
    [Header("Interface e Visual")]
    public string nomeDoSetor = "Setor";
    public Image imagemNormal;    // <-- Para o SetorCard (encolhido)
    public Image imagemExpandida; // <-- Para a ImagemSetor (ampliado)
    public Sprite[] estadosSprite = new Sprite[3]; // 0 = Ruínas, 1 = Em Obras, 2 = Concluído
    public Button botaoUpgrade;
    public TextMeshProUGUI textoBotao;
    
    [Header("Barra de Progresso")]
    public Image barraProgresso; // <-- NOVO: Arraste a imagem azul (fill) da barra aqui
    public TextMeshProUGUI textoPorcentagem;

    private int estadoAtual = 0; // Começa no 0 (Ruínas)

    // NOVO: Variável global que avisa quem é o "dono" do menu lateral no momento
    public static SetorManager setorAtivo; 

    [Header("Requisitos: Nível 1 (Ir para Obras)")]
    public double custoVerbaN1 = 1500;
    public int reqSegurancasN1 = 2;
    public int reqEspecificoN1 = 2;

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
        
        // Se o jogo começou agora, o primeiro setor a carregar assume o controle do menu
        if (setorAtivo == null) 
        {
            AtivarEsteSetor();
        }
    }

    // --- CORREÇÃO DO BUG DO BOTÃO (PRÉDIOS BRIGANDO) ---
    // Esta função deve ser chamada quando você clica na imagem do prédio para expandir
    public void AtivarEsteSetor()
    {
        setorAtivo = this;

        if (botaoUpgrade != null)
        {
            botaoUpgrade.onClick.RemoveAllListeners();
            botaoUpgrade.onClick.AddListener(FazerUpgrade);
            VerificarRequisitos(); // Atualiza o texto na mesma hora
        }
    }

    void Update()
    {
        // O código do botão SÓ RODA se este prédio for o que você clicou por último
        if (setorAtivo == this)
        {
            VerificarRequisitos();
        }
    }

    private void VerificarRequisitos()
    {
        if (estadoAtual >= 2) 
        {
            if (botaoUpgrade != null) botaoUpgrade.interactable = false;
            if (textoBotao != null) textoBotao.text = $"{nomeDoSetor} Concluído!";
            return; 
        }

        bool podeComprar = false;
        GameManager gm = GameManager.Instance; 

        double verbaNecessaria = (estadoAtual == 0) ? custoVerbaN1 : custoVerbaN2;
        int segurancasNecessarios = (estadoAtual == 0) ? reqSegurancasN1 : reqSegurancasN2;
        int profNecessarios = (estadoAtual == 0) ? reqEspecificoN1 : reqEspecificoN2;

        int profAtuais = ObterQuantidadeProfissionalExigido(gm);

        if (gm.verba >= verbaNecessaria && gm.totalSegurancas >= segurancasNecessarios && profAtuais >= profNecessarios)
        {
            podeComprar = true;
        }

        if (botaoUpgrade != null) botaoUpgrade.interactable = podeComprar;
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

        if (gm.verba < verbaNecessaria || gm.totalSegurancas < segurancasNecessarios || profAtuais < profNecessarios)
        {
            return; 
        }

        // --- CORREÇÃO: AGORA ELE SUBTRAI OS FUNCIONÁRIOS DA SUA CONTA ---
        gm.verba -= verbaNecessaria;
        gm.totalSegurancas -= segurancasNecessarios;
        SubtrairProfissionalExigido(gm, profNecessarios);

        estadoAtual++;
        
        if (estadoAtual == 2) gm.setoresRecuperados++; 

        AtualizarVisual();

        if (estadoAtual == 2)
        {
            if (botaoUpgrade != null) botaoUpgrade.interactable = false;
            if (textoBotao != null) textoBotao.text = $"{nomeDoSetor} Concluído!";
        }
    }

    private void AtualizarVisual()
    {
        if (estadosSprite.Length > estadoAtual && estadosSprite[estadoAtual] != null)
        {
            Sprite spriteAtual = estadosSprite[estadoAtual];

            // 1. Atualiza a imagem pequena (SetorCard)
            if (imagemNormal != null)
            {
                imagemNormal.sprite = spriteAtual;
                imagemNormal.enabled = false;
                imagemNormal.enabled = true; // Força o redesenho
            }

            // 2. Atualiza a imagem grande (ImagemSetor)
            if (imagemExpandida != null)
            {
                imagemExpandida.sprite = spriteAtual;
                imagemExpandida.enabled = false;
                imagemExpandida.enabled = true; // Força o redesenho
            }
        }

        // --- BARRA DE PROGRESSO ---
        if (barraProgresso != null)
        {
            if (estadoAtual == 0) barraProgresso.fillAmount = 0f;
            else if (estadoAtual == 1) barraProgresso.fillAmount = 0.5f;
            else if (estadoAtual == 2) barraProgresso.fillAmount = 1f;
        }

        // --- PORCENTAGEM ---
        if (textoPorcentagem != null)
        {
            if (estadoAtual == 0) textoPorcentagem.text = "0%";
            else if (estadoAtual == 1) textoPorcentagem.text = "50%";
            else if (estadoAtual == 2) textoPorcentagem.text = "100%";
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

    private void SubtrairProfissionalExigido(GameManager gm, int quantidade)
    {
        switch (tipoExigido)
        {
            case TipoProfissional.Bibliotecario: gm.totalBibliotecarios -= quantidade; break;
            case TipoProfissional.Professor: gm.totalProfessores -= quantidade; break;
            case TipoProfissional.Cozinheiro: gm.totalCozinheiros -= quantidade; break;
        }
    }

    private string ObterNomeProfissaoPlural()
    {
        switch (tipoExigido)
        {
            case TipoProfissional.Bibliotecario: return "Bibliotecários";
            case TipoProfissional.Professor: return "Professores";
            case TipoProfissional.Cozinheiro: return "Cozinheiros";
            default: return "Profissionais";
        }
    }

    private void AtualizarTextoBotao(double vrb, int seg, int prof)
    {
        if (estadoAtual >= 2) return;
        
        // Puxa o nome correto baseado na configuração do Inspector
        string nomeProfissao = ObterNomeProfissaoPlural();

        if (textoBotao != null)
        {
            textoBotao.text = $"Melhorar {nomeDoSetor}\nVerba: {vrb}\nSeguranças: {seg}\n{nomeProfissao}: {prof}";
        }
    }
}