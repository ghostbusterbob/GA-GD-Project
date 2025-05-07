using UnityEngine;

public class flashlightgrabscript : MonoBehaviour
{
    public string parentBoneName = "Head";  // Name of the parent bone
    public string childBoneName = "UpperArm_L";  // Name of the child bone

    private Transform parentBoneTransform;
    private Transform childBoneTransform;

    void Start()
    {
        // Find the parent and child bones by name
        parentBoneTransform = FindBoneTransform(parentBoneName);
        childBoneTransform = FindBoneTransform(childBoneName);

        if (parentBoneTransform != null && childBoneTransform != null)
        {
            // Connect the bones (make the child follow the parent)
            ConnectBonesTogether();
        }
        else
        {
            Debug.LogError("Failed to find the specified bones.");
        }
    }

    Transform FindBoneTransform(string boneName)
    {
        // Get all transforms in the hierarchy
        Transform[] allBones = GetComponentsInChildren<Transform>();

        // Loop through and find the specific bone by name
        foreach (var bone in allBones)
        {
            if (bone.name == boneName)
            {
                return bone;
            }
        }

        return null;  // Return null if the bone is not found
    }

    void ConnectBonesTogether()
    {
        // Make the child bone follow the parent bone's position, rotation, and scale
        childBoneTransform.SetParent(parentBoneTransform);

        // Optionally, reset the local position and rotation to maintain alignment
        childBoneTransform.localPosition = Vector3.zero;
        childBoneTransform.localRotation = Quaternion.identity;
    }
}