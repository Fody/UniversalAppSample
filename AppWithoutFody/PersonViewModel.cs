
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class PersonViewModel : INotifyPropertyChanged
{
    string givenNames;
    string familyName;

    public string FullName => GivenNames + " " + FamilyName;

    public string FamilyName
    {
        get { return familyName; }
        set
        {
            familyName = value;
            OnPropertyChanged();
            OnPropertyChanged("FullName");
        }
    }

    public string GivenNames
    {
        get { return givenNames; }
        set
        {
            givenNames = value; 
            OnPropertyChanged();
            OnPropertyChanged("FullName");
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
