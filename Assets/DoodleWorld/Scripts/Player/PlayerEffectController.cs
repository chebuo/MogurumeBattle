using UnityEngine;
using Cysharp.Threading.Tasks;
using R3;

public class PlayerEffectController : MonoBehaviour
{
    Material material;

    [SerializeField]Color hitColor=Color.red;
    [SerializeField]Color walkColor=Color.cyan;
    [SerializeField]Color attackColor=Color.blue;
    [SerializeField]Color digColor=Color.yellow;
    [SerializeField]Color heelColor=Color.green;

    void Awake()
    {
        Renderer renderer=GetComponent<Renderer>();
        material=renderer.material;
    }

    public async UniTask HitEffect(int damage)
    {
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor",hitColor);
        await HitStop(damage);
        await UniTask.Delay(500);
        material.DisableKeyword("_EMISSION");
        Debug.Log("hitEffect");
    }

    public async UniTask HitStop(int damage)
    {
        int hitStop=damage*10;
        int minHitStop=100;
        int maxHitStop=500;
        if(hitStop<=minHitStop)hitStop=minHitStop;
        if(hitStop>=maxHitStop)hitStop=maxHitStop;

        Time.timeScale=0;
        await UniTask.Delay(hitStop,ignoreTimeScale:true);
        Time.timeScale=1;
    }

    public async UniTask WalkEffect()
    {
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor",walkColor);
        await UniTask.Delay(500);
        material.DisableKeyword("_EMISSION");
        Debug.Log("walkEffect");
    }

    public async UniTask AttackEffect()
    {
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor",attackColor);
        await UniTask.Delay(500);
        material.DisableKeyword("_EMISSION");
        Debug.Log("walkEffect");
    }
    public async UniTask DigEffect()
    {
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor",digColor);
        await UniTask.Delay(500);
        material.DisableKeyword("_EMISSION");
        Debug.Log("walkEffect");
    }
    public async UniTask HealEffect()
    {
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor",heelColor);
        await UniTask.Delay(500);
        material.DisableKeyword("_EMISSION");
        Debug.Log("walkEffect");
    }
}