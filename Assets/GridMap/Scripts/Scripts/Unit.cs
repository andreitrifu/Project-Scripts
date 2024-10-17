using UnityEngine;//

public class Unit : MonoBehaviour
{
    public int companyNumber;
    void Start()
    {
        UnitSelections.Instance.unitList.Add(this.gameObject);
    }


    private void OnDestroy()
    {
        UnitSelections.Instance.unitList.Remove(this.gameObject);
    }
}
