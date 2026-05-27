using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetorManager : MonoBehaviour
{
    [Header("Interface e Visual")]
    public string nomeDoSetor = "Setor";
    public Image imagemNormal;    
    public Image imagemExpandida; 
    public Sprite[] estadosSprite = new Sprite[3]; // 0 = Ruínas, 1 = Em Obras, 2 = Concluído
    public Button botaoUpgrade;
    public TextMeshProUGUI textoBotao;
    
    [Header("Barra de Progresso")]
    public Image barraProgresso; 
    public TextMeshProUGUI textoPorcentagem;

    [Header("Personagens: Menu Lateral (SetorCard)")]
    public GameObject[] personagensSetorCard; 

    [Header("Personagens: Tela Central (ImagemSetor)")]
    public GameObject[] personagensImagemSetor;

    [Header("Imagens Felizes (Serve para os dois acima)")]
    public Sprite[] spritesFelizes; 

    // --- NOVO: Variável para as fumaças ---
    [Header("Fumaças da Obra")]
    // Arraste para cá todas as fumaças que devem sumir (do menu e da tela central)
    public GameObject[] fumaçasDoSetor; 
    // --------------------------------------

    [Header("Áudios")]
    public AudioSource audioSource;
    public AudioClip somAbrir;
    public AudioClip somFechar;

    private int estadoAtual = 0;

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
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0;
        }

        AtualizarVisual();
        
        if (setorAtivo == null) AtivarEsteSetor();
    }

    void OnEnable()
    {
        if (audioSource != null && somAbrir != null)
        {
            audioSource.spatialBlend = 0; 
            audioSource.PlayOneShot(somAbrir);
        }
    }

    void OnDisable()
    {
        if (audioSource != null && somFechar != null)
        {
            audioSource.PlayOneShot(somFechar);
        }
    }

    public void AtivarEsteSetor()
    {
        setorAtivo = this;

        if (botaoUpgrade != null)
        {
            botaoUpgrade.onClick.RemoveAllListeners();
            botaoUpgrade.onClick.AddListener(FazerUpgrade);
            VerificarRequisitos(); 
        }
    }

    void Update()
    {
        if (setorAtivo == this) VerificarRequisitos();
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
        ButtonAudioFeedback feedbackBotao = null;
        if (botaoUpgrade != null) feedbackBotao = botaoUpgrade.GetComponent<ButtonAudioFeedback>();

        double verbaNecessaria = (estadoAtual == 0) ? custoVerbaN1 : custoVerbaN2;
        int segurancasNecessarios = (estadoAtual == 0) ? reqSegurancasN1 : reqSegurancasN2;
        int profNecessarios = (estadoAtual == 0) ? reqEspecificoN1 : reqEspecificoN2;
        int profAtuais = ObterQuantidadeProfissionalExigido(gm);

        if (gm.verba < verbaNecessaria || gm.totalSegurancas < segurancasNecessarios || profAtuais < profNecessarios)
        {
            if (feedbackBotao != null) feedbackBotao.TocarFalha();
            return; 
        }

        gm.verba -= verbaNecessaria;
        gm.totalSegurancas -= segurancasNecessarios;
        SubtrairProfissionalExigido(gm, profNecessarios);

        estadoAtual++;

        if (feedbackBotao != null) feedbackBotao.TocarSucesso();

        gm.notaMEC += 0.7f; 
        if (gm.notaMEC > 5.0f) gm.notaMEC = 5.0f; 
        
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

            if (imagemNormal != null)
            {
                imagemNormal.sprite = spriteAtual;
                imagemNormal.enabled = false;
                imagemNormal.enabled = true; 
            }

            if (imagemExpandida != null)
            {
                imagemExpandida.sprite = spriteAtual;
                imagemExpandida.enabled = false;
                imagemExpandida.enabled = true; 
            }
        }

        if (barraProgresso != null)
        {
            if (estadoAtual == 0) barraProgresso.fillAmount = 0f;
            else if (estadoAtual == 1) barraProgresso.fillAmount = 0.5f;
            else if (estadoAtual == 2) barraProgresso.fillAmount = 1f;
        }

        if (textoPorcentagem != null)
        {
            if (estadoAtual == 0) textoPorcentagem.text = "0%";
            else if (estadoAtual == 1) textoPorcentagem.text = "50%";
            else if (estadoAtual == 2) textoPorcentagem.text = "100%";
        }

        if (estadoAtual == 2)
        {
            DeixarPersonagensFelizes();
            // --- NOVO: Chama a função para sumir com as fumaças ---
            DesativarFumaças();
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
        
        string nomeProfissao = ObterNomeProfissaoPlural();

        if (textoBotao != null)
        {
            textoBotao.text = $"Melhorar {nomeDoSetor}\nVerba: {vrb}\nSeguranças: {seg}\n{nomeProfissao}: {prof}";
        }
    }

    private void DeixarPersonagensFelizes()
    {
        if (spritesFelizes == null) return;

        if (personagensSetorCard != null)
        {
            for (int i = 0; i < personagensSetorCard.Length; i++)
            {
                if (personagensSetorCard[i] != null && i < spritesFelizes.Length && spritesFelizes[i] != null)
                {
                    TrocarSprite(personagensSetorCard[i], spritesFelizes[i]);
                }
            }
        }

        if (personagensImagemSetor != null)
        {
            for (int i = 0; i < personagensImagemSetor.Length; i++)
            {
                if (personagensImagemSetor[i] != null && i < spritesFelizes.Length && spritesFelizes[i] != null)
                {
                    TrocarSprite(personagensImagemSetor[i], spritesFelizes[i]);
                }
            }
        }
    }

    private void TrocarSprite(GameObject boneco, Sprite spriteFeliz)
    {
        Animator anim = boneco.GetComponent<Animator>();
        if (anim != null) anim.enabled = false;

        SpriteRenderer sr = boneco.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = spriteFeliz;

        Image img = boneco.GetComponent<Image>();
        if (img != null) img.sprite = spriteFeliz;
    }

    // --- NOVO: FUNÇÃO QUE DESATIVA AS FUMAÇAS ---
    private void DesativarFumaças()
    {
        if (fumaçasDoSetor == null) return;

        for (int i = 0; i < fumaçasDoSetor.Length; i++)
        {
            if (fumaçasDoSetor[i] != null)
            {
                fumaçasDoSetor[i].SetActive(false); // Isso desliga o objeto na cena
            }
        }
    }
    // --------------------------------------------
}