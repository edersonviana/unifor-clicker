using UnityEngine;

[CreateAssetMenu(fileName = "NovoDialogo", menuName = "UniforClicker/Dialogo")]
public class DialogoData : ScriptableObject 
{
    public string nomePersonagem;
    public Sprite fotoPersonagem;
    [TextArea(3, 10)]
    public string[] falas;
}