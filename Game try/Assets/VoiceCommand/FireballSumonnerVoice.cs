using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class FireballSumonnerVoice : MonoBehaviour
{
    public GameObject fireBallPrefab;   // your fireball prefab
    public Transform spawnPoint;        // where fireball appears

    public VoiceCommanV2 player; // reference to player (for facing direction)


    private void Start()
    {
        player = FindFirstObjectByType<VoiceCommanV2>();
    }

    public void SummonFireball()
    {
        // create fireball object
        GameObject fb = Instantiate(fireBallPrefab, spawnPoint.position, Quaternion.identity);

        // get fireball script and give it the direction
        FireBall fbScript = fb.GetComponent<FireBall>();

        // send direction based on player's facing

        if (player.facingDirection == 1)
            fbScript.direction = Vector2.right;
        else
            fbScript.direction = Vector2.left;
    }
}
