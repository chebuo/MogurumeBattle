using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private float power;
    public float multipler;
    private GameObject owner;

    public bool isAttack=true;
    private bool isHit=false;
    private bool damageProccess=true;

    IWeapon weapon;
    MeshRenderer[] meshRenderers;

    public void Init(float attackForce,GameObject owner)
    {
        this.power=attackForce*multipler;
        this.owner=owner;

        Attack().Forget();
    }

    void Awake()
    {
        weapon=this.GetComponent<IWeapon>();
        meshRenderers=this.GetComponentsInChildren<MeshRenderer>();

        weapon.IsAttack.Where(isAttack=>!isAttack).Take(1).Subscribe(async _=>{
            await DestroyAfterHit();
        }).AddTo(this);

        weapon.IsHit.Where(hit => hit).Subscribe(_ =>
        {
            Debug.Log("isHit:"+isHit);
            this.isHit=true;
        }).AddTo(this);
    }

    async UniTask Attack()
    {
        await UniTask.WaitUntil(()=>owner!=null);
        await weapon.Attack(owner,power);
    }

    private async UniTask DestroyAfterHit()
    {
        Debug.Log("damageProccess:"+damageProccess);
        // ヒット処理が終わるまで待つ
        await UniTask.WaitUntil(() => !damageProccess);
        for(int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].enabled=false;
        }
        
        Debug.Log("damageProccess:"+damageProccess+"finished");
        await UniTask.Delay(2000);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider col)
    {
        Debug.Log(this.gameObject.name+":"+isHit);
        if(!isHit)return;
        Debug.Log("isHit"+isHit);
        if(col.gameObject!=owner){
             Debug.Log("attack!!");
            IDamageable damageable=col.GetComponent<IDamageable>();
            if(damageable!=null)damageable.TakeDamage((int)power);
        }
       
        isHit=false;
        damageProccess=false;
        Debug.Log($"isHit:{isHit},damageProccess:{damageProccess}");
    }
}