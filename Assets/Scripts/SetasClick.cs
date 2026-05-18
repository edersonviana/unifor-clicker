using UnityEngine;
using UnityEngine.UI;

public class SetasClick : MonoBehaviour
{
    public GameObject prefabSeta;
    public float raio = 250f;
    private int setasAtuais = 0;

    void Update()
    {
        int totalSetas = (int)GameManager.Instance.verbaPorClique;
        
        if (totalSetas > setasAtuais)
        {
            AdicionarSeta();
        }
    }

    void AdicionarSeta()
    {
        setasAtuais++;
        ReposicionarTodas();

        GameObject novaSeta = Instantiate(prefabSeta, transform);
        AplicarPosicaoERotacao(novaSeta.GetComponent<RectTransform>(), setasAtuais - 1);
    }

    void ReposicionarTodas()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            AplicarPosicaoERotacao(transform.GetChild(i).GetComponent<RectTransform>(), i);
        }
    }

    void AplicarPosicaoERotacao(RectTransform rt, int index)
    {
        float angulo = 270f + (360f / setasAtuais) * index;
        float rad = angulo * Mathf.Deg2Rad;
        rt.anchoredPosition = new Vector2(Mathf.Cos(rad) * raio, Mathf.Sin(rad) * raio);
        rt.rotation = Quaternion.Euler(0, 0, angulo + 90f);
    }
}