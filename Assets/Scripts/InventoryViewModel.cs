using UnityEngine;
using System.ComponentModel;

public class InventoryViewModel: INotifyPropertyChanged
{
    private readonly InventoryModel _model;

    public InventoryViewModel(InventoryModel model)
    {
        _model = model;
        _model.OnInventoryUpdated += () => {
            OnPropertyChanged(nameof(Inventory));
        };
    }

    // Expose data for the UI
    public bool[] Inventory => _model.Inventory;

    // Trigger Model actions and notify the UI
    public void OnAddToInventory(int index)
    {
        _model.AddToInventory(index);
        OnPropertyChanged(nameof(Inventory));
    }

    // INotifyPropertyChanged implementation

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

