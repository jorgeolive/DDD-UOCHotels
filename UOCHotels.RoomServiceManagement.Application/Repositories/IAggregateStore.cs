using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Application.Repositories
{
    public interface IAggregateStore
    {
        Task<bool> Exists<TAggregate, TId>(TId aggregateId) where TAggregate : AggregateRoot<TId> where TId : ValueObject<TId>;
        Task Save<TAggregate, TId>(TAggregate aggregate) where TAggregate : AggregateRoot<TId> where TId : ValueObject<TId>;
        Task<AggregateRoot<TId>> Load<TAggregate, TId>(TId aggregate) where TAggregate : AggregateRoot<TId> where TId : ValueObject<TId>;
    }
}
