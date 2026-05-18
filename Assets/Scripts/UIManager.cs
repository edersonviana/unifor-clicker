using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI verbaTexto;
    
    [Header("Textos dos Botões de RH")]
    public TextMeshProUGUI textoBotaoSeguranca;
    public TextMeshProUGUI textoBotaoBibliotecario;
    public TextMeshProUGUI textoBotaoProfessor;  // <-- NOVO
    public TextMeshProUGUI textoBotaoCozinheiro; // <-- NOVO
    public TextMeshProUGUI textoBotaoClique;     // <-- NOVO

    void Update()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null || verbaTexto == null) return;

        // 1. Atualiza a verba na tela
        verbaTexto.text = "Verba: R$ " + gm.verba.ToString("F0");

        // 2. Atualiza o Segurança
        if (textoBotaoSeguranca != null)
        {
            int podeComprarSeg = (int)(gm.verba / gm.custoSeguranca);
            textoBotaoSeguranca.text = $"Contratar Segurança (R$ {gm.custoSeguranca})\nPossui: {gm.totalSegurancas} | Pode comprar: {podeComprarSeg}";
        }

        // 3. Atualiza o Bibliotecário
        if (textoBotaoBibliotecario != null)
        {
            int podeComprarBib = (int)(gm.verba / gm.custoBibliotecario);
            textoBotaoBibliotecario.text = $"Contratar Bibliotecário (R$ {gm.custoBibliotecario})\nPossui: {gm.totalBibliotecarios} | Pode comprar: {podeComprarBib}";
        }

        // 4. Atualiza o Professor
        if (textoBotaoProfessor != null)
        {
            int podeComprarProf = (int)(gm.verba / gm.custoProfessor);
            textoBotaoProfessor.text = $"Contratar Professor (R$ {gm.custoProfessor})\nPossui: {gm.totalProfessores} | Pode comprar: {podeComprarProf}";
        }

        // 5. Atualiza o Cozinheiro
        if (textoBotaoCozinheiro != null)
        {
            int podeComprarCoz = (int)(gm.verba / gm.custoCozinheiro);
            textoBotaoCozinheiro.text = $"Contratar Cozinheiro (R$ {gm.custoCozinheiro})\nPossui: {gm.totalCozinheiros} | Pode comprar: {podeComprarCoz}";
        }

        // 6. Atualiza o Upgrade de Clique
        if (textoBotaoClique != null)
        {
            int podeComprarClique = (int)(gm.verba / gm.custoClique);
            textoBotaoClique.text = $"Upgrade de Clique (R$ {gm.custoClique})\nValor clique atual: {gm.verbaPorClique} | Pode comprar: {podeComprarClique}";
        }
    }
}