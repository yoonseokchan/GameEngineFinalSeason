using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Game Setting Data")]
public class EnemyStatus : ScriptableObject
{
    public int startHp = 100;
    public int startAttack = 10;
    public float EnemyrMoveSpeed = 0.5f;
}