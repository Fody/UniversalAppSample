using PropertyChanged;

[ImplementPropertyChanged]
public class PersonViewModel
{
    public string GivenNames;
    public string FamilyName;

    public string FullName => GivenNames +" " + FamilyName;
}
