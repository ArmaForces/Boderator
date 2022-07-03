using System;
using ArmaForces.Boderator.Core.Infrastructure.Specifications;

namespace ArmaForces.Boderator.Core.Modsets.Specification;

public class ModsetSpecification : IBuildingSpecification<Modset>
{
    private ModsetSpecification() { }

    public static IBuildingSpecification<Modset> ByName(string modsetName)
    {
        return new ModsetSpecification();
    }

    public static IBuildingSpecification<Modset> ByUrl(Uri modsetUrl)
    {
        return new ModsetSpecification();
    }

    public Modset Build()
    {
        return new Modset();
    }
}