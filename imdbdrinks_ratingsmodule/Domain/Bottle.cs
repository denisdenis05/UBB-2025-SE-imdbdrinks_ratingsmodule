namespace imdbdrinks_ratingsmodule.Domain;

using System.ComponentModel;

public class Bottle : INotifyPropertyChanged
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