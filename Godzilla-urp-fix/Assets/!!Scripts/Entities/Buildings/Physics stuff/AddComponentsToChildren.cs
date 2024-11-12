using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddComponentsToChildren : MonoBehaviour
{
    // Stage settings
    public List<GameObject> stage1Objects = new List<GameObject>();
    public List<GameObject> stage2Objects = new List<GameObject>();
    public List<GameObject> stage3Objects = new List<GameObject>();

    public void ApplyStage1()
    {
        ApplyComponents(stage1Objects);
    }

    public void ApplyStage2()
    {
        ApplyComponents(stage2Objects);
    }

    public void ApplyStage3()
    {
        ApplyComponents(stage3Objects);
        PlayBuildingDestructionSfx();
        StartCoroutine(DisableParentAfterDelay());
    }

    private void ApplyComponents(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                if (obj.GetComponent<BoxCollider>() == null)
                {
                    obj.AddComponent<BoxCollider>();
                }

                if (obj.GetComponent<Rigidbody>() == null)
                {
                    obj.AddComponent<Rigidbody>();
                }

                StartCoroutine(DisableAfterDelay(obj));
            }
        }
    }

    private IEnumerator DisableAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(5f);
        obj.SetActive(false);
    }

    private IEnumerator DisableParentAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
    void PlayBuildingDestructionSfx()
    {
        SFXManager.Instance.PlayAudioWithVolume("explosion", 1);

    }

}
