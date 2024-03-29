﻿using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.Buildings;

public enum BuildingResourceCategory
{
    Liquid,
    Solid,
    Gaseous
}

public static class BuildingResourceCategoryExtensions
{
    /**
     * Returns a list of resource kinds that can be produced by a building of this category, sorted by rarity.
     */
    public static List<ResourceKind> GetResourcesKindByCategory(this BuildingResourceCategory category)
    {
        return category switch
        {
            BuildingResourceCategory.Liquid => new List<ResourceKind>
            {
                ResourceKind.Water
            },
            BuildingResourceCategory.Solid => new List<ResourceKind>
            {
                ResourceKind.Titanium,
                ResourceKind.Gold,
                ResourceKind.Aluminium,
                ResourceKind.Iron,
                ResourceKind.Carbon
            },
            BuildingResourceCategory.Gaseous => new List<ResourceKind>
            {
                ResourceKind.Oxygen
            },
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}