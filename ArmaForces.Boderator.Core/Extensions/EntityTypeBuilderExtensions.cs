using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArmaForces.Boderator.Core.Extensions;

internal static class EntityTypeBuilderExtensions
{
    public static PropertyBuilder<T> IsNotRequired<T>(this PropertyBuilder<T> propertyBuilder)
        => propertyBuilder.IsRequired(false);

    public static ReferenceCollectionBuilder<T1, T2> IsNotRequired<T1, T2>(this ReferenceCollectionBuilder<T1, T2> builder)
        where T1 : class
        where T2 : class
        => builder.IsRequired(false);
}
