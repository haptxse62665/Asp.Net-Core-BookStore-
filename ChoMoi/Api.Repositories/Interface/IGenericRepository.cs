using Api.Models;
using DemoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        //
        // Summary:
        //     Filters a sequence of values based on a predicate.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to filter.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Linq.IQueryable`1 that contains elements from the input sequence that
        //     satisfy the condition specified by predicate.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        //
        // Summary:
        //     Begins tracking the given entity, and any other reachable entities that are not
        //     already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state such that they will be inserted into the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //     Use Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State to set the
        //     state of only a single entity.
        //
        // Parameters:
        //   entity:
        //     The entity to add.
        //
        // Returns:
        //     The Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1 for the entity.
        //     The entry provides access to change tracking information and operations for the
        //     entity.
        T Add(T entity);
        //
        // Summary:
        //     Begins tracking the given entity in the Microsoft.EntityFrameworkCore.EntityState.Deleted
        //     state such that it will be removed from the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //
        // Parameters:
        //   entity:
        //     The entity to remove.
        //
        // Returns:
        //     The Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1 for the entity.
        //     The entry provides access to change tracking information and operations for the
        //     entity.
        //
        // Remarks:
        //     If the entity is already tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state then the context will stop tracking the entity (rather than marking it
        //     as Microsoft.EntityFrameworkCore.EntityState.Deleted) since the entity was previously
        //     added to the context and does not exist in the database.
        //     Any other reachable entities that are not already being tracked will be tracked
        //     in the same way that they would be if Microsoft.EntityFrameworkCore.DbSet`1.Attach(`0)
        //     was called before calling this method. This allows any cascading actions to be
        //     applied when Microsoft.EntityFrameworkCore.DbContext.SaveChanges is called.
        //     Use Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State to set the
        //     state of only a single entity.
        T Delete(T entity);

        BookStoreContext GetDataBase();

        void Edit(T entity);
        //
        // Summary:
        //     Saves all changes made in this context to the database.
        //
        // Returns:
        //     The number of state entries written to the database.
        //
        // Exceptions:
        //   T:Microsoft.EntityFrameworkCore.DbUpdateException:
        //     An error is encountered while saving to the database.
        //
        //   T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException:
        //     A concurrency violation is encountered while saving to the database. A concurrency
        //     violation occurs when an unexpected number of rows are affected during save.
        //     This is usually because the data in the database has been modified since it was
        //     loaded into memory.
        //
        // Remarks:
        //     This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges
        //     to discover any changes to entity instances before saving to the underlying database.
        //     This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
        int Save();
        //
        // Summary:
        //     Returns the first element of a sequence that satisfies a specified condition
        //     or a default value if no such element is found.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return an element from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     default(TSource) if source is empty or if no element passes the test specified
        //     by predicate; otherwise, the first element in source that passes the test specified
        //     by predicate.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        //
        // Summary:
        //     Returns the first element of a sequence that satisfies a specified condition.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return an element from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The first element in source that passes the test in predicate.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        //
        //   T:System.InvalidOperationException:
        //     No element satisfies the condition in predicate. -or- The source sequence is
        //     empty.
        T First(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Determines whether any element of a sequence satisfies a condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>true if any elements in the source sequence pass the test in the specified predicate;otherwise, false.</returns>
        /// <exception>T:System.ArgumentNullException: source or predicate is null.</exception>
        bool Any(Expression<Func<T, bool>> predicate);
        bool All(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindByWithQueryable(Expression<Func<T, bool>> predicate);
        //
        // Summary:
        //     Returns the number of elements in the specified sequence that satisfies a condition.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 that contains the elements to be counted.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The number of elements in the sequence that satisfies the condition in the predicate
        //     function.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        //
        //   T:System.OverflowException:
        //     The number of elements in source is larger than System.Int32.MaxValue.
        int Count(Expression<Func<T, bool>> predicate);
    }
}
