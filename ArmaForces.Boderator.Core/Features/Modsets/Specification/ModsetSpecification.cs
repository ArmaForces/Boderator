using System;
using System.Linq;
using ArmaForces.Boderator.Core.Infrastructure.Specifications;

namespace ArmaForces.Boderator.Core.Modsets.Specification;

public class ModsetSpecification : IBuildingSpecification<Modset>
{
    private ModsetSpecification() { }

    private string Name { get; init; } = string.Empty;
    
    public static IBuildingSpecification<Modset> Named(string modsetName)
    {
        return new ModsetSpecification
        {
            Name = modsetName
        };
    }

    public static IBuildingSpecification<Modset> ByUrl(Uri modsetUrl)
    {
        return new ModsetSpecification
        {
            Name = modsetUrl.Segments.Last()
        };
    }

    public Modset Build()
    {
        return new Modset
        {
            Name = Name
        };
    }
}