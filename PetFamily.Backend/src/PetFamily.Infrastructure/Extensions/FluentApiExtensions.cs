using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Infrastructure.Extensions;

public static class FluentApiExtensions
{
    public static EntityTypeBuilder<TEntity> ValueObjectListToJson<TEntity, TValueEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, ValueObjectList<TValueEntity>?>> navigationExpression,
        Action<OwnedNavigationBuilder<ValueObjectList<TValueEntity>, TValueEntity>> buildAction,
        string? columnName
    )
        where TEntity : class
        where TValueEntity : class
    {
        return builder.OwnsOne(navigationExpression, navigationBuilder =>
        {
            navigationBuilder.ToJson(columnName);

            navigationBuilder.OwnsMany(x => x.Values, buildAction);
        });
    }
}