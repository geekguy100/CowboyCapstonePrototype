using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private CharacterMovement characterMovement;

    void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
    }
}
