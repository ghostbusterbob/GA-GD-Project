using UnityEngine;

public class bonescript : MonoBehaviour
{
    public string[] boneNames;  // List of bones to apply the wiggling effect to
    public float wiggleAmplitude = 0.05f;  // Amplitude of the wiggle (how far the bone moves)
    public float wiggleSpeed = 1.0f;  // Speed at which the bones wiggle

    private Transform[] boneTransforms;
    private float[] initialPositionsY;  // Store the initial Y position for each bone

    void Start()
    {
        boneTransforms = new Transform[boneNames.Length];
        initialPositionsY = new float[boneNames.Length];

        // Find the bone transforms and store their initial positions
        for (int i = 0; i < boneNames.Length; i++)
        {
            boneTransforms[i] = FindBoneTransform(boneNames[i]);
            if (boneTransforms[i] != null)
            {
                initialPositionsY[i] = boneTransforms[i].localPosition.y;  // Store initial Y position for wiggle
            }
            else
            {
                Debug.LogError("Bone " + boneNames[i] + " not found.");
            }
        }
    }

    void Update()
    {
        // Apply the wiggling effect to each bone using sine wave motion
        for (int i = 0; i < boneTransforms.Length; i++)
        {
            if (boneTransforms[i] != null)
            {
                float newY = initialPositionsY[i] + Mathf.Sin(Time.time * wiggleSpeed + i) * wiggleAmplitude;
                Vector3 newPosition = boneTransforms[i].localPosition;
                newPosition.y = newY;  // Only modify the Y position to create the wiggle effect
                boneTransforms[i].localPosition = newPosition;
            }
        }
    }

    Transform FindBoneTransform(string boneName)
    {
        // Find the bone in the character's hierarchy
        Transform[] allBones = GetComponentsInChildren<Transform>();
        foreach (var bone in allBones)
        {
            if (bone.name == boneName)
            {
                return bone;
            }
        }
        return null;  // Return null if the bone is not found
    }
}