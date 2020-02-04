using System.Windows;
using InterviewAssessment.App.ViewModels;

namespace InterviewAssessment.App.Views
{
    /// <summary>
    /// Interaction logic for NamePrompt.xaml
    /// </summary>
    public partial class AddEntityDialog : Window
    {
        public AddEntityDialog(Size canvasSize)
        {
            InitializeComponent();

            var viewModel = DataContext as AddEntityDialogViewModel;
            viewModel.MaximumY = canvasSize.Height - 50;
            viewModel.MaximumX = canvasSize.Width - 80;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
