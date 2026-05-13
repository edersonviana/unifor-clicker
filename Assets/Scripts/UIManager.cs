using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI textoVerba;

    void Update()
    {
        textoVerba.text = "Verba: " + Mathf.Floor((float)GameManager.Instance.verba).ToString();
    }
}