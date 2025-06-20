﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace Service
{
    using DocumentFormat.OpenXml.Vml.Office;
    using EF;
    using EF.Models;
    using global::Service.Dto.Transaction;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    namespace Service
    {
        public abstract class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto> where TEntity : class where TDto : class
        {
            protected readonly ISenProContext _context;
            protected readonly DbSet<TEntity> _dbSet;

            protected readonly IUserContext _userContext;

            public BaseService(ISenProContext context)
            {
                _context = context;
                _dbSet = _context.Set<TEntity>();
            }

            public BaseService(ISenProContext context, IUserContext userContext)
            {
                _context = context;
                _dbSet = _context.Set<TEntity>();
                _userContext = userContext;
            }


            public virtual async Task<IEnumerable<TDto>> GetAllAsync()
            {
                var query = IncludeNavigationProperties(_dbSet);
                var entities = await query.Where(e => EF.Property<bool>(e, "IsActive")).ToListAsync();
                return entities.Select(MapToDto).ToList();
            }

            public virtual async Task<IEnumerable<TDto>> GetAllWithInActiveAsync()
            {
                var entities = await _dbSet.ToListAsync();
                return entities.Select(MapToDto).ToList();
            }

            public virtual async Task<TDto> GetByIdAsync(int id)
            {
                var entity = await _dbSet.FindAsync(id);
                return entity != null ? MapToDto(entity) : null;
            }

            public virtual async Task<TDto> GetByIdAsync(string id)
            {
                var entity = await _dbSet.FindAsync(id);
                return entity != null ? MapToDto(entity) : null;
            }

            public virtual async Task<object> AddAsync(TDto dto)
            {
                var entity = MapToEntity(dto);

                // Track child entities generically
                TrackChildEntities(entity);

                // Add the parent entity
                var result = await _dbSet.AddAsync(entity);

                // Save changes for both parent and child entities
                await _context.SaveChangesAsync();

                return result.Entity;
            }

            public virtual async Task UpdateAsync(TDto dto)
            {
                var entity = MapToEntity(dto);

                // Track child entities generically
                TrackChildEntities(entity);

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }

            /// <summary>
            /// Generic method to track child entities and ensure they are marked as Added.
            /// </summary>
            /// <param name="entity">The parent entity containing child collections.</param>
            private void TrackChildEntities(TEntity entity)
            {
                var context = _context; // DbContext reference

                // Use reflection to find all navigation properties that are collections
                var collectionProperties = entity.GetType().GetProperties()
                    .Where(p => typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType)
                                && p.PropertyType != typeof(string));

                foreach (var property in collectionProperties)
                {
                    var childEntities = property.GetValue(entity) as System.Collections.IEnumerable;

                    if (childEntities == null) continue;

                    foreach (var childEntity in childEntities)
                    {
                        // Safely retrieve the entity type
                        var entityType = context.Model.FindEntityType(childEntity.GetType());
                        if (entityType == null) continue; // Skip if entity type is not mapped

                        // Safely retrieve the primary key
                        var keyProperties = entityType.FindPrimaryKey()?.Properties;
                        if (keyProperties == null) continue; // Skip if no primary key is defined

                        // Check if the child entity is new
                        var isNew = keyProperties.All(kp =>
                        {
                            var keyValue = childEntity.GetType().GetProperty(kp.Name)?.GetValue(childEntity);
                            return keyValue == null || (int.TryParse(keyValue.ToString(), out int id) && id == 0);
                        });

                        // Mark entity state accordingly
                        var entry = context.Entry(childEntity);
                        if (entry.State == EntityState.Detached)
                        {
                            context.Entry(childEntity).State = isNew ? EntityState.Added : EntityState.Modified;
                        }
                    }
                }
            }

            public virtual async Task DeleteAsync(string id)
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _context.Entry(entity).Property("IsActive").CurrentValue = false;
                    await _context.SaveChangesAsync();
                }
            }

            public virtual async Task DeleteAsync(int id)
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _context.Entry(entity).Property("IsActive").CurrentValue = false;
                    await _context.SaveChangesAsync();
                }
            }


            public virtual IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> query, List<Filter> filters)
            {
                return query;
            }

            public virtual IQueryable<TDynamic> ApplyDynamicFilters<TDynamic>(IQueryable<TDynamic> query, List<Filter> filters) where TDynamic : class
            {

                if (filters != null && filters.Any())
                {
                    foreach (var filter in filters)
                    {
                        if (filter.FilterOptions != null && filter.FilterOptions.Any())
                        {
                            // Create parameter expression
                            var parameter = Expression.Parameter(typeof(TDynamic), "p");

                            // Start with false expression
                            Expression combinedExpression = Expression.Constant(false);

                            foreach (var option in filter.FilterOptions)
                            {
                                if (filter.FilterName.ToLower() == "requestingoffice")
                                {
                                    // Get property (handles nullable properties)
                                    var property = Expression.Property(parameter, "RequestingOfficeId");

                                    // Convert value to the property's type
                                    var propertyType = property.Type;
                                    var convertedValue = Convert.ChangeType(option.Value,
                                        Nullable.GetUnderlyingType(propertyType) ?? propertyType);
                                    var constant = Expression.Constant(convertedValue, propertyType);

                                    // Create equality expression that handles nullables
                                    Expression equality;

                                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        // For nullable types, we need to handle null comparison
                                        var hasValue = Expression.Property(property, "HasValue");
                                        var value = Expression.Property(property, "Value");
                                        equality = Expression.AndAlso(
                                            hasValue,
                                            Expression.Equal(value, Expression.Convert(constant, value.Type))
                                        );
                                    }
                                    else
                                    {
                                        equality = Expression.Equal(property, constant);
                                    }

                                    // Combine with OR
                                    combinedExpression = Expression.OrElse(combinedExpression, equality);
                                }
                            }

                            // Create the lambda expression
                            var lambda = Expression.Lambda<Func<TDynamic, bool>>(combinedExpression, parameter);

                            // Apply the condition to the query
                            query = query.Where(lambda);
                        }
                    }
                }
                return query;
            }


            public virtual async Task<(IEnumerable<TDto> Data, int TotalRecords)> GetPagedAndFilteredAsync(PagingParameters pagingParameters)
            {
                var query = IncludeNavigationProperties(_dbSet).Where(e => EF.Property<bool>(e, "IsActive"));

                if (pagingParameters.Filters != null && pagingParameters.Filters.Any())
                {
                    // Apply dynamic filters
                    query = ApplyFilters(query, pagingParameters.Filters);
                }

                if (!string.IsNullOrEmpty(pagingParameters.SearchQuery))
                {
                    query = ApplySearchFilter(query, pagingParameters.SearchQuery);
                }

                // Apply server-side sorting if sort instructions are provided
                if (pagingParameters.SortBy != null && pagingParameters.SortBy.Any())
                {
                    bool firstSort = true;
                    foreach (var sort in pagingParameters.SortBy)
                    {
                        // Use reflection to find the correct property name, ignoring case.
                        var propertyInfo = typeof(TEntity)
                            .GetProperties()
                            .FirstOrDefault(pi => string.Equals(pi.Name, sort.Id, StringComparison.OrdinalIgnoreCase));

                        if (propertyInfo == null)
                        {
                            // If no matching property is found, you might choose to skip this sort instruction.
                            continue;
                        }

                        // Use the correct property name from the entity.
                        string propertyName = propertyInfo.Name;

                        if (firstSort)
                        {
                            query = sort.Desc
                                ? query.OrderByDescending(e => EF.Property<object>(e, propertyName))
                                : query.OrderBy(e => EF.Property<object>(e, propertyName));
                            firstSort = false;
                        }
                        else
                        {
                            var orderedQuery = query as IOrderedQueryable<TEntity>;
                            query = sort.Desc
                                ? orderedQuery.ThenByDescending(e => EF.Property<object>(e, propertyName))
                                : orderedQuery.ThenBy(e => EF.Property<object>(e, propertyName));
                        }
                    }
                }
                //  else apply default sorting
                else
                {
                    query = query.OrderByDescending(e => EF.Property<object>(e, "CreatedDate"));
                }

                var totalRecords = await query.CountAsync();
                var data = await query.Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                                      .Take(pagingParameters.PageSize)
                                      .ToListAsync();

                var dtoData = data.Select(MapToDto).ToList();
                return (dtoData, totalRecords);
            }

            /// <summary>
            /// Used for Index page with complex data structure
            /// </summary>
            /// <param name="pagingParameters"></param>
            /// <returns></returns>
            public virtual async Task<(IEnumerable<TDynamic> Data, int TotalRecords)> GetComplexPagedAndFilteredAsync<TDynamic>(PagingParameters pagingParameters)
                 where TDynamic : class
            {
                var query = _context.Set<TDynamic>().Where(e => EF.Property<bool>(e, "IsActive")); ;

                // apply filter criteria,
                if (pagingParameters.ApplyFilterCriteria)
                {
                    query = ApplyFilterCriteria(query);
                }

                if (pagingParameters.Filters != null && pagingParameters.Filters.Any())
                {
                    // Apply dynamic filters
                    query = ApplyDynamicFilters<TDynamic>(query, pagingParameters.Filters);
                }

                if (!string.IsNullOrEmpty(pagingParameters.SearchQuery))
                {
                    query = ApplySearchFilter(query, pagingParameters.SearchQuery);// ApplyDynamicSearchFilter<TDynamic>(query, pagingParameters.SearchQuery);
                }

                // Apply server-side sorting if sort instructions are provided
                if (pagingParameters.SortBy != null && pagingParameters.SortBy.Any())
                {
                    bool firstSort = true;
                    foreach (var sort in pagingParameters.SortBy)
                    {
                        // Use reflection to find the correct property name, ignoring case.
                        var propertyInfo = typeof(TDynamic)
                            .GetProperties()
                            .FirstOrDefault(pi => string.Equals(pi.Name, sort.Id, StringComparison.OrdinalIgnoreCase));

                        if (propertyInfo == null)
                        {
                            // If no matching property is found, you might choose to skip this sort instruction.
                            continue;
                        }

                        // Use the correct property name from the entity.
                        string propertyName = propertyInfo.Name;

                        if (firstSort)
                        {
                            query = sort.Desc
                                ? query.OrderByDescending(e => EF.Property<object>(e, propertyName))
                                : query.OrderBy(e => EF.Property<object>(e, propertyName));
                            firstSort = false;
                        }
                        else
                        {
                            var orderedQuery = query as IOrderedQueryable<TDynamic>;
                            query = sort.Desc
                                ? orderedQuery.ThenByDescending(e => EF.Property<object>(e, propertyName))
                                : orderedQuery.ThenBy(e => EF.Property<object>(e, propertyName));
                        }
                    }
                }
                //  else apply default sorting
                else
                {
                    query = query.OrderByDescending(e => EF.Property<object>(e, "CreatedDate"));
                }

                var totalRecords = await query.CountAsync();
                var data = await query.Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                                      .Take(pagingParameters.PageSize)
                                      .ToListAsync();

                //var dtoData = data.Select(MapToDto).ToList();
                return (data.ToList(), totalRecords);
            }


            /// <summary>
            /// Implement this on every service that needs to apply filter criteria
            /// </summary>
            /// <typeparam name="TDynamic"></typeparam>
            /// <param name="query"></param>
            /// <returns></returns>
            public virtual IQueryable<TDynamic> ApplyFilterCriteria<TDynamic>(IQueryable<TDynamic> query) where TDynamic : class
            {
                return query;
            }


            protected virtual IQueryable<TEntity> IncludeNavigationProperties(IQueryable<TEntity> query)
            {
                return query; // By default, no navigation properties are included.
            }

            // Helper method to combine expressions with OR logic
            protected Expression<Func<TEntity, bool>> CombineWithOr(
                Expression<Func<TEntity, bool>> firstCondition,
                Expression<Func<TEntity, bool>> secondCondition)
            {
                var parameter = Expression.Parameter(typeof(TEntity), "p");

                // Combine the two expressions using OR logic
                var body = Expression.OrElse(
                    Expression.Invoke(firstCondition, parameter),
                    Expression.Invoke(secondCondition, parameter)
                );

                return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
            }

            protected Expression<Func<TDynamic, bool>> CombineWithOrDynamic<TDynamic>(
               Expression<Func<TDynamic, bool>> firstCondition,
               Expression<Func<TDynamic, bool>> secondCondition) where TDynamic : class
            {
                var parameter = Expression.Parameter(typeof(TEntity), "p");

                // Combine the two expressions using OR logic
                var body = Expression.OrElse(
                    Expression.Invoke(firstCondition, parameter),
                    Expression.Invoke(secondCondition, parameter)
                );

                return Expression.Lambda<Func<TDynamic, bool>>(body, parameter);
            }



            protected virtual IQueryable<T> ApplySearchFilter<T>(IQueryable<T> query, string searchQuery) where T : class
            {
                return query; // By default, no search filter is applied.
            }

            protected virtual IQueryable<TEntity> ApplySearchFilter(IQueryable<TEntity> query, string searchQuery)
            {
                return query; // By default, no search filter is applied.
            }
            //protected abstract IQueryable<TEntity> ApplySearchFilter(IQueryable<TEntity> query, string searchQuery);

            protected virtual IQueryable<TDynamic> ApplyDynamicSearchFilter<TDynamic>(IQueryable<TDynamic> query, string searchQuery) where TDynamic : class
            {
                return query; // By default, no search filter is applied.
            }

            public virtual async Task<TransactionStatus> AddTransactionStatus(int moduleId, int id,
                UserTransactionPermissions userTransactionPermissions,
                TransactionStatusDto transactionStatusDto)
            {
                // get current transaction status
                var transactionStatus = await _context.TransactionStatuses
                    .Where(x => x.TransactionId == id && x.PageId == moduleId && x.IsActive)
                    .OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();

                var count = TransactionStatusDto.GetNextCount(userTransactionPermissions.WorkStepId, transactionStatus);
                var trn = new TransactionStatus
                {
                    //RequiredApprover = dto.TransactionStatus.RequiredApprover,
                    CreatedDate = DateTime.Now,
                    IsCurrent = true,// TBD - NO NEED FOR THIS
                    Count = count,
                    IsDone = userTransactionPermissions.RequiredApprover == count,
                    PageId = moduleId, // ModuleId
                    ProcessByUserId = _userContext.UserId,
                    Remarks = transactionStatusDto.Remarks,
                    Status = userTransactionPermissions.WorkStepName,
                    TransactionId = id,
                    WorkstepId = userTransactionPermissions.WorkStepId,
                    Action = transactionStatusDto.Approved ? "approved" : "disapproved",// TO DO - other actions
                    IsActive = true,
                };

                return trn;
            }

            public async Task DisableTransactionStatuses(int moduleId, int id)
            {
                var transactionStatus = await _context.TransactionStatuses
                   .Where(x => x.TransactionId == id && x.PageId == moduleId && x.IsActive).ToListAsync();

                foreach (var item in transactionStatus)
                {
                    item.IsActive = false;
                }

                await _context.SaveChangesAsync();
            }

            public virtual async Task<string> GetTransactionStatus(int workStepId, TransactionStatusDto transactionStatus)
            {
                var result = "";

                if (transactionStatus.Approved)
                {
                    var workStep = await _context.UmWorkSteps.FindAsync(workStepId);
                    if (workStep != null)
                    {
                        if (workStep.IsLastStep.GetValueOrDefault())
                        {
                            result = "approved";
                        }
                        else
                        {
                            result = "approval in progress";
                        }
                    }
                }
                else if (transactionStatus.Disapproved)
                {
                    result = "disapproved";
                    //entity.IsSubmitted = false;
                }

                return result;
            }

            // Mapping methods
            protected abstract TDto MapToDto(TEntity entity);
            protected abstract TEntity MapToEntity(TDto dto);
        }
    }

}
