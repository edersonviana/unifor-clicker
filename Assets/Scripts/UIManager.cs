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

    void Update()
    {
        GameManager gm = GameManager.Instance;
        
        // 1. Atualiza a verba na tela
        verbaTexto.text = "Verba: R$ " + gm.verba.ToString("F0");

        // 2. Atualiza o Segurança
        if (textoBotaoSeguranca != null)
        {
            textoBotaoSeguranca.text = $"Contratar Segurança (R$ {gm.custoSeguranca})\nPossui: {gm.totalSegurancas}";
        }

        // 3. Atualiza o Bibliotecário
        if (textoBotaoBibliotecario != null)
        {
            textoBotaoBibliotecario.text = $"Contratar Bibliotecário (R$ {gm.custoBibliotecario})\nPossui: {gm.totalBibliotecarios}";
        }

        // 4. Atualiza o Professor
        if (textoBotaoProfessor != null)
        {
            textoBotaoProfessor.text = $"Contratar Professor (R$ {gm.custoProfessor})\nPossui: {gm.totalProfessores}";
        }

        // 5. Atualiza o Cozinheiro (Para quando você for criar)
        if (textoBotaoCozinheiro != null)
        {
            textoBotaoCozinheiro.text = $"Contratar Cozinheiro (R$ {gm.custoCozinheiro})\nPossui: {gm.totalCozinheiros}";
        }
    }
}