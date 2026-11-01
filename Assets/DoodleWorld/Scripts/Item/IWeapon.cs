using UnityEngine;
using Cysharp.Threading.Tasks;
using R3;

interface IWeapon
{
    ReadOnlyReactiveProperty<bool> IsAttack { get; }
    ReadOnlyReactiveProperty<bool> IsHit { get; }

    public UniTask Attack(GameObject owner,float power);
}