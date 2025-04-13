namespace imdbdrinks_ratingsmodule;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using imdbdrinks_ratingsmodule.Assets;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

public sealed partial class RatingWindow : Window, INotifyPropertyChanged
{
    public int RatingScore { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<Bottle> Bottles { get; set; }

    private readonly RatingViewModel ratingViewModel;

    private const int MinimumRatingScore = 1;
    private const int MaximumRatingScore = 5;
    private const int BottleRatingToIndexOffset = 1;
    private const int PlaceholderItemId = 100;


    public RatingWindow(RatingViewModel viewModel)
    {
        this.InitializeComponent();

        this.Bottles = new ObservableCollection<Bottle>();
        foreach (var currentRating in Enumerable.Range(MinimumRatingScore, MaximumRatingScore))
        {
            var bottleToAdd = new Bottle { ImageSource = AssetConstants.EmptyBottlePath };
            this.Bottles.Add(bottleToAdd);
        }

        this.rootGrid.DataContext = this;
        this.ratingViewModel = viewModel;
    }

    private void Bottle_Click(object sender, TappedRoutedEventArgs e)
    {
        if (sender is not Image clickedImage)
            return;

        if (clickedImage.DataContext is not Bottle clickedBottle)
            return;

        int clickedBottleNumber = Bottles.IndexOf(clickedBottle);

        foreach (var currentRatingBottle in Enumerable.Range(MinimumRatingScore, clickedBottleNumber))
        {
            var bottleIndex = currentRatingBottle - BottleRatingToIndexOffset;

            if (bottleIndex <= clickedBottleNumber)
                Bottles[bottleIndex].ImageSource = AssetConstants.FilledBottlePath;
            else
                Bottles[bottleIndex].ImageSource = AssetConstants.EmptyBottlePath;
        }

        RatingScore = clickedBottleNumber + BottleRatingToIndexOffset;
    }

    private void RateButton_Click(object sender, RoutedEventArgs e)
    {
        if (RatingScore == MinimumRatingScore)
            return;

        ratingViewModel.AddRating(PlaceholderItemId, RatingScore);

        this.Close();
    }

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
}
