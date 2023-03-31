using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ArmaForces.Boderator.Core.Missions.Models;

namespace ArmaForces.Boderator.Core.Common.Specifications;

public class MissionQuerySpecification : BaseQuerySpecification<Mission>
{
    public MissionQuerySpecification(DateTime? after = null, DateTime? before = null, string? owner = null)
     : base(x => IsMissionAfter(x, after) && IsMissionBefore(x, after) && IsMissionBy(x, owner))
    { }

    /// <summary>
    /// If a date was given, checks if mission is after that date. Returns true for missions without date.
    /// </summary>
    private static bool IsMissionAfter(Mission mission, DateTime? after)
        => after is null || mission.MissionDate is null || mission.MissionDate > after;

    /// <summary>
    /// If a date was given, checks if mission is before that date.
    /// </summary>
    private static bool IsMissionBefore(Mission mission, DateTime? before)
        => before is null || mission.MissionDate is not null && mission.MissionDate < before;

    /// <summary>
    /// If an owner was given, checks if mission is created by given user.
    /// </summary>
    private static bool IsMissionBy(Mission mission, string? owner)
        => owner is null || mission.Owner == owner;
}

public abstract class BaseQuerySpecification<T> : IQuerySpecification<T>
{
    protected BaseQuerySpecification() { }

    protected BaseQuerySpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }
    public Expression<Func<T, bool>>? Criteria { get; }
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
    
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }
}