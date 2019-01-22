using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderSpeaker : SingletonMonoBehaviour<CommanderSpeaker>
{
    private bool m_IsMuted;

    public void ToggleMute(bool toggle)
    {
        m_IsMuted = toggle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sponge" && !m_IsMuted)
        {
            Sponge sponge = other.transform.parent.GetComponent<Sponge>();

            var controller = MoveController.GetControllerThatHolds(sponge.gameObject);

            if (controller != null)
            {
                controller.DetachCurrentObject(false);
            }

            var childRot = transform.GetChild(0).transform.rotation.eulerAngles;

            sponge.transform.position = transform.position;
            sponge.transform.rotation = transform.rotation;
            sponge.transform.position += sponge.transform.up * 0.2f;
            sponge.transform.Rotate(0, 90, 0, Space.Self);
            sponge.GetComponent<Rigidbody>().isKinematic = true;

            m_IsMuted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sponge" && m_IsMuted)
        {
            Sponge sponge = other.transform.parent.GetComponent<Sponge>();

            {
                m_IsMuted = false;
                sponge.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

}
