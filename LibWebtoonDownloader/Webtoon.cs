using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace LibWebtoonDownloader
{
    class Webtoon
    {
        private int titleId;

        private int startNo, endNo;

        public Webtoon()
        {
            titleId = startNo = endNo = -1;
        }

        public Webtoon(int titleId)
        {
            this.titleId = titleId;
            startNo = endNo = -1;
        }

        public Webtoon(int titleId, int no)
        {
            this.titleId = titleId;
            startNo = endNo = no;
        }

        public Webtoon(int )
    }
}
