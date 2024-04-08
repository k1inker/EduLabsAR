using TMPro;
using UnityEngine;

public class CNickNameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNickName;

    public void SetNickName(string newNickName)
    {
        this.textNickName.text = newNickName;
    }
}
