using System.Windows;
using System.Windows.Controls;

namespace ZdravoCorp.View;

/// <summary>
///     Interaction logic for MakeAppointmentView.xaml
/// </summary>
public partial class MakeAppointmentView : Window
{
    //private DoctorRepository _doctorRepository;
    public MakeAppointmentView()
    {
        //MakeAppointmentViewModel MAV = new MakeAppointmentViewModel(drRepository.GetAll(), scheduleRepository, Appointments, drRepository, patient);
        //DataContext = MAV;
        InitializeComponent();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}