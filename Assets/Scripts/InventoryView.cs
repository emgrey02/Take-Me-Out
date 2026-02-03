using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public InventoryViewModel ViewModel;

    public GameObject InventoryMenu;

    void Start()
    {
        //ViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    //private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    //{
     //   if (e.PropertyName == nameof(ViewModel.Inventory))
    //    {
    //        // update visibility of each card on panel
     //   }
    //}

    public void CardGameObjectClicked(int index)
    {
        ViewModel.OnAddToInventory(index);
    }
}
