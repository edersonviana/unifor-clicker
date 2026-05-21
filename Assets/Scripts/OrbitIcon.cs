using UnityEngine;

public class OrbitIcon : MonoBehaviour
{
    public float raio = 150f;
    public float velocidade = 30f;
    public float anguloInicial = 0f;

    private float anguloAtual;
    private RectTransform retTransform;
    private RectTransform centro;

    void Start()
    {
        retTransform = GetComponent<RectTransform>();
        centro = transform.parent.GetComponent<RectTransform>();
        anguloAtual = anguloInicial;
    }

    void Update()
    {
        anguloAtual += velocidade * Time.deltaTime;

        float x = Mathf.Cos(anguloAtual * Mathf.Deg2Rad) * raio;
        float y = Mathf.Sin(anguloAtual * Mathf.Deg2Rad) * raio;

        retTransform.anchoredPosition = new Vector2(x, y);
    }
}