This is walk through the complexity in using Fody inside a Windows Universal project. The approach used can be applied to any project that uses the new project.json approach. 


## The Solution

This repository contains two projects. 


### AppWithoutFody

A project that does not use Fody and implement `INotifyPropertyChanged` manually.


### AppWithFody

A "finished" project that does already utilizes use Fody, and the weavers PropertyChanged and Fielder, to simplify the PersonViewModel.


## Upgrading AppWithoutFody

We will now walk through converting AppWithoutFody to use Fody


### 1. Install Fody

Run the following on the Package Manager Console

    install-package Fody

You want to install the Fody package first since package dependency resolution in nuget defaults to the oldest version. Hence if u install a weaver first you will get a really old version of Fody instead of the newest (desired) version.

Usually when installing Fody the `FodyWeavers.xml` is deployed via the [Content feature of nuget](https://docs.nuget.org/create/nuspec-reference#content-files). However in NuGet 3.1 the Content feature was [deprecated for projects using the project.json approach](http://blog.nuget.org/20150729/Introducing-nuget-uwp.html).

Hence you will need to manually add this `FodyWeavers.xml` file to your project.

```
<?xml version="1.0" encoding="utf-8" ?>
<Weavers>
</Weavers>
``` 


### 2. Install some weaver nugets

Run the following on the Package Manager Console

    install-package Fielder.Fody
    install-package PropertyChanged.Fody

Usually when you install a Fody addin it will leverage either the [install.ps1](https://docs.nuget.org/create/creating-and-publishing-a-package#automatically-running-powershell-scripts-during-package-installation-and-removal) or the [xdt](https://docs.nuget.org/create/configuration-file-and-source-code-transformations) features of nuget to inject itself into the `FodyWeavers.xml`. However in NuGet 3.1 support for [xdt and install.ps1 was deprecated for projects using the `project.json` approach](http://blog.nuget.org/20150729/Introducing-nuget-uwp.html).

Hence if you build you will get this error

```
No configured weavers. 
It is possible you have not installed a weaver or have installed a fody weaver nuget into a project type that does not support install.ps1. 
You may need to add that weaver to FodyWeavers.xml manually. 
eg. <Weavers><WeaverName/></Weavers>. 
see https://github.com/Fody/Fody/wiki/SampleUsage
```

This is because Fody is trying to tell you something is not configured correctly since it cant find any weavers.


### 3. Add weavers to xml

So initially add just `<Fielder/>`

```
<?xml version="1.0" encoding="utf-8" ?>
<Weavers>
  <Fielder/>
</Weavers>
```

Now build again

You will get the following error

```
PropertyChanged expected to be executed. 
You may also need to manually add '<PropertyChanged />' into your FodyWeavers.xml. 
eg <Weavers><PropertyChanged/></Weavers>. See https://github.com/Fody/Fody/wiki/SampleUsage	
```

This is because while the XDT and install.ps1 was deprecated the [import msbuild targets feature](https://docs.nuget.org/create/creating-and-publishing-a-package#import-msbuild-targets-and-props-files-into-project) still works. So for project.json projects a fody weaver can plug into the msbuild pipeline and ensure that weaving has occurred.   


```
<?xml version="1.0" encoding="utf-8" ?>
<Weavers>
  <Fielder/>
  <PropertyChanged/>
</Weavers>
```


### 4. Simplify the view model

You can now convert the view model over


#### Before

```

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
```


#### After

```
[ImplementPropertyChanged]
public class PersonViewModel
{
    public string GivenNames;
    public string FamilyName;
    public string FullName => GivenNames +" " + FamilyName;
}
```


## Summary

The above might seem complex but in reality you just need to remember two things. 

1. Manually add a FodyWeavers.xml when you install Fody
2. Manually add a weavers name to FodyWeavers.xml when you install it. 


## But cant you make it simpler

Unfortunately without any extension points exposed by Nuget it is not possible to simplify this process. Feel free to ask the nuget guys to add either xdt or isntall.ps1 suppor to project.json projects. 