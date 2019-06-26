using Api.Repositories;
using ChoMoi.Api.Repositories.Interface;
using DemoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoMoi.Api.Repositories.Implement
{
    public class PublisherRepository : GenericRepository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(BookStoreContext bookStoreContext)
          : base(bookStoreContext)
        {
        }
    }
}
