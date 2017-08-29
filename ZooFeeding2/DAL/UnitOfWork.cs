using System;
using ZooFeeding2.Models;

namespace ZooFeeding2.DAL
{
    public class UnitOfWork : IDisposable
    {
        private ZooContext context = new ZooContext();
        private GenericRepository<Animal> animalRepository;
        private GenericRepository<Food> foodRepository;
        private GenericRepository<Relationship> relationshipRepository;

        public GenericRepository<Animal> AnimalRepository
        {
            get
            {
                if (this.animalRepository == null)
                {
                    this.animalRepository = new GenericRepository<Animal>(context);
                }
                return animalRepository;
            }
        }

        public GenericRepository<Food> FoodRepository
        {
            get
            {
                if (this.foodRepository == null)
                {
                    this.foodRepository = new GenericRepository<Food>(context);
                }
                return foodRepository;
            }
        }

        public GenericRepository<Relationship> RelationshipRepository
        {
            get
            {
                if (this.relationshipRepository == null)
                {
                    this.relationshipRepository = new GenericRepository<Relationship>(context);
                }
                return relationshipRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}