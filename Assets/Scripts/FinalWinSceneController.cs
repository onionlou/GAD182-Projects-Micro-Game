using UnityEngine;

public class FinalWinSceneController : MonoBehaviour
{
    [SerializeField] private AudioClip victoryClip;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        if (audioSource != null && victoryClip != null)
        {
            audioSource.PlayOneShot(victoryClip);
            Debug.Log("FinalWinSceneController: A_VICTORY sound played.");
        }
        else
        {
            Debug.LogWarning("FinalWinSceneController: Missing AudioSource or AudioClip reference.");
        }
    }
}
