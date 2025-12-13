using UnityEngine;

public class FireballSumonnerVoice : MonoBehaviour
{
    public GameObject fireBallPrefab;
    public Transform spawnPoint;

    // 🔧 CHANGE THIS TYPE
    public TurnFighter player;

public void SummonFireball()
{
    if (!fireBallPrefab || !spawnPoint || !player)
    {
        Debug.LogError("FireballSummonnerVoice: Missing references!");
        return;
    }

    GameObject fb = Instantiate(fireBallPrefab, spawnPoint.position, Quaternion.identity);
    FireBall fbScript = fb.GetComponent<FireBall>();

    if (!fbScript)
    {
        Debug.LogError("FireBall script missing on prefab!");
        return;
    }

    fbScript.direction = player.facingDirection == 1
        ? Vector2.right
        : Vector2.left;
}

}
