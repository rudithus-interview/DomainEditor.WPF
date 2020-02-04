using System.Windows;
using InterviewAssessment.App.ViewModels;
using Microsoft.Xaml.Behaviors.Layout;

namespace InterviewAssessment.App.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddEntity_Click(object sender, RoutedEventArgs e)
        {
            var popup = new AddEntityDialog(EditorCanvas.RenderSize);
            popup.ShowDialog();
        }

        private void MouseDragElementBehavior_DragFinished(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.Source is Entity entity &&
                sender is MouseDragElementBehavior behavior &&
                !double.IsNaN(behavior.X) &&
                !double.IsNaN(behavior.Y))
            {
                var windowCoordinates = new Point(behavior.X, behavior.Y);
                var screenCoordinates = PointToScreen(windowCoordinates);
                var parentCoordinates = EditorCanvas.PointFromScreen(screenCoordinates);

                var viewModel = entity.DataContext as EntityViewModel;
                viewModel.X = (int)parentCoordinates.X;

                viewModel.Y = (int)parentCoordinates.Y;
            }
        }
    }
}
