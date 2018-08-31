namespace BlogExperimentalPlatform.Business.Entities
{
    using System.Collections.Generic;

    public class EntityPage<T>
        where T : Entity
    {
        private int totalFiltered = 0;

        public ICollection<T> Entities { get; set; }

        public int TotalEntities { get; set; }

        public int TotalPages { get; set; }

        public int TotalFiltered { get => totalFiltered; set => totalFiltered = value; }
    }
}
