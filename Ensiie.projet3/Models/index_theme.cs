using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensiie.projet3.Models
{
    public class index_theme
    {
        public IEnumerable<Theme_> themes;
        public IEnumerable<Abonnement_> abonnés;

        public index_theme(IEnumerable<Theme_> t, IEnumerable<Abonnement_> a)
        {
            themes = t;
            abonnés = a;
        }
    }
}