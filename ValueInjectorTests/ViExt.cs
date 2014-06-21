using System;
using System.Collections.Generic;
using Omu.ValueInjecter;

namespace ValueInjecterTests
{
    public static class ViExt
    {
        public static List<TTo> InjectFrom<TFrom, TTo>(this List<TTo> to, params IEnumerable<TFrom>[] sources) where TTo : new()
        {
            if (to == null)
            {
                throw new ArgumentNullException("to : You must initialize the source collection.");
            }

            foreach (var from in sources)
            {
                foreach (var source in from)
                {
                    var target = new TTo();
                    target.InjectFrom(source);
                    to.Add(target);
                }
            }
            return to;
        }
    }
}