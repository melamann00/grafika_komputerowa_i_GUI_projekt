using UnityEngine;

public class SnowflakeBuilder : MonoBehaviour
{
    public Material iceMaterial;
    public float armLength = 4f;
    public float armThickness = 0.25f;
    public float depth = 0.4f;
    public int branchLevels = 2;
    public float branchScale = 0.45f;
    public float branchAngle = 60f;

    void Start() { Build(); }

    [ContextMenu("Build")]
    public void Build()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);

        for (int i = 0; i < 6; i++)
        {
            var arm = new GameObject("Arm" + i).transform;
            arm.SetParent(transform, false);
            arm.localRotation = Quaternion.Euler(0, 0, i * 60f);
            BuildBranch(arm, armLength, armThickness, branchLevels);
        }
    }

    void BuildBranch(Transform parent, float length, float thickness, int level)
    {
        var seg = GameObject.CreatePrimitive(PrimitiveType.Cube);
        seg.transform.SetParent(parent, false);
        seg.transform.localPosition = Vector3.up * length * 0.5f;
        seg.transform.localScale = new Vector3(thickness, length, depth);
        seg.GetComponent<MeshRenderer>().sharedMaterial = iceMaterial;
        DestroyImmediate(seg.GetComponent<BoxCollider>());

        if (level <= 0) return;

        foreach (float t in new[] { 0.4f, 0.7f })
        {
            foreach (int sign in new[] { -1, 1 })
            {
                var pivot = new GameObject("Branch").transform;
                pivot.SetParent(parent, false);
                pivot.localPosition = Vector3.up * length * t;
                pivot.localRotation = Quaternion.Euler(0, 0, sign * branchAngle);
                BuildBranch(pivot, length * branchScale, thickness * 0.8f, level - 1);
            }
        }
    }
}