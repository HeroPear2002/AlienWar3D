using Unity.VisualScripting;
using UnityEngine;

public class MonsterType1 : MonsterBase
{
    protected override void Awake()
    {
        base.Awake();
        Initialize("Wolf", 8000f, 100f, 5f, 20f);
        KC = 4;
        coin = 80;
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