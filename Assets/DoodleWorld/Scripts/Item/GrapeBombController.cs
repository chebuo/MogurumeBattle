using UnityEngine;
using UnityEngine.VFX;
using Cysharp.Threading.Tasks;
using R3;

public class GrapeBombController:MonoBehaviour,IWeapon
{
    private readonly ReactiveProperty<bool> isAttack=new(true);
    public ReadOnlyReactiveProperty<bool> IsAttack=>isAttack;

    private readonly ReactiveProperty<bool> isHit=new(false);
    public ReadOnlyReactiveProperty<bool> IsHit=>isHit;

    float power;

    VisualEffect effect;

    Collider col;
    PlayerController playerController;
    TerrainGenerator terrainGenerator;

    AudioSource audioSource;
    [SerializeField]AudioClip digSound;

    void Awake()
    {
        effect=this.GetComponent<VisualEffect>();
        effect.Stop();
        col=this.GetComponent<Collider>();
        terrainGenerator=FindFirstObjectByType<TerrainGenerator>();

        audioSource=this.GetComponent<AudioSource>();
    }

    public async UniTask Attack(GameObject owner,float power)
    {
        playerController=owner.GetComponent<PlayerController>();
        this.power=power;

        await WaitBomb();
        await BombGrape();
    }

    private async UniTask WaitBomb()
    {
        Debug.Log("waiting");
        await UniTask.Delay(2000);
    }

    private async UniTask BombGrape()
    {
        effect.Play();
        float amount=terrainGenerator.DigCollider(col,power);
        playerController.AddDigAmount(amount);
        PlayDigSound(amount);
        isHit.Value=true;
        await UniTask.Yield();
        isAttack.Value=false;
    }

    private void PlayDigSound(float amount)
    {
        // 地形を掘れていなければ音を鳴らさない
        if (amount <= 0) return;

        // AudioSourceやAudioClipが設定されていなければ終了
        if (audioSource == null || digSound == null) return;

        audioSource.PlayOneShot(digSound);
    }

}