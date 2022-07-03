namespace ArmaForces.Boderator.Core.Infrastructure.Specifications;

public interface IBuildingSpecification<out T>
{
    T Build();
}
