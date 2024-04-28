using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Level")]
public class CLevelsConfig : ScriptableObject
{
    [SerializeField] private GameObject[] levelPrefabs;

    public GameObject GetLevelByIndex(int index)
    {
        return levelPrefabs[index];
    }
}
