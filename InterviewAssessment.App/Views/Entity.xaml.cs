using System.Windows.Controls;
using InterviewAssessment.App.ViewModels;

namespace InterviewAssessment.App.Views
{
    /// <summary>
    /// Interaction logic for Entity.xaml
    /// </summary>
    public partial class Entity : UserControl
    {
        public Entity()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = DataContext as EntityViewModel;
            var attributesPanelViewModel = new EntityAttributesPanelViewModel(vm.Attributes);
            var popup = new EntityAttributesPanel(attributesPanelViewModel);
            popup.ShowDialog();
        }
    }
}
