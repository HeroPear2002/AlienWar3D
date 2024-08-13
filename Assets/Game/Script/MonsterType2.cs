using Unity.VisualScripting;
using UnityEngine;

public class MonsterType2 : MonsterBase
{
    protected override void Awake()
    {
        base.Awake();
        Initialize("Rake", 5000f, 200f, 4f, 20f);
        KC = 3;
        coin = 50;
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