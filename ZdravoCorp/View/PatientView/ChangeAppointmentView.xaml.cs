using System.Windows;

namespace ZdravoCorp.View.PatientV;

/// <summary>
///     Interaction logic for ChangeAppointmentView.xaml
/// </summary>
public partial class ChangeAppointmentView : Window
{
    public ChangeAppointmentView()
    {
        //ChangeAppointmentViewModel CAVM = new ChangeAppointmentViewModel(appointmentViewModel ,drRepository.GetAll(), scheduleRepository, Appointments, drRepository, patient);
        //DataContext = CAVM;
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}