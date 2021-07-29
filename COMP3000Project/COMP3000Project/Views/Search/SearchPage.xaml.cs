using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLToolkit.Forms.SwipeCardView.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        SearchPageViewModel viewModel;
        public SearchPage(string location, string time, string[] eateryTypes)//start new search
        {
            InitializeComponent();

            viewModel = new SearchPageViewModel(location, time, eateryTypes);
            BindingContext = viewModel;
            SwipeCardView.Dragging += OnDragging;
        }

        public SearchPage(string SearchCode)//join existing search
        {
            InitializeComponent();

            viewModel = new SearchPageViewModel(SearchCode);
            BindingContext = viewModel;
            SwipeCardView.Dragging += OnDragging;
        }


        private void OnDragging(object sender, DraggingCardEventArgs e)
        {
            var view = (Xamarin.Forms.View)sender;
            var nopeFrame = view.FindByName<Frame>("NopeFrame");
            var likeFrame = view.FindByName<Frame>("LikeFrame");
            var superLikeFrame = view.FindByName<Frame>("SuperLikeFrame");

            var threshold = (BindingContext as SearchPageViewModel).Threshold;

            var draggedXPercent = e.DistanceDraggedX / threshold;

            var draggedYPercent = e.DistanceDraggedY / threshold;

            switch (e.Position)
            {
                case DraggingCardPosition.Start:
                    nopeFrame.Opacity = 0;
                    likeFrame.Opacity = 0;
                    superLikeFrame.Opacity = 0;
                    break;

                case DraggingCardPosition.UnderThreshold:
                    if (e.Direction == SwipeCardDirection.Left)
                    {
                        nopeFrame.Opacity = (-1) * draggedXPercent;
                        superLikeFrame.Opacity = 0;
                    }
                    else if (e.Direction == SwipeCardDirection.Right)
                    {
                        likeFrame.Opacity = draggedXPercent;
                        superLikeFrame.Opacity = 0;
                    }
                    else if (e.Direction == SwipeCardDirection.Up)
                    {
                        nopeFrame.Opacity = 0;
                        likeFrame.Opacity = 0;
                        superLikeFrame.Opacity = (-1) * draggedYPercent;
                    }
                    break;

                case DraggingCardPosition.OverThreshold:
                    if (e.Direction == SwipeCardDirection.Left)
                    {
                        nopeFrame.Opacity = 1;
                        superLikeFrame.Opacity = 0;
                    }
                    else if (e.Direction == SwipeCardDirection.Right)
                    {
                        likeFrame.Opacity = 1;
                        superLikeFrame.Opacity = 0;
                    }
                    else if (e.Direction == SwipeCardDirection.Up)
                    {
                        nopeFrame.Opacity = 0;
                        likeFrame.Opacity = 0;
                        superLikeFrame.Opacity = 1;
                    }
                    break;

                case DraggingCardPosition.FinishedUnderThreshold:
                    nopeFrame.Opacity = 0;
                    likeFrame.Opacity = 0;
                    superLikeFrame.Opacity = 0;
                    break;

                case DraggingCardPosition.FinishedOverThreshold:
                    nopeFrame.Opacity = 0;
                    likeFrame.Opacity = 0;
                    superLikeFrame.Opacity = 0;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}