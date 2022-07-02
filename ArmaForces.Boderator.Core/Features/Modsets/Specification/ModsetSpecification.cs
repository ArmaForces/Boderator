using System;

namespace ArmaForces.Boderator.Core.Modsets.Specification;

public class ModsetSpecification : IBuildingModsetSpecification
{
    private ModsetSpecification() { }

    public static IBuildingModsetSpecification ByName(string modsetName)
    {
        return new ModsetSpecification();
    }

    public static IBuildingModsetSpecification ByUrl(Uri modsetUrl)
    {
        return new ModsetSpecification();
    }

    public Modset Build()
    {
        return new Modset();
    }
}