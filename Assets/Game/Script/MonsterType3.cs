using Unity.VisualScripting;
using UnityEngine;

public class MonsterType3 : MonsterBase
{
    protected override void Awake()
    {
        base.Awake();
        Initialize("Catfish", 5000f, 500f, 4f, 20f);
        KC = 3;
        coin = 100;
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void PlayerAttack(float playerDamage)
    {
        base.PlayerAttack(playerDamage);
    }
    public override void MonsterDead()
    {
        base.MonsterDead();
    }
}