using UnityEngine;
using UnityEngine.VFX;
using Cysharp.Threading.Tasks;
using R3;

public class SoftcreamController : MonoBehaviour,IWeapon
{
    [SerializeField]float speed=2;
    [SerializeField]float speedMagnification=1.2f;
    float power;
    GameObject cream;
    private readonly ReactiveProperty<bool> isAttack=new(true);
    public ReadOnlyReactiveProperty<bool> IsAttack=>isAttack;

    private readonly ReactiveProperty<bool> isHit=new(false);
    public ReadOnlyReactiveProperty<bool> IsHit=>isHit;

    CapsuleCollider col;
    Rigidbody rb;
    VisualEffect effect;
    PlayerController playerController;
    TerrainGenerator terrainGenerator; 

    AudioSource audioSource;
    [SerializeField]AudioClip digSound;
    void Awake()
    {
        cream=transform.GetChild(1).gameObject;
        col=cream.GetComponent<CapsuleCollider>();
        rb=cream.GetComponent<Rigidbody>();
        effect=cream.GetComponent<VisualEffect>();
        effect.Stop();
        terrainGenerator=FindFirstObjectByType<TerrainGenerator>();

        audioSource=this.GetComponent<AudioSource>();
    }

    public UniTask Attack(GameObject owner,float power)
    {
        playerController=owner.GetComponent<PlayerController>();
        this.power=power;

        CreamMove().Forget();
        StopAttack().Forget();

        return UniTask.CompletedTask;
    }

    public async UniTask CreamMove()
    {
        isHit.Value=true;
        await UniTask.Yield();
        
        await UniTask.Delay(0);
        while (isAttack.Value)
        {            
            if(rb.linearVelocity.magnitude<=10)rb.AddForce(transform.up*speed);
            speed=speed*speedMagnification;
            col.height+=rb.linearVelocity.magnitude/20;
            col.center=new Vector3(col.center.x,col.center.y-rb.linearVelocity.magnitude/40,col.center.z);
            if(playerController!=null){
                float amount =terrainGenerator.DigCollider(col,power);
                playerController.AddDigAmount(amount);
                PlayDigSound(amount);
            }
            effect.Play();
            await UniTask.Delay(200,cancellationToken:destroyCancellationToken);
        }
    }

    private void PlayDigSound(float amount)
    {
         // 地形を掘れていなければ音を鳴らさない
        if (amount <= 0) return;

        // AudioSourceやAudioClipが設定されていなければ終了
        if (audioSource == null || digSound == null) return;

        audioSource.PlayOneShot(digSound);
    }

    private async UniTask StopAttack()
    {
        await UniTask.Delay(1500);
        if(playerController!=null)playerController.AddDigAmount(terrainGenerator.DigCollider(col,power));
        isAttack.Value=false;
    }

    void Onestroy()
    {
        isAttack.Dispose();
    }
}