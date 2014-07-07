using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotesDomain.Entities.OneToOne
{
    public class FriendStudentMapping :BaseEntity
    {
         public int Id { get; set; }
         public virtual Student Student { get; set; }
    }
}
