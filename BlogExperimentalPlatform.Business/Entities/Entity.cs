namespace BlogExperimentalPlatform.Business.Entities
{
    using System;

    public abstract class Entity : IEquatable<Entity>
    {
        public int Id { get; set; }

        // Soft delete
        public bool Deleted { get; set; }

        #region IEquatable
        public bool Equals(Entity other)
        {
            if (Id == other.Id)
                return true;

            return false;
        }
        #endregion
    }
}
