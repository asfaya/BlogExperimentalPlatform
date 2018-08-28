namespace BlogExperimentalPlatform.Web.DTOs
{
    using System;

    public abstract class EntityDTO : IEquatable<EntityDTO>
    {
        public int Id { get; set; }

        // Soft delete
        public bool Deleted { get; set; }

        #region IEquatable
        public bool Equals(EntityDTO other)
        {
            if (Id == other.Id)
                return true;

            return false;
        }
        #endregion
    }
}
