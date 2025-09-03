using UnityEngine;

//レシピのマテリアルを管理するクラス

public class RecipeButton : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer = default;

    //このレシピIDを取得します。
    public int CookingId { get => cookingId; private set { cookingId = value; } }
    [SerializeField]
    private int cookingId = -1;
    // このボタンの通常時マテリアルを指定します。
    [SerializeField]
    private Material normalMaterial;
    // このボタンの選択時マテリアルを指定します。
    [SerializeField]
    private Material pressedMaterial;

    //AudioSource型の変数aを宣言 使用するAudioSourceコンポーネントをアタッチ必要
    AudioSource audioSource;
    new MeshRenderer renderer;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<MeshRenderer>();
    }

    // このボタンの各パラメーターを一度に設定します。
    public void Initialize(int cookingId, Material normalMaterial, Material pressedMaterial)
    {
        this.CookingId = cookingId;
        this.normalMaterial = normalMaterial;
        this.pressedMaterial = pressedMaterial;
        renderer.material = normalMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
       var layerMask = 1 << other.gameObject.layer;
        if ((layerMask & playerLayer) != 0)
        {
            audioSource.Play();
            GetComponent<MeshRenderer>().material = pressedMaterial;
        }
    }

    void OnTriggerExit(Collider other)
    {
        var layerMask = 1 << other.gameObject.layer;
        if ((layerMask & playerLayer) != 0)
        {
            GetComponent<MeshRenderer>().material = normalMaterial;
        }
    }
}
