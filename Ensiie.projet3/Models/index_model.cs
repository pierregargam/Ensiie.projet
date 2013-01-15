using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ensiie.projet3.Models
{
    public class SomeViewModel
    {
        public IEnumerable<News_> news;
        public IEnumerable<Theme_> themes;
        public IEnumerable<Like_news_> like;
        public IEnumerable<Like_news_> like_all;

        public SomeViewModel(IEnumerable<News_> n, IEnumerable<Theme_> t, IEnumerable<Like_news_> l, IEnumerable<Like_news_> l_a)
        {
            news = n;
            themes = t;
            like = l;
            like_all = l_a;
        }
    }
}