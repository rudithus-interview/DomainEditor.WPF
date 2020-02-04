using System.Collections.ObjectModel;
using System.Windows;
using InterviewAssessment.App.ViewModels;

namespace InterviewAssessment.App.Views
{
    /// <summary>
    /// Interaction logic for EntityAttributesPanel.xaml
    /// </summary>
    public partial class EntityAttributesPanel : Window
    {
        public EntityAttributesPanel(EntityAttributesPanelViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}