using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetorManager : MonoBehaviour
{
    [Header("Interface e Visual")]
    public string nomeDoSetor = "Biblioteca";
    public Image imagemCenario; 
    public Image imagemMiniaturaCard; 
    public Sprite[] estadosSprite = new Sprite[3]; // AGORA SÃO 3: 0 = Ruínas, 1 = Em Obras, 2 = Concluído
    public Button botaoUpgrade;
    public TextMeshProUGUI textoBotao;
    public Slider barraProgresso;
    public TextMeshProUGUI textoProgresso; 

    [Header("Grupos de Personagens")]
    public GameObject grupoRuinas;
    public GameObject grupoObras;
    public GameObject grupoConcluido; // Removi o 4º grupo que estava sobrando

    private int estadoAtual = 0; // Começa no 0 (Ruínas)

    [Header("Requisitos: Nível 1 (Ir para Obras)")]
    public double custoVerbaN1 = 1500;
    public int reqSegurancasN1 = 2;
    public int reqEspecificoN1 = 2; 

    [Header("Requisitos: Nível 2 (Ir para Totalmente Concluído)")]
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
        if (botaoUpgrade != null)
        {
            botaoUpgrade.onClick.RemoveAllListeners();
            botaoUpgrade.onClick.AddListener(FazerUpgrade);
        }
    }

    void Update()
    {
        VerificarRequisitos();
    }

    private void VerificarRequisitos()
    {
        // TRAVA: O nível máximo agora é 2!
        if (estadoAtual >= 2) return; 

        bool podeComprar = false;
        GameManager gm = GameManager.Instance; 

        if (gm == null) return;

        double verbaNecessaria = 0;
        int segurancasNecessarios = 0;
        int profNecessarios = 0;

        if (estadoAtual == 0)
        {
            verbaNecessaria = custoVerbaN1;
            segurancasNecessarios = reqSegurancasN1;
            profNecessarios = reqEspecificoN1;
        }
        else if (estadoAtual == 1)
        {
            verbaNecessaria = custoVerbaN2;
            segurancasNecessarios = reqSegurancasN2;
            profNecessarios = reqEspecificoN2;
        }

        int profAtuais = ObterQuantidadeProfissionalExigido(gm);

        if (gm.verba >= verbaNecessaria && 
            gm.totalSegurancas >= segurancasNecessarios && 
            profAtuais >= profNecessarios)
        {
            podeComprar = true;
        }

        if (botaoUpgrade != null)
        {
            botaoUpgrade.interactable = podeComprar;
        }
        
        AtualizarTextoBotao(verbaNecessaria, segurancasNecessarios, profNecessarios);
    }

    public void FazerUpgrade()
    {
        // TRAVA: O nível máximo agora é 2
        if (estadoAtual >= 2) return;

        GameManager gm = GameManager.Instance;
        
        double verbaNecessaria = 0;
        int segurancasNecessarios = 0;
        int profNecessarios = 0;

        if (estadoAtual == 0)
        {
            verbaNecessaria = custoVerbaN1;
            segurancasNecessarios = reqSegurancasN1;
            profNecessarios = reqEspecificoN1;
        }
        else if (estadoAtual == 1)
        {
            verbaNecessaria = custoVerbaN2;
            segurancasNecessarios = reqSegurancasN2;
            profNecessarios = reqEspecificoN2;
        }

        int profAtuais = ObterQuantidadeProfissionalExigido(gm);

        if (gm.verba < verbaNecessaria || gm.totalSegurancas < segurancasNecessarios || profAtuais < profNecessarios)
        {
            Debug.Log("Faltam recursos para melhorar!");
            return; 
        }

        gm.verba -= verbaNecessaria;
        estadoAtual++;
        
        // Se chegou no nível 2 (máximo), contabiliza setor recuperado
        if (estadoAtual == 2) 
        {
            gm.setoresRecuperados++; 
        }

        AtualizarVisual();

        if (estadoAtual == 2)
        {
            if (botaoUpgrade != null) botaoUpgrade.interactable = false;
            if (textoBotao != null) textoBotao.text = "Setor Concluído!";
        }
    }

    private void AtualizarVisual()
    {
        if (estadosSprite.Length > estadoAtual && estadosSprite[estadoAtual] != null)
        {
            if (imagemCenario != null)
            {
                imagemCenario.sprite = estadosSprite[estadoAtual];
                imagemCenario.enabled = false;
                imagemCenario.enabled = true;
            }

            if (imagemMiniaturaCard != null)
            {
                imagemMiniaturaCard.sprite = estadosSprite[estadoAtual];
                imagemMiniaturaCard.gameObject.SetActive(false);
                imagemMiniaturaCard.gameObject.SetActive(true);
            }
        }

        if (grupoRuinas != null) grupoRuinas.SetActive(estadoAtual == 0);
        if (grupoObras != null) grupoObras.SetActive(estadoAtual == 1);
        if (grupoConcluido != null) grupoConcluido.SetActive(estadoAtual >= 2);

        if (barraProgresso != null)
        {
            // DIVIDE POR 2F AGORA!
            barraProgresso.value = (float)estadoAtual / 2f;
        }

        if (textoProgresso != null)
        {
            // DIVIDE POR 2F AGORA!
            int porcentagem = Mathf.RoundToInt(((float)estadoAtual / 2f) * 100);
            textoProgresso.text = porcentagem + "%";
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
        if (textoBotao != null)
        {
            textoBotao.text = $"Melhorar {nomeDoSetor}\nVerba: {vrb}\nSeguranças: {seg}\nProfissionais {tipoExigido}: {prof}";
        }
    }
}