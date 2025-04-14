using System.ComponentModel;

namespace imdbdrinks_ratingsmodule.ViewHelpers;

public class BottleAsset : INotifyPropertyChanged
{
    private string bottleImageSource;

    public string ImageSource
    {
        get => bottleImageSource;
        set
        {
            bottleImageSource = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSource)));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
}