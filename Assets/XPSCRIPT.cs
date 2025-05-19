using UnityEngine;
using UnityEngine.UI;

public class XPSCRIPT : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    int xp1 = 0;
    [SerializeField]Text xpText;

    void Update()
    {
        updateXP();
    }

    public void AddXP(int xp)
    {
        xp1 += xp;
    }
    void updateXP()
    {
        xpText.text = xp1.ToString();
    }
}
