using System;
using System.Collections.Generic;
using System.Linq;
using NotesDomain;
using NotesDomain.Entities;
using NotesDomainInterfaces;
using NotesManagerTransferEntities;
using Omu.ValueInjecter;

namespace NotesServiceLayer
{
    public class MapEnum : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            return c.SourceProp.Name == c.TargetProp.Name
                && c.SourceProp.Type.IsEnum
                && c.TargetProp.Type.IsEnum;
        }

        protected override object SetValue(ConventionInfo c)
        {
            return Enum.Parse(c.TargetProp.Type, c.SourceProp.Value.ToString());
        }
    }
}
