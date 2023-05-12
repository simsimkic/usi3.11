using ZdravoCorp.Core.Models.AnamnesisReport;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class FullAnamnesisViewModel : ViewModelBase
{
    private readonly Anamnesis _anamnesis;

    public FullAnamnesisViewModel(Anamnesis anamnesis)
    {
        _anamnesis = anamnesis;
    }

    public string KeyWord => _anamnesis.KeyWord;
    public string Opinion => _anamnesis.Opinion;
    public string Symptoms => _anamnesis.SymptomsToString();
    public string Allergens => _anamnesis.AllergensToString();
}