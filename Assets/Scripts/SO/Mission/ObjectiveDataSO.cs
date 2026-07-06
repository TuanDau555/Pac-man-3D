using UnityEngine;

public abstract class ObjectiveDataSO : ScriptableObject
{
    [Header("Mission")]
    [SerializeField] private string title;

    [SerializeField]
    [TextArea(3, 10)]
    private string description;

    public string Title => title;
    public string Description => description;
}