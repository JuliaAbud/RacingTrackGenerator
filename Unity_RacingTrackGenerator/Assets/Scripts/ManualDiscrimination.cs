
using UnityEngine;
using UnityEngine.Events;

public class ManualDiscrimination : MonoBehaviour
{
    [SerializeField] private UnityEvent PressQ;
    [SerializeField] private UnityEvent PressW;

    [SerializeField] private UnityEvent PressSpace;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PressSpace?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PressQ?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            PressW?.Invoke();
        }
    }
}
